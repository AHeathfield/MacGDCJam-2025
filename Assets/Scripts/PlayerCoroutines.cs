using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCoroutines : MonoBehaviour
{
    private ClosestSwitch closestSwitchPoint;
    private AudioManager audioManager;
    private bool isPresent = true;
    private bool canSwitch = true;

    void Awake()
    {
        // Maybe just change this to a Serialize field??
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager.PlaySFX(audioManager.timeChangeSFX);
    }

    // Handles the time change
    public void ChangeTime()
    {
        if (canSwitch)
        {
            closestSwitchPoint = this.GetComponent<ClosestSwitch>();
            StartCoroutine(TimeSwitch());
        }
    }

    // The coroutine for the time change 
    IEnumerator TimeSwitch()
    {
        canSwitch = false;
        audioManager.PlaySFX(audioManager.timeChangeSFX);

        TimeSwitchAnimations timeAnims = GetComponent<TimeSwitchAnimations>();
        timeAnims.RunTimeSwitchAnimations();
        yield return new WaitForSeconds(timeAnims.GetTimeFadeDuration());

        float currentX = closestSwitchPoint.getSwitchPoint().x;
        float currentZ = closestSwitchPoint.getSwitchPoint().z;
        if (isPresent)
        {
            transform.position = new Vector3(currentX, 49.5f, currentZ);
            Physics.SyncTransforms();
            isPresent = false;
        }
        else
        {
            transform.position = new Vector3(currentX, 0.05f, currentZ);
            Physics.SyncTransforms();
            isPresent = true;
        }

        audioManager.PlaySFX(audioManager.timeChangeSFX);
        canSwitch = true;
    }

}
