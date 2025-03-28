using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.CompilerServices;

// Currently this script just handles coroutine when player moves to the future
public class PlayerCoroutines : MonoBehaviour
{
    private ClosestSwitch closestSwitchPoint;
    private AudioManager audioManager;
    private bool isPresent = true;
    private bool canSwitch = true;
    private GameObject[] timeReapers;
    private GameObject[] guards;
    private LightSwitch lightSwitch;

    void Awake()
    {
        // Maybe just change this to a Serialize field??
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //audioManager.PlaySFX(audioManager.timeChangeSFX);
        timeReapers = GameObject.FindGameObjectsWithTag("TimeReaper");
        guards = GameObject.FindGameObjectsWithTag("Guard");
        lightSwitch = FindAnyObjectByType<LightSwitch>();
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
        //audioManager.PlaySFX(audioManager.timeChangeSFX);

        TimeSwitchAnimations timeAnims = GetComponent<TimeSwitchAnimations>();
        timeAnims.RunTimeSwitchAnimations();
        yield return new WaitForSeconds(timeAnims.GetTimeFadeDuration());

        float currentX = closestSwitchPoint.getSwitchPoint().x;
        float currentZ = closestSwitchPoint.getSwitchPoint().z;
        if (isPresent)
        {
            transform.position = new Vector3(currentX, 100.0f, currentZ);
            Physics.SyncTransforms();
            isPresent = false;
        }
        else
        {
            transform.position = new Vector3(currentX, 0.05f, currentZ);
            Physics.SyncTransforms();
            isPresent = true;
        }

        // Ghost(s) will start following you
        
        foreach (GameObject reaper in timeReapers)
        {
            // Will toggle follow to true when going to future, false when going to present
            reaper.GetComponent<FollowPlayer>().toggleFollow();
        }

        foreach (GameObject guard in guards)
        {
            // Will toggle follow to true when in present, false when in future
            guard.GetComponent<EnemyAI>().toggleFollow();
        }

        if (lightSwitch != null)
        {
            lightSwitch.toggleLight();
        }

        //audioManager.PlaySFX(audioManager.timeChangeSFX);
        canSwitch = true;
    }

}
