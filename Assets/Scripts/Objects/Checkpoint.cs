using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive;

    private bool isActiveStoredValue;

    public GameObject glimmerParticles;

    public Vector2 respawnPos;

    private GameObject currentParticles;

    private void Awake()
    {
        isActiveStoredValue = isActive;

        if (isActive)
        {
            currentParticles = Instantiate(glimmerParticles, this.transform);
            GameController.instance.SetActiveCheckpoint(this);
        }
        
        respawnPos = new Vector2(this.transform.position.x, this.transform.position.y + 1);
    }

    private void Update()
    {
        if (isActive != isActiveStoredValue)
        {
            // we were active, deactivate glimmer
            if (isActiveStoredValue)
            {
                Destroy(currentParticles);
            }
            else
            {
                currentParticles = Instantiate(glimmerParticles, this.transform);
            }

            isActiveStoredValue = isActive;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 3) { 
            GameController.instance.SetActiveCheckpoint(this);
        }
    }


}
