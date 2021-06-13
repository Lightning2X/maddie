using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScythController : MonoBehaviour
{

    [SerializeField]
    private GameObject Scyth;
    [SerializeField]
    private Transform scythSpawnpoint;
    
    public GameObject attackHitBox;



    private PlayerController playerController;

    private Animator scythAnimator;

    public GameObject player;

    private PlayerController plController;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plController = player.GetComponent<PlayerController>();
        attackHitBox.SetActive(false);
    }
    


    public void ScythAppear(){
        var newScyth = Instantiate(Scyth, scythSpawnpoint.position, player.transform.rotation);
        scythAnimator = newScyth.GetComponentInChildren<Animator>();
        scythAnimator.SetBool("appear", true);
    }

    public void Scythdisappear(){
        var newScyth = Instantiate(Scyth, scythSpawnpoint.position, player.transform.rotation);
        scythAnimator = newScyth.GetComponentInChildren<Animator>();
        scythAnimator.SetBool("appear", false);
    }


    public void attack(){
        attackHitBox.SetActive(true);
        StopCoroutine(deactivateAttackBox());
        StartCoroutine(deactivateAttackBox());
    }

    public IEnumerator deactivateAttackBox(){
        yield return new WaitForFixedUpdate();
        attackHitBox.SetActive(false);
    }

    public void AttackFinish(){
        plController.canControl = true;
    }

}
