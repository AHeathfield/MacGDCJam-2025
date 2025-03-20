using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollider: MonoBehaviour
{
    // [SerializeField] private GameObject spawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            // For now Im just going to reload the scene
            // Debug.Log("Before setting: " + Player.timePos);
            // Player.timePos = spawnPoint.transform.position;
            // Player.timeRot = spawnPoint.transform.rotation;
            // Debug.Log("After setting: " + Player.timePos);
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }
}
