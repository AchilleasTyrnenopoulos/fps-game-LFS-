using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.WSA.Input;

public class EnemyControllerZombie : MonoBehaviour
{
    #region Variables
    public Transform goal;
    public float moveSpeed;
    public float rotSpeed;

    public Animator anim;
    public NavMeshAgent agent;

    public float distanceToChase;//change to private
    public float distanceToChaseWhenCrouching;
    public float distanceToAttack = 3f;//change to private

    float accuracy = 1.0f;
    public bool chasing = false;//change to private later
    public bool attacking = false;//change to private later
    #endregion

    void LateUpdate()
    {            
        if (!chasing)
        {
            if (!PlayerController.instance.crouching)
            {
                if (Vector3.Distance(this.transform.position, goal.transform.position) < distanceToChase && !attacking)
                    chasing = true;//start chasing
            }
            else
            {
                if (Vector3.Distance(this.transform.position, goal.transform.position) < distanceToChaseWhenCrouching && !attacking)
                    chasing = true;
            }
            //if (Vector3.Distance(this.transform.position, goal.transform.position) <= distanceToAttack)
            //{
            //    chasing = false;
            //    attacking = true;
            //}
        }
        else
        {            
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, lookAtGoal) > accuracy)
            {
                this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(this.transform.position, goal.transform.position) <= distanceToAttack && PlayerController.instance.gameObject.activeInHierarchy)
            {
                chasing = false;
                //maybe add counter here and an if statement, if attackCounter <= 0, then attack
                attacking = true;
            }
        }

        if (attacking)
        {
            chasing = false;
            //agent.destination = this.transform.position;
            if (Vector3.Distance(this.transform.position, goal.transform.position) > distanceToAttack || !PlayerController.instance.gameObject.activeInHierarchy)
            {
                attacking = false;
                chasing = true;
            }
        }

        anim.SetBool("isMoving", chasing);
        anim.SetBool("Attacking", attacking);//attacking animation bugs for some reason, maybe create counter for attacking equal to the animations duration or trigger somethign else for the attacking animations so it won't send it every update.
    }

    public void ChangeDistanceWhenCrouch()
    {
        distanceToChase *= 2;
    }
}
