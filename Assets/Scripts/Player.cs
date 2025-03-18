using UnityEngine;

// This will contain globals about the Player that we can use when we switch time
public class Player : MonoBehaviour
{   
    // Public globals, may be bad, if it is we will fix
    public static Vector3 timePos = new Vector3(0.0f, 0.0f, 0.0f);
    public static Quaternion timeRot = new Quaternion();
    public static float timeHealth = 0.0f;


    // If we make them private, I don't think it matter currently if its public
    // Static getters so we can do Player.GetPosition() for scene transitions
    // public static Vector3 GetPosition() { return currentPos; }
    // public static Quaternion GetRotation() { return currentRot; }
    // public static float currentHealth
}
