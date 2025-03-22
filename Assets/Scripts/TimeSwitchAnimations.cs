using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

// Might rename class to TimeSwitchAnimations with the purpose of handling all animations
// When switching time
public class TimeSwitchAnimations : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackScreenTime = 5f;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private bool canFade = true;
    

    private void Start()
    {
        transitionAnim.SetTrigger("Exit");
    }

    public float GetTimeFadeDuration() { return fadeDuration; }

    public void RunTimeSwitchAnimations()
    {
        if (canFade)
        {
            Debug.Log("Starting Time animation");
            StartCoroutine(FadeInAndOut());
        }
    }

    // This one basically runs Fade in and fade out using the FadeScreen routine
    private IEnumerator FadeInAndOut()
    {
        canFade = false;

        // Fade to black
        // Debug.Log("Fading to black");
        yield return StartCoroutine(FadeScreen(0f, 1f));

        yield return new WaitForSeconds(blackScreenTime);   // Extra black screen time

        yield return StartCoroutine(FadeScreen(1f, 0f));

        canFade = true;
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        
        // Fading happens here
        while (elapsedTime < fadeDuration)
        {
            // Debug.Log("Black alpha = " + fadeCanvasGroup.alpha);
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;  // Basically tells Unity to pause for 1 frame before continuing
        }
        fadeCanvasGroup.alpha = endAlpha;   // ensuring it's either fully black or fully clear
    }

}
