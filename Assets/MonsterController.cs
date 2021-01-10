using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    #region Variables
    Animator anim;
    GameObject player;
    NavMeshAgent agent;
    public GameObject ragdoll;

    public bool isIdle;

    //idle & wander
    public float idleTimer;
    public float idleCounter = 0;
    public float wanderTimer;
    public float wanderCounter = 0;

    //line of sight
    public bool seesPlayer;
    public float senseDistance = 20.0f;
    public float visDistance = 50f;
    public float visAngle = 90.0f;
    public float huntingTimer = 5f;
    public float huntingCounter = 0f;

    //attacking
    public bool attacking;


    public enum STATE { IDLE, WANDER, CHASE, ATTACK}
    STATE state;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponentInChildren<Animator>();
        state = STATE.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        //spawn ragdoll
        if (this.GetComponent<EnemyHealthController>().isDead)
        {
            GameObject rd = Instantiate(ragdoll, this.transform.position, this.transform.rotation);
            rd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * Random.Range(1000, 2000));
            
            Destroy(this.gameObject);
            return;
        }

        //calculate direction from npc to player
        Vector3 direction = player.transform.position - this.transform.position;

        //calculate the angle from the direction (of npc to player) and the 'facing forwrd' of npc
        float angle = Vector3.Angle(direction, this.transform.forward);

        //check if player is visible
        if (direction.magnitude < senseDistance && angle < visAngle
            )
        {
            seesPlayer = true;
        }
        //else
        //{
        //    seesPlayer = false;
        //}

        RaycastHit hit;
        /*else*/
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1.6f, 0), this.transform.forward, out hit, visDistance))//make 50f a variable
        {
            if (hit.collider.tag == "Player")
            {
                //Debug.Log("sees player");
                seesPlayer = true;
            }
        }

        Debug.DrawRay(this.transform.position + new Vector3(0, 1.6f, 0), this.transform.forward * visDistance, Color.red);

        if (seesPlayer)
        {
            huntingCounter += Time.deltaTime;
            if (huntingTimer <= huntingCounter)
            {
                seesPlayer = false;
                huntingCounter = 0f;
            }
        }
        switch (state)
        {
            case STATE.IDLE:
                if (!isIdle)
                {
                    idleCounter += Time.deltaTime;

                    if (idleCounter >= idleTimer)
                    {
                        idleCounter = 0f;
                        anim.SetTrigger("moving");
                        state = STATE.WANDER;
                    }
                }

                //test if npc sees player
                if (seesPlayer)
                {
                    //set direction y to zero (so npc wont tilt)
                    direction.y = 0;

                    ////turn npc to player                    
                    anim.SetTrigger("moving");
                    state = STATE.CHASE;
                }                
                else
                {
                    return;
                }
                break;
                
            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    float rand = Random.Range(0f, 1f);
                    if (rand > 0.5f)
                    {
                        anim.SetTrigger("idle");
                        state = STATE.IDLE;

                        return;
                    }

                    float newX = this.transform.position.x + Random.Range(-5, 5);
                    float newZ = this.transform.position.z + Random.Range(-5, 5);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));

                    Vector3 destination = new Vector3(newX, newY, newZ);
                    agent.stoppingDistance = 0;
                    agent.SetDestination(destination);
                }                

                if (seesPlayer)
                {                
                    //anim.SetTrigger("moving");
                    state = STATE.CHASE;
                }
                break;

            case STATE.CHASE:                
                agent.stoppingDistance = 1.5f;
                agent.SetDestination(player.transform.position);

                //switch states
                //check if enemy is in attacking distance
                if (Vector3.Distance(agent.transform.position, player.transform.position) <= agent.stoppingDistance && !agent.pathPending)
                {
                    //add a timer in the check to see if player is in range for more than x secs
                    float rand = Random.Range(0f, 1f);
                    anim.SetFloat("rand", rand);
                    anim.SetTrigger("attacking");
                    state = STATE.ATTACK;
                }
                else if (!seesPlayer)
                {                    
                    anim.SetTrigger("moving");
                    agent.stoppingDistance = 0f;
                    agent.ResetPath();
                    state = STATE.WANDER;
                }
                break;
            case STATE.ATTACK:
                //rotate npc to face player
                this.transform.LookAt(player.transform.position);


                if (!attacking)
                {
                    //add crouch attack

                    //dont move
                    //agent.isStopped = true;
                    agent.SetDestination(agent.transform.position);



                    if (Vector3.Distance(agent.transform.position, player.transform.position) > agent.stoppingDistance)
                    {
                        anim.SetTrigger("moving");
                        state = STATE.CHASE;
                    }
                }
                break;
            default:
                break;
        }
    }
}
