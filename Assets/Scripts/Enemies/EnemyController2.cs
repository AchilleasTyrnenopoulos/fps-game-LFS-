using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController2 : MonoBehaviour
{
    public float moveSpeed;
  
    [Header("CHASING")]
    private bool chasing;
    public float distanceToChase = 10f;
    public float distanceToLose = 15f;
    public float distanceToStop = 2f;
    public float keepChasingTime = 5F;
    public float chaseCounter;

    private Vector3 targetPoint, startPoint;// the position of the player, the start position of the enemy

    public NavMeshAgent agent;

    [Header("SHOOTING")]
    public GameObject throwingWeapon;
    public Transform firepoint;
    public float fireRate, waitBetweenShots = 1.5f, timeToShoot = 1f;
    public float fireCount;
    private float shotWaitCounter, shootTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        //targetPoint.y = transform.position.y;//that way the enemy wont look up or down only around side to side, but also want jump
        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                //fireCount = 1f;//wait before first shot
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
            }
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }          
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;//move the player towards the player
            }
            else
            {
                agent.destination = transform.position;//stop moving
            }
            //check if player still on range
            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;

                chaseCounter = keepChasingTime;
            }
            if(shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;
                if (shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }
            }
            else
            {
                shootTimeCounter -= Time.deltaTime;
                if (shootTimeCounter > 0)
                {
                    fireCount -= Time.deltaTime;
                    if (fireCount <= 0)
                    {
                        fireCount = fireRate;

                        //make enemy look at the player
                        firepoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.5f, 0f));//the new Vector is needed so the enemy wont shoot at the players feet

                        //check the angle to the player
                        Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                        float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);//so the enemy will eventually look up

                        if (Mathf.Abs(angle) <  30f)//if wants more accurate enemy, lower the 30f 
                        {
                            Instantiate(throwingWeapon, firepoint.position, firepoint.rotation);
                        }
                        else
                        {
                            shotWaitCounter = waitBetweenShots;
                        }
                        
                    }

                    agent.destination = transform.position;//enemy not moving just firisng its shot
                }
                else
                {
                    shotWaitCounter = waitBetweenShots;
                }
            }
           

           
        }
        
    }
}
