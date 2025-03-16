using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private Health playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerHealth = GetComponent<Health>();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            playerHealth.ChangeHealth(-1 * enemy.GetDamage());
            Debug.Log(playerHealth.GetHealth());
        }
    }
}
