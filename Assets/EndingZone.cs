using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingZone : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
       if(other.gameObject.layer == 3) {
           PlayerPrefs.SetInt("finished", 1);
            SceneManager.LoadScene("Menu");
       }
        
   }
}
