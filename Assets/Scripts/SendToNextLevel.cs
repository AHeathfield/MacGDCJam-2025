using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SendToNextLevel : MonoBehaviour
{
    [SerializeField] private float timeChangeDuration = 1f;
    [SerializeField] private Animator transitionAnim;

    public void OnTriggerEnter(Collider other)
    {
        // This can be used for other things instead of just close application
        // Example, load next level, set a boolean to true, etc.
        if (other.gameObject.tag == "Player") 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            StartCoroutine(LoadNextLevel(player));
        }
    }


    private IEnumerator LoadNextLevel(PlayerController player)
    { 
        player.toggleMove();
        transitionAnim.SetTrigger("Enter");
        yield return new WaitForSeconds(timeChangeDuration);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
}
