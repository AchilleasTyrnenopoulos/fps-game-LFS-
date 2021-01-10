using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeToFall;

    Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //trigger shake animation
            Invoke("Fall", timeToFall);
        }
    }

    void Fall()
    {
        rb.useGravity = true;
    }
}
