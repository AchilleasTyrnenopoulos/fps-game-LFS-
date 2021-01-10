using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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

    [Header("ATTACKING")]
    
    public Transform firepoint;
    public float fireRate, waitBetweenAttacks = 1.5f, timeToAttack = 1f;
    public float fireCount;
    public Animator anim;

    private float attackWaitCounter, attackTimeCounter;
   

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        attackTimeCounter = timeToAttack;
        attackWaitCounter = waitBetweenAttacks;

        
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
                attackTimeCounter = timeToAttack;
                attackWaitCounter = waitBetweenAttacks;
            }
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }
        }//if our enemy is not chasing us
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;//move the enemy towards the player
            }//if player is close enough to enemy to keep being chased
            else
            {
                agent.destination = transform.position;//stop moving
            }//if enemy loses the player
            //check if player still on range
            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;

                chaseCounter = keepChasingTime;
            }//if distance between player and enemy is big enough for the enemy to lose the player
            if (attackWaitCounter > 0)
            {
                attackWaitCounter -= Time.deltaTime;
                if (attackWaitCounter <= 0)
                {
                    attackTimeCounter = timeToAttack;
                }
            }
            else
            {
                attackTimeCounter -= Time.deltaTime;
                if (attackTimeCounter > 0)
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

                        if (Mathf.Abs(angle) < 30f)//if wants more accurate enemy, lower the 30f 
                        {

                            anim.SetTrigger("Attacking");
                        }
                        else
                        {                            
                            attackWaitCounter = waitBetweenAttacks;
                        }            
                    }
                    agent.destination = transform.position;//enemy not moving just attacking
                    anim.SetTrigger("Attacking");
                }
                else
                {
                    attackWaitCounter = waitBetweenAttacks;
                }
            }
            //anim.SetBool("attacking", attacking);
        }//if enemy is chasing us

        anim.SetBool("isMoving", chasing);
    }
}
