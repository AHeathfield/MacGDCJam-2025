//using UnityEditor.UI;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private GameObject directionalLight;
    private Light futureLight;
    
    private bool isPlayerInFuture = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        futureLight = directionalLight.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        futureLight.enabled = isPlayerInFuture;
    }

    public void toggleLight()
    {
        isPlayerInFuture = !isPlayerInFuture;
        Debug.Log("Switching Light: " + isPlayerInFuture);
        
        //directionalLight.SetActive(isPlayerInFuture);
    }
}
