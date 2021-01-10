using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;

    CharacterController cc;

    [Header("POSITION")]
    public bool shouldMove;
    [Space(10)]
    public bool forthX, forthY, forthZ;
    public bool backX = false, backY = false, backZ = false;
    public float x_maxDistanceA, x_maxDistanceB, y_maxDistanceA, y_maxDistanceB, z_maxDistanceA, z_maxDistanceB;//create for other directions
    public float moveSpeedX, moveSpeedY, moveSpeedZ;

    [Header("ROTATION")]
    public bool shouldRotate;
    [Space(10)]
    public float rotateSpeedX, rotateSpeedY, rotateSpeedZ;
    #endregion 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            //handle movement position on x axis
            if (forthX)
            {
                transform.position += new Vector3(moveSpeedX, 0f, 0f) * Time.deltaTime;
                if (transform.position.x >= x_maxDistanceA)
                {
                    forthX = false;
                    backX = true;
                }
            }
            if (backX)
            {
                transform.position -= new Vector3(moveSpeedX, 0f, 0f) * Time.deltaTime;
                if (transform.position.x >= x_maxDistanceB)
                {
                    forthX = false;
                    backX = true;
                }
            }
            //handle movement position on y axis
            if (forthY)
            {
                transform.position += new Vector3(0f, moveSpeedY, 0f) * Time.deltaTime;
                if (transform.position.y >= y_maxDistanceA)
                {
                    forthY = false;
                    backY = true;
                }

            }
            if (backY)
            {
                transform.position -= new Vector3(0f, moveSpeedY, 0f) * Time.deltaTime;
                if (transform.position.y <= y_maxDistanceB)
                {
                    forthY = true;
                    backY = false;
                }
            }
            //handle movement position on z axis
            if (forthZ)
            {
                transform.position += new Vector3(0f, 0f, moveSpeedZ) * Time.deltaTime;
                if (transform.position.z >= z_maxDistanceA)
                {
                    forthZ = false;
                    backZ = true;
                }
                
            }     
            if (backZ)
            {
                transform.position -= new Vector3(0f, 0f, moveSpeedZ) * Time.deltaTime;
                if (transform.position.z <= z_maxDistanceB)
                {
                    forthZ = true;
                    backZ = false;
                }
            }
        }

        if (shouldRotate)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, rotateSpeedY * Time.deltaTime, 0f));
        }
    }
    
}
