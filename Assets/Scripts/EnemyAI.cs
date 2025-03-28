using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float pointWaitTime = 1.5f;
    [SerializeField] private float guardSpeed;

    [Header("Player Detection Settings")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Icons and Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject questionMark;

    private NavMeshAgent _navMeshAgent;
    private int _currentPatrolPointIndex = 0;
    private bool waiting = false;
    private bool playerInSight = false;
    private Vector3 lastKnownPlayerPosition;
    private bool investigating = false;
    private bool isPlayerInFuture = false;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Count > 0)
        {
            MoveToNextPoint();
        }

        if (exclamationMark != null) exclamationMark.SetActive(false); // Hide at start
        if (questionMark != null) questionMark.SetActive(false); // Hide at start
    }

    void Update()
    {
        if (isPlayerInFuture)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        _navMeshAgent.isStopped = false; // Resume movement

        CheckLineOfSight();

        if (playerInSight)
        {
            //Debug.Log("Player in sight");
            ChasePlayer();
        }
        else if (investigating)
        {
            //Debug.Log("Investigating");
            Investigate();
        }
        else
        {
            //Debug.Log("Patrolling");
            Patrol();
        }

        animator.SetBool("isWalking", _navMeshAgent.velocity.magnitude > 0.1f);
        
        // if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !waiting)
        // {
        //     StartCoroutine(WaitAndMove());
        // }

        // if (exclamationMark != null)
        //     exclamationMark.SetActive(playerInSight); // Show/hide icon
    }

    public void toggleFollow() {
        isPlayerInFuture = !isPlayerInFuture;
        _navMeshAgent.isStopped = isPlayerInFuture; // Stop or resume the agent

        if (isPlayerInFuture)
        {
            animator.SetBool("isWalking", false); // Ensure walking stops when frozen
        }
        else
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                MoveToNextPoint(); // Resume patrol if not chasing
            }
        }
    }

    private void CheckLineOfSight()
    {
        bool wasInSight = playerInSight;
        playerInSight = false;

        // Check if the player is within sight range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > sightRange)
        {
            if (wasInSight) OnPlayerLost();
            return;
        }

        //Get direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //Check if player is within vision angle
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > visionAngle / 2)
        {
            if (wasInSight) OnPlayerLost();
            return;
        }

        //Send out a raycast to check for obstacles
        if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange, obstacleMask))
        {
            playerInSight = true;
            lastKnownPlayerPosition = player.position;
        }
        else if (wasInSight)
        {
            OnPlayerLost();
        }

        Debug.DrawRay(transform.position, directionToPlayer * sightRange, playerInSight ? Color.green : Color.red);
    }

    void ChasePlayer()
    {
        //Show exclamation mark and hide question mark
        if (exclamationMark != null) exclamationMark.SetActive(true);
        if (questionMark != null) questionMark.SetActive(false);

        //Make the guard faster when chasing the player
        _navMeshAgent.speed = 12.5f;

        _navMeshAgent.SetDestination(player.position);
    }

    void Investigate()
    {
        //Show question mark and hide exclamation mark
        if (exclamationMark != null) exclamationMark.SetActive(false);
        if (questionMark != null) questionMark.SetActive(true);

        _navMeshAgent.speed = guardSpeed;

        if (!_navMeshAgent.hasPath)
            _navMeshAgent.SetDestination(lastKnownPlayerPosition);
        // else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        //     _navMeshAgent.SetDestination(lastKnownPlayerPosition);

        //If the enemy reaches the last known player position, return to patrolling
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= 0.1f)
        {
            investigating = false;
            if (questionMark != null) questionMark.SetActive(false);
            MoveToNextPoint();
        }
    }

    void Patrol()
    {
        //Hide both icons
        if (exclamationMark != null) exclamationMark.SetActive(false);
        if (questionMark != null) questionMark.SetActive(false);

        _navMeshAgent.speed = guardSpeed;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !waiting)
        {
            animator.SetBool("isWalking", false);
            StartCoroutine(WaitAndMove());
        }
    }

    private void MoveToNextPoint()
    {
        if (patrolPoints.Count == 0) return;

        _navMeshAgent.SetDestination(patrolPoints[_currentPatrolPointIndex].position);
        _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % patrolPoints.Count; // Loop back to the start
    }

    public void OnPlayerLost()
    {
        if (!playerInSight) investigating = true;
    }

    private IEnumerator WaitAndMove()
    {
        
        waiting = true;
        yield return new WaitForSeconds(pointWaitTime);
        waiting = false;
        animator.SetBool("isWalking", true);
        MoveToNextPoint();
    }

    private void OnDrawGizmos()
{
    if (player == null) return;

    // Set Gizmo color (visible in Scene view)
    Gizmos.color = Color.yellow;

    // Draw the detection range circle
    Gizmos.DrawWireSphere(transform.position, sightRange);

    // Draw the field of view
    Vector3 forward = transform.forward;
    float halfAngle = visionAngle * 0.5f;

    Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * forward;
    Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * forward;

    Gizmos.DrawRay(transform.position, leftBoundary * sightRange);
    Gizmos.DrawRay(transform.position, rightBoundary * sightRange);

    // Optionally, draw a line to the player if in sight
    if (playerInSight)
    {
        Gizmos.color = Color.green; // Player is visible
        Gizmos.DrawLine(transform.position, player.position);
    }
}
    
    // // Reference to the NavMeshAgent component for movement
    // public NavMeshAgent agent;
    // // Reference to the player's transform
    // public Transform player;
    // // Layers to identify ground and player
    // public LayerMask whatIsGround, whatIsPlayer;

    // // Patroling variables
    // public Vector3 walkPoint; // Target point for patrolling
    // bool walkPointSet; // Whether a walk point is set
    // public float walkPointRange; // Range within which walk points are chosen

    // // Attacking variables
    // public float timeBetweenAttacks; // Time delay between attacks
    // bool alreadyAttacked; // Whether the enemy has already attacked

    // // State variables
    // public float sightRange, attackRange; // Ranges for sight and attack
    // public bool playerInSightRange, playerInAttackRange; // Flags for player detection

    // private void Awake()
    // {
    //     // Find the player object and get its transform
    //     player = GameObject.Find("Player").transform;
    //     // Get the NavMeshAgent component attached to this object
    //     agent = GetComponent<NavMeshAgent>();
    // }

    // private void Update()
    // {
    //     // Check if the player is within sight or attack range
    //     playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    //     playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    //     // Decide behavior based on player's position relative to the enemy
    //     if (!playerInSightRange && !playerInAttackRange) Patrolling(); // Patrol if player is not detected
    //     if (playerInSightRange && !playerInAttackRange) ChasePlayer(); // Chase if player is in sight but out of attack range
    //     if (playerInSightRange && playerInAttackRange) AttackPlayer(); // Attack if player is in attack range
    // }

    // private void Patrolling()
    // {
    //     // If no walk point is set, find a new one
    //     if (!walkPointSet) SearchWalkPoint();

    //     // Move to the walk point if set
    //     if (walkPointSet)
    //         agent.SetDestination(walkPoint);

    //     // Calculate distance to the walk point
    //     Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //     // If the walk point is reached, reset the flag
    //     if (distanceToWalkPoint.magnitude < 1f)
    //         walkPointSet = false;
    // }

    // private void SearchWalkPoint()
    // {
    //     // Generate a random point within the defined range
    //     float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //     float randomX = Random.Range(-walkPointRange, walkPointRange);

    //     // Set the walk point relative to the enemy's current position
    //     walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //     // Check if the walk point is on the ground
    //     if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //         walkPointSet = true;
    // }

    // private void ChasePlayer()
    // {
    //     // Set the player's position as the destination
    //     agent.SetDestination(player.position);
    // }

    // private void AttackPlayer()
    // {
    //     // Stop moving while attacking
    //     agent.SetDestination(transform.position);

    //     // Face the player
    //     transform.LookAt(player);

    //     if (!alreadyAttacked)
    //     {
    //         // Attack logic goes here (e.g., shooting, melee attack)
    //         alreadyAttacked = true;
    //         // Reset attack after a delay
    //         Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //     }
    // }

    // private void ResetAttack()
    // {
    //     // Allow the enemy to attack again
    //     alreadyAttacked = false;
    // }

    // private void OnDrawGizmosSelected()
    // {
    //     // Draw a red sphere to visualize the attack range in the editor
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);
    //     // Draw a yellow sphere to visualize the sight range in the editor
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, sightRange);
    // }
}
