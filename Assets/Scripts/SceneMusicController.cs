using UnityEngine;

// For the final scene, pausing the global audio and just using own since I want to play cursed knowledge :)
public class SceneMusicController : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start()
    {
        AudioManager.instance.PlaySceneTrack(music);
    }
}
