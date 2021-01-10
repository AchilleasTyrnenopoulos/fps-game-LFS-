using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    //public Transform startPosition;
    //public Transform endPosition;
    public float travelTime;

    private Rigidbody rb;
    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = Vector3.Lerp(startPos, endPos, Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -2f + .1f);
        rb.MovePosition(currentPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Player")
            
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlayerController.instance.charCon.Move(rb.velocity * Time.deltaTime);
    }

}
