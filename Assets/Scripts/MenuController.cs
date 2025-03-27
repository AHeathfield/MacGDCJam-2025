using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static MenuController instance;

    public bool isPaused = false;


    // Singleton, only 1 GameManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This class will handle all things that goes into menu (currently nothing but may be bigger)
    public void toggleMenu()
    {
        Debug.Log("Toggling pause menu");
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        SceneController.instance.PauseScene(isPaused); 
    }
}
