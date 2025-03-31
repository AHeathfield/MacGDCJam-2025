using UnityEngine;

public class EvilPortalCollider : MonoBehaviour
{
    [SerializeField] private GameObject timeReaper;

    // Basically you can't move and ghost comes get you
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.DisableMovement();
            player.DisableTimeSwitch();
            timeReaper.SetActive(true);
            timeReaper.GetComponent<FollowPlayer>().toggleFollow();
        }
    }
}
