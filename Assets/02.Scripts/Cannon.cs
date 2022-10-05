using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject expEffect;
    public float speed = 3000.0f;

    public int actorNumber;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject obj = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 4.0f);

        Destroy(this.gameObject);
    }
}
