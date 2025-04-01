using UnityEngine;

public class GhostSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource ghostSFX;
    private bool isPlayerInFuture = false;

    public void ToggleSFX()
    {
        if (isPlayerInFuture)
        {
            isPlayerInFuture = false;
            ghostSFX.Stop();
        }
        else 
        {
            isPlayerInFuture = true;
            ghostSFX.Play();
        }
    }
}
