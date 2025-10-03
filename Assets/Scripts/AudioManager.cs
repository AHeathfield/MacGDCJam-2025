using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Background Music Tracks")]
    [SerializeField] private List<AudioClip> backgroundTracks = new List<AudioClip>();
    [SerializeField] private AudioClip startTrack;
    //[SerializeField] private AudioLowPassFilter lowPassFilter;
    //[SerializeField] private float normalCutoffFrequency = 22000f;

    [Header("Silence Duration")]
    [SerializeField] private float minSilenceDuration = 10f;
    [SerializeField] private float maxSilenceDuration = 30f;
    [SerializeField] private float openingSceneDur = 10f;

    public static AudioManager instance;

    private List<AudioClip> shuffledTracks = new List<AudioClip>();
    private int trackIndex = 0;
    private Coroutine musicCoroutine;
    private AudioClip currentSceneClip;
    private bool isPlayingSceneTrack = false;

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

    private void Start()
    {
        // Ensure the background music source is assigned
        if (musicSource == null)
        {
            musicSource = GetComponentInChildren<AudioSource>();
        }

        // Ensure Low-Pass Filter is setup
        if (musicSource != null)
        {
            // lowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
            // if (lowPassFilter == null)
            //     lowPassFilter = musicSource.gameObject.AddComponent<AudioLowPassFilter>();

            // lowPassFilter.cutoffFrequency = normalCutoffFrequency;
        }
        else
        {
            Debug.LogError("No AudioSource found for background music!");
        }
        
        StartCoroutine(OpeningSceneWait());
        // StartRandomPlaylist();
    }

    private void StartRandomPlaylist()
    {
        ShuffleTracks();
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }
        musicCoroutine = StartCoroutine(PlayRandomMusic());
    }

    private void ShuffleTracks()
    {
        shuffledTracks = new List<AudioClip>(backgroundTracks);
        for (int i = 0; i < shuffledTracks.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledTracks.Count);
            (shuffledTracks[i], shuffledTracks[randomIndex]) = (shuffledTracks[randomIndex], shuffledTracks[i]);
        }
        trackIndex = 0;
    }

    private IEnumerator PlayRandomMusic()
    {
        while (true)
        {
            if (shuffledTracks.Count == 0) yield break;

            if (trackIndex >= shuffledTracks.Count) ShuffleTracks();

            AudioClip selectedTrack = shuffledTracks[trackIndex];
            trackIndex++;
            musicSource.clip = selectedTrack;
            musicSource.Play();
            Debug.Log("Now Playing: " + selectedTrack.name);

            yield return new WaitForSeconds(selectedTrack.length);

            float silenceDuration = Random.Range(minSilenceDuration, maxSilenceDuration);
            yield return new WaitForSeconds(silenceDuration);
        }
    }

    public void PlaySceneTrack(AudioClip clip)
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }

        currentSceneClip = clip;
        isPlayingSceneTrack = true;
        musicSource.clip = clip;
        musicSource.loop = false;

        // Disable low-pass filter for scene tracks
        // if (lowPassFilter != null)
        // {
        //     lowPassFilter.enabled = false;
        // }

        musicSource.Play();
        StartCoroutine(WaitForSceneTrackToEnd());
    }

    private IEnumerator WaitForSceneTrackToEnd()
    {
        yield return new WaitForSeconds(currentSceneClip.length);
        
        if (isPlayingSceneTrack && musicSource.clip == currentSceneClip)
        {
            isPlayingSceneTrack = false;
            currentSceneClip = null;

            // Re-enable the low-pass filter when returning to the playlist
            // if (lowPassFilter != null)
            // {
            //     lowPassFilter.enabled = true;
            // }

            StartRandomPlaylist();
        }
    }


    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopAllMusic()
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }
        musicSource.Stop();
        isPlayingSceneTrack = false;
        currentSceneClip = null;
    }

    private IEnumerator OpeningSceneWait()
    {
        yield return new WaitForSeconds(openingSceneDur);
        // StartRandomPlaylist();
        PlaySceneTrack(startTrack);
    }
}
