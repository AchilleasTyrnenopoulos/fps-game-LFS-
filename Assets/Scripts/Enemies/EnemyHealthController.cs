using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EnemyHealthController : MonoBehaviour
{
    public static EnemyHealthController instance;

    public bool hasRagdoll;
    public bool isDead;

    [Header("ENEMY STATS")]
    public int maxHealth = 5;
    public int currentHealth = 5;
    //public EnemyController theEC;
    
    [Header("LOOT")]
    public bool dropsLoot;
    public GameObject lootItem;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            isDead = true;
             
            //GET CURRENT ENEMY POSITION
            Vector3 currEnemyPos = new Vector3();
            currEnemyPos = transform.position;
            
            if (!hasRagdoll)            
                Destroy(gameObject);
            
            //DROP LOOT
            if (dropsLoot)
            {
                Instantiate(lootItem, currEnemyPos, Quaternion.Euler(0, 0, 0));
            }
            //AudioManager.instance.PlaySFX(2);         
        }
    }

    //handle melee
    //private void OnTriggerEnter(Collider other)
    //{
        //if (other.tag == "Melee")
        //{
        //    DamageEnemy(10); //or do dmg
        //    Debug.Log("collision");
        //} other.tag == "Player" ||
        //if ( other.tag == "Melee")
        //{
        //    DamageEnemy(10);
        //}
    //}

}
