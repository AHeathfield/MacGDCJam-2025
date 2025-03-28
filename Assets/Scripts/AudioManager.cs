using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Background Music Tracks")]
    [SerializeField] private List<AudioClip> backgroundTracks = new List<AudioClip>(); // List to hold multiple tracks
    [SerializeField] private AudioLowPassFilter lowPassFilter;
    [SerializeField] private float normalCutoffFrequency = 22000f; // Normal frequency for the low-pass filter

    [Header("Silence Duration")]
    [SerializeField] private float minSilenceDuration = 10f;
    [SerializeField] private float maxSilenceDuration = 30f;

    private List<AudioClip> shuffledTracks = new List<AudioClip>(); // Holds shuffled tracks
    private int trackIndex = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Ensure the background music source is assigned
        if (musicSource == null)
        {
            musicSource = GetComponentInChildren<AudioSource>(); // Find the child with an AudioSource
        }

        // Ensure Low-Pass Filter is added to the correct GameObject (music child)
        if (musicSource != null)
        {
            lowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
            if (lowPassFilter == null)
                lowPassFilter = musicSource.gameObject.AddComponent<AudioLowPassFilter>();

            lowPassFilter.cutoffFrequency = normalCutoffFrequency; // Start with normal sound
        }
        else
        {
            Debug.LogError("No AudioSource found for background music!");
        }

        ShuffleTracks();
        StartCoroutine(PlayRandomMusic());
    }

    private void ShuffleTracks()
    {
        shuffledTracks = new List<AudioClip>(backgroundTracks); // Copy the list
        for (int i = 0; i < shuffledTracks.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledTracks.Count);
            (shuffledTracks[i], shuffledTracks[randomIndex]) = (shuffledTracks[randomIndex], shuffledTracks[i]); // Swap elements
        }
        trackIndex = 0; // Reset index
    }

    private IEnumerator PlayRandomMusic()
    {
        while (true)
        {
            if (shuffledTracks.Count == 0) yield break; // If no tracks are available, exit

            if (trackIndex >= shuffledTracks.Count) ShuffleTracks(); // If all tracks are played, reshuffle

            AudioClip selectedTrack = shuffledTracks[trackIndex];
            trackIndex++;
            musicSource.clip = selectedTrack;
            musicSource.Play();

            yield return new WaitForSeconds(selectedTrack.length); // Wait for the track to finish

            float silenceDuration = Random.Range(minSilenceDuration, maxSilenceDuration);
            yield return new WaitForSeconds(silenceDuration); // Wait before playing the next track
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
