using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> playlist;  
    public bool shuffle = true;     
    public bool loop = true;        

    private AudioSource audioSource;
    private List<AudioClip> shuffledPlaylist;
    private int currentTrackIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playlist.Count == 0)
        {
            return;
        }

        if (shuffle)
        {
            ShufflePlaylist();
        }
        else
        {
            shuffledPlaylist = new List<AudioClip>(playlist);
        }
        PlayNextTrack();
    }
    void Update()
    {
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            PlayNextTrack();
        }
    }
    private void ShufflePlaylist()
    {
        shuffledPlaylist = new List<AudioClip>(playlist);
        for (int i = 0; i < shuffledPlaylist.Count; i++)
        {
            AudioClip temp = shuffledPlaylist[i];
            int randomIndex = Random.Range(i, shuffledPlaylist.Count);
            shuffledPlaylist[i] = shuffledPlaylist[randomIndex];
            shuffledPlaylist[randomIndex] = temp;
        }
    }
    private void PlayNextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= shuffledPlaylist.Count)
        {
            if (loop)
            {
                currentTrackIndex = 0;
                if (shuffle)
                {
                    ShufflePlaylist();
                }
            }
            else
            {
                return;
            }
        }
        audioSource.clip = shuffledPlaylist[currentTrackIndex];
        audioSource.Play();
    }
    public void StopPlayback()
    {
        audioSource.Stop();
    }
    public void PausePlayback()
    {
        audioSource.Pause();
    }
    public void ResumePlayback()
    {
        audioSource.UnPause();
    }
}
