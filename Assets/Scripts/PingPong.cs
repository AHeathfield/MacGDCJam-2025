using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField] private Vector3 pointA = new Vector3(0, 0, 0); // Starting point
    [SerializeField] private Vector3 pointB = new Vector3(0, 0, 0); // Ending point
    public float speed = 1.0f; // Speed of the movement

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);
    }
}
