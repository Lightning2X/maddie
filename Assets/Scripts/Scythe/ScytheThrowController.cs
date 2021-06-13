using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScytheThrowController : MonoBehaviour
{
    public GameObject scytheThrowObject;

    public int scytheLimit = 1;
    private List<GameObject> thrownScythes;

    private Rigidbody2D rigidBody;

    public float throwForce = 750f;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        thrownScythes = new List<GameObject>();
        GameController.instance.AddResetListener(Reset);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && canThrow())
        {
            updateScytheList();
            GameObject newScyte = Instantiate(scytheThrowObject, rigidBody.position, Quaternion.Euler(0f, 0f, rigidBody.rotation));
            Rigidbody2D scytheRigidBody = newScyte.GetComponent<Rigidbody2D>();
            scytheRigidBody.AddForce(getNormalizedMouseVector() * throwForce);
            thrownScythes.Add(newScyte);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void Reset()
    {
        thrownScythes.ForEach(x => Destroy(x));
        thrownScythes = new List<GameObject>();
    }

    private bool canThrow()
    {
        if (thrownScythes.Count == 0)
            return true;

        return thrownScythes[thrownScythes.Count - 1].GetComponent<ThrownScytheController>().Collided;
    }

    private void updateScytheList()
    {
        thrownScythes.Where(item => item.activeSelf).ToList();
        if (thrownScythes.Count >= scytheLimit)
        {
            GameObject scytheToDelete = thrownScythes[0];
            thrownScythes.Remove(scytheToDelete);
            Destroy(scytheToDelete);
        }
    }

    private Vector2 getNormalizedMouseVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = new Vector2(mousePos.x, mousePos.y) - rigidBody.position;
        return mouseDir.normalized;
    }
}
