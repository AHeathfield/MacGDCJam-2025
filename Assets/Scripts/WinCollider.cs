using UnityEngine;

public class WinCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // This can be used for other things instead of just close application
        // Example, load next level, set a boolean to true, etc.
        if (other.gameObject.tag == "Player") 
        {
            // Basically if this is an exe it will quit game, but since were in editor it will
            // just stop playing
            #if UNITY_STANDALONE
                Application.Quit();
            #endif
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
