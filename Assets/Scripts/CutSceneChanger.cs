using UnityEngine;

public class CutSceneChanger : MonoBehaviour
{
    [SerializeField] private float cutSceneDuration;
    
    private bool isNotLoading = true;   // So it only loads once!

    private void Update()
    {
        cutSceneDuration -= Time.deltaTime;
        if (cutSceneDuration <= 0 && isNotLoading)
        {
            isNotLoading = false;
            SceneController.instance.LoadStartMenu();
        }
    }
}
