using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{

    [SerializeField]
    private GameObject Scyth;
    [SerializeField]
    private Transform scythSpawnpoint;
    [SerializeField]
    private GameObject attackHitBox;


    private PlayerController playerController;

    private Animator scythAnimator;

    public GameObject player;


    private PlayerController plController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plController = player.GetComponent<PlayerController>();
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

    public IEnumerator attack(){
        attackHitBox.SetActive(true);
        yield return new WaitForFixedUpdate();
        attackHitBox.SetActive(false);
        GameController.instance.PlayClip("big_slam");
        
    }

    public void smallAttack() {
        GameController.instance.PlayClip("sword_impact");
    }

    public void jumping() {
        GameController.instance.PlayClip("jump");
    }

    public void AttackFinish(){
        plController.canControl = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 5;
    }

}
