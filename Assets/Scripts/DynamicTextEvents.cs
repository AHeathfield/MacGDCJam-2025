using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This is for the StartScreenMenu, aka new start screen
public class DynamicTextEvents : MonoBehaviour
{
    [SerializeField] private string speaker;
    [SerializeField] private string message;
    // [SerializeField] private float fadeDuration = 1f;

    private UIDocument _document;
    private VisualElement _container;

    // private AudioSource _audioSource;

    // Using OnEnable, everytime we enable the obj, it will run this (takes less space over time)
    private void OnEnable()
    {
        // _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        // Setting up containers
        _container = _document.rootVisualElement.Q("Container");
        _container.style.opacity = 0f;
        
        // Getting elements
        Label speakerLabel = _container.Q<Label>("Speaker");
        Label messageLabel = _container.Q<Label>("Message");
        
        // Overwriting them
        speakerLabel.text = speaker;
        messageLabel.text = message;

        // Fade in
        FadeIn();
    }

    private void OnDisable()
    {
        FadeOut();
    }

    // Fade in
    public void FadeIn()
    {
        _container.style.opacity = 1f;
        // float elapsedTime = 0f;
        //
        // while (elapsedTime < fadeDuration)
        // {
        //     elapsedTime += Time.deltaTime;
        //     float opacity = Mathf.Clamp01(elapsedTime / fadeDuration);  // Clamp01 makes sure value is between 0 and 1
        //     _container.style.opacity = opacity;
        // }
    }

    // Fade Out
    public void FadeOut()
    {
        _container.style.opacity = 0f;
        // float elapsedTime = 0f;
        //
        // while (elapsedTime < fadeDuration)
        // {
        //     elapsedTime += Time.deltaTime;
        //     float opacity = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);  // Clamp01 makes sure value is between 0 and 1
        //     _container.style.opacity = opacity;
        // }
    }

    // private IEnumerator Fade(float endOpacity)
    // {
    //     float startOpacity = _container.style.opacity;
    //     float elapsedTime = 0f;
    //
    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         _container.style.opacity = Mathf.Lerp(startOpacity, endOpacity, elapsedTime / fadeDuration);
    //         yield return null;
    //     }
    //
    //     _container.style.opacity = endOpacity;
    // }
}
