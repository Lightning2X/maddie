using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float timeTillDestruct;

    private void Start() {
        StartCoroutine(destruct());
    }

    private IEnumerator destruct(){
        yield return new WaitForSecondsRealtime(timeTillDestruct);
        Destroy(gameObject);
    }
}
