using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed;
    private float oldSpeed;
    public float damage;
    public float maxHealth;
    private float currentHealth;

    public float checkDistance;

    public LayerMask whatIsGround;

    public AudioSource damageSound;
    public AudioSource deathSound;

    public GameObject deathParticle;


    private SpriteRenderer sprite;

    private Material matWhite;
    private Material matDefault;


    private bool canGetDamage = true;
    private bool isGrounded;

    private Rigidbody2D rb;

    private Transform attackerPos;
    public Transform feetPos;

    private Vector2 startPos;

    public float range = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
        startPos = transform.position;
        GameController.instance.AddResetListener(Reset);
        oldSpeed = speed;

    }

    private void Reset()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        transform.position = startPos;
        canGetDamage = true;
        speed = oldSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitBox") && canGetDamage)
        {
            StartCoroutine(EnemyDamage());
        }
    }

    private IEnumerator EnemyDamage()
    {
        currentHealth--;
        canGetDamage = false;
        if (damageSound != null)
            damageSound.Play();
        yield return new WaitForFixedUpdate();

        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        speed = 0;

        if (currentHealth <= 0)
        {
            if (deathSound != null)
                deathSound.Play();

            gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(0.5f);
        speed = oldSpeed;
        canGetDamage = true;

    }

    private void FixedUpdate()
    {
        if (this.gameObject.GetComponent<ScytheControlledObject>() == null)
        {
            Vector3 playerPos = GameController.Player.transform.position;
            Vector3 direction = playerPos - transform.position;


            if (Vector2.Distance(transform.position, playerPos) < range)
                rb.MovePosition(transform.position + (direction.normalized * speed * Time.deltaTime));

            
            if(playerPos.x < transform.position.x)
                transform.eulerAngles = new Vector3(0.0f, 0f, 0.0f);
            else
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

                
                
        }

    }

}
