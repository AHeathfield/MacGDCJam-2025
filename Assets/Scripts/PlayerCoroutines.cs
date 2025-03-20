using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCoroutines : MonoBehaviour
{
    [Header("TimeChange Settings")]
    [SerializeField] private float timeChangeDuration = 1.2f;
    [SerializeField] Animator transitionAnim;

    private ClosestSwitch closestSwitchPoint;

    AudioManager audioManager;
    private bool hasTimeSwapped = false;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hasTimeSwapped)
        {
            transitionAnim.SetTrigger("Exit");
            audioManager.PlaySFX(audioManager.timeChangeSFX);
        }
        hasTimeSwapped = true;
    }

    // Handles the time change
    public void ChangeTime()
    {
        closestSwitchPoint = this.GetComponent<ClosestSwitch>();
        Player.timePos = closestSwitchPoint.getSwitchPoint();
        Player.timeRot = transform.rotation;
        Player.timeHealth = GetComponent<Health>().GetHealth();
        StartCoroutine(TimeSwitch());
    }

    // The coroutine for the time change 
    IEnumerator TimeSwitch()
    {
        audioManager.PlaySFX(audioManager.timeChangeSFX);
        transitionAnim.SetTrigger("Enter");
        yield return new WaitForSeconds(timeChangeDuration);
        if (SceneManager.GetActiveScene().name.Equals("Present"))
        {
            Debug.Log("Switching to Future");
            SceneManager.LoadScene("Future");
        }
        else {
            Debug.Log("Switching to Past");
            SceneManager.LoadScene("Present");
        }
    }

}
