using UnityEngine;

// To trigger the popup text box
public class TextCollider : MonoBehaviour
{
    [SerializeField] private GameObject textMenu;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Debug.Log("Show textbox!");
            // DynamicTextEvents containerAnimation = textMenu.GetComponent<DynamicTextEvents>();
            // containerAnimation.FadeIn();
            textMenu.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Close textbox!");
            // DynamicTextEvents containerAnimation = textMenu.GetComponent<DynamicTextEvents>();
            // containerAnimation.FadeIn();
            textMenu.SetActive(false);
        }
    }
}
