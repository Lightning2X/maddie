using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrownScytheController : MonoBehaviour
{
    public GameObject scytheBreakParticles;
    private Rigidbody2D rigidBody;
    private GameObject controlledObject = null;

    public bool Collided { get; private set; }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Collided = false;
    }

    private void FixedUpdate()
    {
        if (!Collided)
            this.rigidBody.rotation += 5.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Collided)
            return;

        Collided = true;
        if (StaticObjectProps.getProps(other.gameObject).immovable)
        {
            GameController.instance.PlayClip("scythe_break");
            GameObject particles =  GameController.Instantiate(scytheBreakParticles, this.transform.position, this.transform.rotation, null);
            this.gameObject.SetActive(false);
            Destroy(particles, 2.5f);
            return;
        }
        controlledObject = other.gameObject;
        LockRigidBody();
        ControlObject();
    }

    private void LockRigidBody()
    {
        rigidBody.rotation = 0f;
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    private void ControlObject()
    {
        ScytheControlledObject scytheControl = controlledObject.GetComponent<ScytheControlledObject>();
        if (scytheControl == null)
        {
            controlledObject.AddComponent<ScytheControlledObject>();
        }
        else
        {
            scytheControl.ScythesAttached += 1;
        }

        this.gameObject.transform.SetParent(controlledObject.transform);
    }

    private void OnDestroy()
    {
        if (controlledObject != null)
        {
            ScytheControlledObject scytheControl = controlledObject.GetComponent<ScytheControlledObject>();
            if (scytheControl.ScythesAttached <= 1)
            {
                Destroy(controlledObject.GetComponent<ScytheControlledObject>());
            }
            else
            {
                scytheControl.ScythesAttached -= 1;
            }

        }
    }
}
