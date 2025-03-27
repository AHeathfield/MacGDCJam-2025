using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCollider: MonoBehaviour
{
    // [SerializeField] private GameObject spawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            //Debug.Log("Player collided with ghost");
            //string currentScene = SceneManager.GetActiveScene().name;
            //Load the start menu scene
            SceneManager.LoadScene("StartMenu");
        }
    }
}
