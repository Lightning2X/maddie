using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    
    public AudioClip trackToPlay;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 3){
            GameController.instance.PlayMusic(trackToPlay);
        }
    }

}
