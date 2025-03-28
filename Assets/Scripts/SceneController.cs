using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float timeChangeDuration = 1f;
    [SerializeField] private Animator transitionAnim;
    // [SerializeField] private GameObject startMenu;

    public static SceneController instance;

    //I'm thinking we make this script the one that does all the scene transitions
    
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

        // LoadStartMenu();
    }
    
    public void LoadStartMenu()
    {
        StartCoroutine(LoadStartMenuRoutine());
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelRoutine());
    }

    public void LoadNextLevelFromUI(VisualElement fadeOverlay)
    {
        StartCoroutine(UILoadNextLevel(fadeOverlay));
    }

    public void ExitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Stops everything in scene from moving
    public void PauseScene(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }

    private IEnumerator LoadNextLevelRoutine()
    { 
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(timeChangeDuration);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("Start");
    }

    private IEnumerator LoadStartMenuRoutine()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(timeChangeDuration);
        SceneManager.LoadSceneAsync("StartMenu");
        // startMenu.SetActive(true);
        transitionAnim.SetTrigger("Start");
    }

    private IEnumerator UILoadNextLevel(VisualElement fadeOverlay)
    {
        // This transiiton won't show since the UI ToolKit is separate from the actual scene, but
        // this is needed so next scene starts with a black screen
        transitionAnim.SetTrigger("End");

        fadeOverlay.style.visibility = Visibility.Visible;
        fadeOverlay.style.opacity = 0;

        float elapsedTime = 0f;
        while (elapsedTime < timeChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / timeChangeDuration);
            fadeOverlay.style.opacity = alpha;
            yield return null; // Wait for next frame
        }

        fadeOverlay.style.opacity = 1; // Ensure fully opaque
        Debug.Log("Fade to black complete!");

        // A slight pause
        // yield return new WaitForSeconds(0.2f); 
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        // startMenu.SetActive(false);
        transitionAnim.SetTrigger("Start");
    }
}
