using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollider: MonoBehaviour
{
    // [SerializeField] private GameObject spawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }
}
