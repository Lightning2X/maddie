using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    // Singleton
    public static GameController instance;

    [Tooltip("Audiosource for all things in the game")]
    public AudioSource audioSource;

    public GameObject player;

    private Checkpoint checkpoint;

    public static AudioSource AudioSource => instance.audioSource;

    public static GameObject Player => instance.player;

    public static Checkpoint ActiveCheckpoint => instance.checkpoint;

    private UnityEvent gameOverEvent;

    public void Awake()
    {
        if(gameOverEvent == null) 
            gameOverEvent = new UnityEvent();
        instance = this;
    }


    public void PlayClip(string path) { 
        AudioClip audioClip = (AudioClip)Resources.Load("SFX/" + path);
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayMusic(AudioClip audioClip) {
        if(audioSource.clip != audioClip)  {
            audioSource.loop = true;
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
    public void PlayMusic(string path)
    {
        AudioClip audioClip = (AudioClip)Resources.Load("SFX/" + path);
        PlayMusic(audioClip);
    }

    public void SetActiveCheckpoint(Checkpoint newCheckPoint) { 
        if(checkpoint != null)  {
            checkpoint.isActive = false;
        }
        newCheckPoint.isActive = true;
        checkpoint = newCheckPoint;
    }

    public void AddResetListener(UnityAction listener) { 
        this.gameOverEvent.AddListener(listener);
    }

    public void GameOver() {
        this.PlayClip("crunch");
        this.gameOverEvent.Invoke();
        player.transform.position = checkpoint.respawnPos;
    }

}
