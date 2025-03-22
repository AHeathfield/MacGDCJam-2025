using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float rotateSpeed = 10f;

    private bool isPlayerInFuture = false;

    void Update()
    {
        if (player == null) return;

        if (isPlayerInFuture)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
    }

    public void toggleFollow() {
        isPlayerInFuture = !isPlayerInFuture;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        transform.position += direction * speed * Time.deltaTime;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Keep movement in the horizontal plane
        return direction.normalized;
    }
}
