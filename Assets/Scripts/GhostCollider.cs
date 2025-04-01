using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GhostCollider: MonoBehaviour
{
    // [SerializeField] private GameObject spawnPoint;
    
    private bool isPlayerAlive = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && isPlayerAlive)
        {
            isPlayerAlive = false;
            
            // Getting rid of player movement
            PlayerController player = other.GetComponent<PlayerController>();
            player.DisableMovement();
            player.DisableTimeSwitch();

            // Death animation
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        FadeCanvas deathScreen = GetComponent<FadeCanvas>();
        deathScreen.RunFadeAnimations();
        yield return new WaitForSeconds(deathScreen.GetTotalDuration() - 2f);
        SceneController.instance.LoadStartMenu();
    }
}
