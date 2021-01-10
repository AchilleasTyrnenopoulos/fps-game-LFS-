using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public Animator anim;

    private int baseDamage;

    public int atk1dmg;
    public int atk2dmg;
    public int atk3dmg;
    public int atk4dmg;

    public int atk1StamCost;
    public int atk2StamCost;
    public int atk3StamCost;
    public int atk4StamCost;

    public /*static*/ Collider meleeCollider;//make second collider for offhand

    //public bool attack1;
    //public bool attack2;    
    private void Start()
    {
        //set baseDamage
        baseDamage = Convert.ToInt32(PlayerController.instance.strength / 2);

        meleeCollider = GetComponent<Collider>();      
    }
    
    private void Update()
    {
        //reset baseDamage
        baseDamage = Convert.ToInt32(PlayerController.instance.strength / 2);

        if (PlayerController.instance.enabledRangedSystem == false)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                if (Input.GetMouseButtonDown(1) && PlayerHealthController.instance.currentStamina >= 2)
                {                    
                    //PlayerHealthController.instance.notMoving = false;
                    PlayerHealthController.instance.SpendStamina(atk2StamCost);                                        

                    PlayerController.instance.attack2 = true;

                    PlayerHealthController.instance.notMoving = false;
                    meleeCollider.isTrigger = true;//make it a function to use as an animation event
                    anim.SetBool("attacking2", true);
                }
                if (Input.GetMouseButtonDown(0) && PlayerHealthController.instance.currentStamina >= 2)
                {

                    //PlayerHealthController.instance.notMoving = false;
                    //meleeCollider.isTrigger = true;//make it a function to use as an animation event
                    PlayerHealthController.instance.SpendStamina(atk3StamCost);

                    PlayerController.instance.attack3 = true;

                    PlayerHealthController.instance.notMoving = false;
                    meleeCollider.isTrigger = true;//make it a function to use as an animation event
                    anim.SetBool("attacking3", true);
                }

            }
            else
            {
                if (Input.GetMouseButtonDown(1) && PlayerHealthController.instance.currentStamina >= 2)
                {
                    PlayerHealthController.instance.SpendStamina(atk1StamCost);

                    PlayerController.instance.attack1 = true;

                    PlayerHealthController.instance.notMoving = false;
                    //PlayerHealthController.instance.SpendStamina(15);
                    meleeCollider.isTrigger = true;//make it a function to use as an animation event
                    anim.SetBool("attacking", true);
                }
           
                if (Input.GetMouseButtonDown(0) && PlayerHealthController.instance.currentStamina >= 2)
                {
                    PlayerHealthController.instance.SpendStamina(atk4StamCost);

                    PlayerController.instance.attack4 = true;

                    PlayerHealthController.instance.notMoving = false;
                    meleeCollider.isTrigger = true;//make it a function to use as an animation event
                    anim.SetBool("attacking4", true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (PlayerController.instance.attack1)
            {
                other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(atk1dmg + baseDamage);
            }
            else if(PlayerController.instance.attack2)
            {
                other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(atk2dmg + baseDamage);
            }
            else if (PlayerController.instance.attack3)
            {
                other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(atk3dmg + baseDamage);
            }
            else if (PlayerController.instance.attack4)
            {
                other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(atk4dmg + baseDamage);
            }
        }

        if (other.gameObject.tag == "HeadShot")
        {
            other.GetComponentInParent<EnemyHealthController>().DamageEnemy(other.GetComponentInParent<EnemyHealthController>().maxHealth);//dmg based on characters attributes (and weapon maybe)
        }

        if (other.gameObject.tag == "BodyShot" )
        {
            other.GetComponentInParent<EnemyHealthController>().DamageEnemy(3);//dmg based on characters attributes (and weapon maybe)
        }

        if (other.gameObject.tag == "ArmShot")
        {
            other.GetComponentInParent<EnemyHealthController>().DamageEnemy(1);//dmg based on characters attributes (and weapon maybe)
        }

        if (other.gameObject.tag == "Footshot")
        {
            other.GetComponentInParent<EnemyHealthController>().DamageEnemy(2);//dmg based on characters attributes (and weapon maybe)
        }


    }
   
}
