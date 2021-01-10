using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeaponsController : MonoBehaviour
{
    #region Variables

    public static ThrowingWeaponsController instance;

    public int rangeDamage;
    public float moveSpeed, lifeTime;
    public Rigidbody theRB;
    public float gravityModifier;

    public bool damageEnemy;
    public bool damagePlayer;
    //public float nonGravityTimer;
    //public float nonGravityCounter = 0.4f;
    public Vector3 move;//change to private
    //make variable here or on playerController throwStrength which will grow when holding down leftMouse until maxThrowStrength

    #endregion


    // Start is called before the first frame update
    void Start()
    {
       // nonGravityTimer = nonGravityCounter;
        //set the move speed of the weapon based on the strength of the character
        //moveSpeed = PlayerController.instance.strength;
    }

    // Update is called once per frame
    void Update()
    {
        float yStore = move.y;
        //set the move speed of the weapon based on the strength of the character
        //moveSpeed = PlayerController.instance.strength;

        //move throwing weapon
        //moveSpeed = PlayerController.instance.strength;//doesnt work for some reason
        theRB.velocity = transform.forward * moveSpeed * PlayerController.instance.strength/10 ;//add strength variable, maybe replace movespeed with strength and weight of weapon|||try .right instead of .forward for approriate dagger direction
        //if (nonGravityTimer <= 0)
        //{
            move.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
            theRB.AddForce(move);
        //}    

        //countdown for lifetime
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(rangeDamage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "HeadShot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(rangeDamage * 2);
        }
        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            //other.gameObject.GetComponent<PlayerHealthController>().DamagePlayer();
            Debug.Log("Player took damage");
            Destroy(gameObject);//needs inside if otherwise gets destroyed when shooting from the enemy, interacts with enemy and gets destroyed before actually shooting it
        }

        Destroy(gameObject);   
    }
}
