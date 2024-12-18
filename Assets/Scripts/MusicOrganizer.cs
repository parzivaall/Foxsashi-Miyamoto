using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicOrganizer : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip[] sceneMusicTracks;

    public void setVolume(float volume){
        musicSource.volume = volume;
    }

    public float getVolume(){
        return musicSource.volume;
    }

    public void PlaySceneMusic(string sceneName)
    {
        // Select the appropriate music track for the scene
        AudioClip trackToPlay = null;

        switch (sceneName)
        {
            case "MainMenu":
                trackToPlay = sceneMusicTracks[1]; // Assign the corresponding track index
                break;
            case "Shop":
                trackToPlay = sceneMusicTracks[1];
                break;
            case "GameOver":
                trackToPlay = sceneMusicTracks[2];
                break;
            case "PipeYard":
                trackToPlay = sceneMusicTracks[3];
                break;
            case "Expanse":
                trackToPlay = sceneMusicTracks[4];
                break;
            default:
                trackToPlay = sceneMusicTracks[0]; // Default to first track if none specified
                break;
        }

        // Only play if it's a new track
        if (musicSource.clip != trackToPlay)
        {
            StartCoroutine(CrossfadeToNewTrack(trackToPlay));
        }
    }

    System.Collections.IEnumerator CrossfadeToNewTrack(AudioClip newTrack)
    {
        // Fade out current track
        for (float volume = musicSource.volume; volume > 0; volume -= Time.deltaTime)
        {
            musicSource.volume = volume;
            yield return null;
        }

        // Switch tracks and fade in the new track
        musicSource.clip = newTrack;
        musicSource.Play();

        for (float volume = 0; volume < 1f; volume += Time.deltaTime)
        {
            musicSource.volume = volume;
            yield return null;
        }
    }
}