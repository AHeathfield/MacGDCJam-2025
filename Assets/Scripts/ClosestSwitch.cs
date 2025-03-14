using UnityEngine;

public class ClosestSwitch : MonoBehaviour
{
    private Vector3 closestSwitch;

    public void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals("SwitchPoint"))
        {
            closestSwitch = other.transform.position;
            Debug.Log(closestSwitch);
        }
    }

    public Vector3 getSwitchPoint() { return closestSwitch; }
}
