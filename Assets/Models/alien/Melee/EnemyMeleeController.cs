using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMeleeController : MonoBehaviour
{
    #region Variables
    NavMeshAgent agent;
    GameObject player;
    Animator anim;
    WPManager wpScript;

    public GameObject ragdoll;

    //patrolling
    public bool isPatrol = false;
    public float patrolWaitTime;
    public int patrolArrCounter = 0;

    Vector3 startPos;    
    public float walkspeed = 2f;
    public float runSpeed = 8.0f;

    //line of sight
    public bool seesPlayer;
    public float senseDistance = 20.0f;
    public float visDistance = 50f;
    public float visAngle = 90.0f;
    public float huntingTimer = 5f;
    public float huntingCounter = 0f;

    //awareness
    public float awareRange = 4.0f;
    public bool aware = false;
    public float awarenessTimer = 10f;
    public float awarenessCounter = 0f;

    //attacking
    float attackingRange;//in check to attack replace agent.stopping distance with this    
    public bool attacking = false;

    public enum STATE { IDLE, AWARE, CHASE, ATTACK, GOTOSTART, HIDE, REGEN}
    STATE state;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponentInChildren<Animator>();
        wpScript = this.GetComponentInChildren<WPManager>();

        agent.speed = walkspeed;
        state = STATE.IDLE;
        startPos = this.transform.position;
        attackingRange = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //spawn ragdoll
        if (this.GetComponent<EnemyHealthController>().isDead)
        {
            GameObject rd = Instantiate(ragdoll, this.transform.position, this.transform.rotation);
            rd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1000);
            Destroy(this.gameObject);
            return;
        }

        if (state != STATE.CHASE)
            agent.speed = walkspeed;

        //calculate direction from npc to player
        Vector3 direction = player.transform.position - this.transform.position;

        //calculate the angle from the direction (of npc to player) and the 'facing forwrd' of npc
        float angle = Vector3.Angle(direction, this.transform.forward);

        //check if player is visible
        if (direction.magnitude < senseDistance && angle < visAngle
            || aware && Vector3.Distance(agent.transform.position, player.transform.position) < 2f) 
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

        //check if npc 'senses the player'
        if (Vector3.Distance(agent.transform.position, player.transform.position) < awareRange && PlayerController.instance.crouching == false 
            || seesPlayer == true 
            || state == STATE.AWARE)
        {
            aware = true;
        }
        else
        {
            aware = false;
        }           

        #region States
        switch (state)
        {
            case STATE.IDLE:                
                //test if npc sees player
                if (seesPlayer)
                {
                    //set direction y to zero (so npc wont tilt)
                    direction.y = 0;

                    ////turn npc to player                    
                    anim.SetTrigger("running");
                    state = STATE.CHASE;                    
                }
                else if(aware)
                {
                    //wander
                    anim.SetTrigger("walking");
                    state = STATE.AWARE;
                }
                else if (isPatrol)
                {
                    GoToWaypoint(patrolArrCounter);                                                                  
                }
                else
                {
                    return;
                }
                break;

            case STATE.CHASE:
                agent.speed = runSpeed;
                agent.stoppingDistance = 3.5f;
                agent.SetDestination(player.transform.position);

                //switch states
                //check if enemy is in attacking distance
                if(Vector3.Distance(agent.transform.position, player.transform.position) <= agent.stoppingDistance && !agent.pathPending)
                {
                    //add a timer in the check to see if player is in range for more than x secs
                    float rand = Random.Range(0f, 1f);
                    anim.SetFloat("rand", rand);
                    anim.SetTrigger("attacking");
                    state = STATE.ATTACK;
                }    
                else if(!seesPlayer)
                {
                    aware = true;
                    anim.SetTrigger("walking");
                    agent.stoppingDistance = 0f;
                    agent.ResetPath();
                    state = STATE.AWARE;
                }
                break;

            case STATE.AWARE:
                awarenessCounter += Time.deltaTime;
                if (awarenessCounter >= awarenessTimer)
                {
                    awarenessCounter = 0;
                    state = STATE.GOTOSTART;
                }

                if (!agent.hasPath)
                {
                    float newX = this.transform.position.x + Random.Range(-5, 5);
                    float newZ = this.transform.position.z + Random.Range(-5, 5);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));

                    Vector3 destination = new Vector3(newX, newY, newZ);
                    agent.stoppingDistance = 0;
                    agent.SetDestination(destination);                    
                }

                if(seesPlayer)
                {
                    awarenessCounter = 0f;
                    anim.SetTrigger("running");
                    state = STATE.CHASE;
                }                
                break;

            case STATE.GOTOSTART:
                agent.SetDestination(startPos);
                if (this.transform.position == startPos)
                {
                    anim.SetTrigger("idle");
                    state = STATE.IDLE;
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
                        anim.SetTrigger("running");
                        state = STATE.CHASE;
                    }
                }
                break;

            case STATE.HIDE:
                break;
            case STATE.REGEN:
                break;
            default:
                break;
        }
        #endregion
    }

    #region Functions
    void GoToWaypoint(int i)
    {
        //trigger walking animation
        if (!agent.hasPath && !agent.isStopped)
            anim.SetTrigger("walking");

        //set waypoint destination position
        agent.SetDestination(wpScript.waypoints[i].transform.position);

        //stop npc
        if (Vector3.Distance(agent.transform.position, wpScript.waypoints[i].transform.position) <= agent.stoppingDistance + 1)
        {
            //stop npc
            if(!agent.isStopped)
                anim.SetTrigger("idle");
            
            agent.isStopped = true;

            //change waypoint
            if (i < wpScript.waypoints.Length - 1)
            {
                patrolArrCounter++;
                Invoke("ResumeAgent", patrolWaitTime);
            }
            else
            {
                patrolArrCounter = 0;
                Invoke("ResumeAgent", patrolWaitTime);
            }
        }
    }

    void ResumeAgent()
    {
        agent.isStopped = false;
        anim.SetTrigger("walking");
    }

    #endregion
}


