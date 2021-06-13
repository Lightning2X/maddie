using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioClip mainMenu;

    public AudioClip credits;

    public AudioSource audioSource;
    private void Start() {
        if(PlayerPrefs.GetInt("finished", 0) == 1) {
            audioSource.clip = credits;
        }
        else {
            audioSource.clip = mainMenu;
        }

        audioSource.Play();
    }
    
    public void PlayButton(){
        SceneManager.LoadScene("Main");
    }

    public void Quit(){
        Application.Quit();
    }



}
