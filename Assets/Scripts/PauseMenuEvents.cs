using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This is for the StartScreenMenu, aka new start screen
public class PauseMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _mainContainer;
    private VisualElement _controlsContainer;
    // private VisualElement _fadeOverlay;
    private Button _mainContinueButton;
    private Button _mainControlsButton;
    private Button _mainExitButton;
    private Button _controlsBackButton;
    private List<Button> _menuButtons = new List<Button>();

    // Sound when button clicked
    private AudioSource _audioSource;
    // private bool isActive = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        // Setting up containers
        _mainContainer = _document.rootVisualElement.Q("Main");
        _controlsContainer = _document.rootVisualElement.Q("Controls");
        // _fadeOverlay = _document.rootVisualElement.Q("FadeOverlay");
        
        // Setting up buttons
        _mainContinueButton = _mainContainer.Q<Button>("MainContinueButton");   // cast to button
        _mainControlsButton = _mainContainer.Q<Button>("MainControlsButton");
        _mainExitButton = _mainContainer.Q<Button>("MainExitGameButton");
        _controlsBackButton = _controlsContainer.Q<Button>("ControlsBackButton");

        // Register callbacks
        _mainContinueButton.RegisterCallback<ClickEvent>(OnMainContinueClick);
        _mainControlsButton.RegisterCallback<ClickEvent>(OnMainControlsClick);
        _mainExitButton.RegisterCallback<ClickEvent>(OnMainExitClick);
        _controlsBackButton.RegisterCallback<ClickEvent>(OnControlsBackClick);

        // For all buttons
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }


    private void OnDisable()
    {
        _mainContinueButton.UnregisterCallback<ClickEvent>(OnMainContinueClick);
        _mainControlsButton.UnregisterCallback<ClickEvent>(OnMainControlsClick);
        _mainExitButton.UnregisterCallback<ClickEvent>(OnMainExitClick);
        _controlsBackButton.UnregisterCallback<ClickEvent>(OnControlsBackClick);
    }


    // ===================== Specific Buttons ===================
    // ===================== Main ==========================
    private void OnMainContinueClick(ClickEvent evt)
    {
        // Load next scene
        Debug.Log("Unpausing game...");
        MenuController.instance.toggleMenu();
        // gameObject.SetActive(false);
    }

    private void OnMainControlsClick(ClickEvent evt)
    {
        // Make #Main container invisible and #Controls visible
        Debug.Log("You pressed the controls button");
        _mainContainer.style.display = DisplayStyle.None;
        _controlsContainer.style.display = DisplayStyle.Flex;
    }

    private void OnMainExitClick(ClickEvent evt)
    {
        Debug.Log("Exiting Game...");
        SceneController.instance.ExitGame();
    }

    // ==================== Controls ========================
    private void OnControlsBackClick(ClickEvent evt)
    {
        Debug.Log("You pressed the back button in controls");
        _controlsContainer.style.display = DisplayStyle.None;
        _mainContainer.style.display = DisplayStyle.Flex;
    }

    // Whenever any button is clicked it will run this method
    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }
}
