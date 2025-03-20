using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float pointWaitTime = 1.5f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Image alertIcon;

    private NavMeshAgent _navMeshAgent;
    private int _currentPatrolPointIndex = 0;
    private bool waiting = false;
    private bool playerInSight;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Count > 0)
        {
            MoveToNextPoint();
        }

        if (alertIcon != null) alertIcon.gameObject.SetActive(false); // Hide at start
    }

    void Update()
    {
        CheckLineOfSight();

        if (playerInSight)
        {
            Debug.Log("Player in sight");
            //Play some sort of alert animation
            _navMeshAgent.SetDestination(player.position);
        }
        
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !waiting)
        {
            StartCoroutine(WaitAndMove());
        }

        if (alertIcon != null)
            alertIcon.gameObject.SetActive(playerInSight); // Show/hide icon
    }

    private void CheckLineOfSight()
    {
        playerInSight = false;

        // Check if the player is within sight range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > sightRange) return;

        //Get direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //Check if player is within vision angle
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > visionAngle / 2) return;

        //Send out a raycast to check for obstacles
        if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange, obstacleMask))
        {
            playerInSight = true;
        }
        else
        {
            Debug.Log("Obstacle hit: " + hit.collider.name);
        }

        Debug.DrawRay(transform.position, directionToPlayer * sightRange, playerInSight ? Color.green : Color.red);
    }

    private void MoveToNextPoint()
    {
        if (patrolPoints.Count == 0) return;

        _navMeshAgent.SetDestination(patrolPoints[_currentPatrolPointIndex].position);
        _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % patrolPoints.Count; // Loop back to the start
    }

    private IEnumerator WaitAndMove()
    {
        waiting = true;
        yield return new WaitForSeconds(pointWaitTime);
        waiting = false;
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
}
