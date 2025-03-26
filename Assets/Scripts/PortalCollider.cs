using UnityEngine;

public class PortalCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // This can be used for other things instead of just close application
        // Example, load next level, set a boolean to true, etc.
        if (other.gameObject.tag == "Player") 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            // SendToNextLevel levelScript = GetComponent<SendToNextLevel>();
            player.toggleMove();
            // StartCoroutine(levelScript.LoadNextLevel());
            SceneController.instance.LoadNextLevel();
        }
    }
}
