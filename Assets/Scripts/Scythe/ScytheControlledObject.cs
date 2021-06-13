using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScytheControlledObject : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rigidBody;
    public float initialMovementSpeed = 5f;

    public float playerGravCompensationVelocity = -0.1f;
    private int scythesAttached;
    public int ScythesAttached
    {
        get { return scythesAttached; }
        set
        {
            MovementSpeed = initialMovementSpeed * FallOff(value);
            scythesAttached = value;
        }
    }

    private float MovementSpeed { get; set; }

    private bool rigidBodyExisted = true;
    private void Start()
    {
        this.ScythesAttached = 1;
        this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        if(rigidBody == null) {
            this.rigidBody = this.gameObject.AddComponent<Rigidbody2D>();
            rigidBodyExisted = false;
        }
        this.rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.rigidBody.freezeRotation = true;
        this.rigidBody.bodyType = RigidbodyType2D.Dynamic;
        this.rigidBody.gravityScale = 0f;
        this.rigidBody.mass = 10f;
    }

    private void FixedUpdate()
    {
        float xInputHor = Input.GetAxisRaw("Horizontal");
        float xInputVert = Input.GetAxisRaw("Vertical");
        if (rigidBody != null)
        {
            this.rigidBody.velocity = new Vector2(MovementSpeed * xInputHor, MovementSpeed * xInputVert);
        }

        if(GameController.Player.transform.parent != null && GameController.Player.transform.parent != rigidBody.transform) { 
            this.rigidBody.velocity += new Vector2(0, playerGravCompensationVelocity);
        }
    }
    private float FallOff(int x)
    {
        float fallOffSum = 1;
        for (int i = 1; i <= x - 1; i++)
        {
            fallOffSum += 0.5f / i;
        }

        return fallOffSum;
    }

    private void OnDestroy()
    {
        ClearPlayerParent();
        if(!rigidBodyExisted) 
            Destroy(rigidBody);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3) {
            if(GameController.Player.transform.position.y > rigidBody.position.y) { 
                GameController.Player.transform.SetParent(rigidBody.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == 3)
        {
            ClearPlayerParent();
        }
    }

    private void ClearPlayerParent() {
        if (GameController.Player.transform.parent == rigidBody.transform)
        {
            GameController.Player.transform.SetParent(null);
        }
    }

}
