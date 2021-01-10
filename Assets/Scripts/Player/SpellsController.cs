using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsController : MonoBehaviour
{
    #region Variables

    public static SpellsController instance;

    [Header("Healing Spell")]    
    public float healAmount;

    public bool healed = false;

    private float healCdRate;
    private float healCdCounter;

    private int healManaCost = 10;

    

    [Header("Invulnerability Spell")]    
    public float invulDuration;

    public bool invuled = false;

    public float invulCdRate;
    private float invulCdCounter;

    private int invulManaCost = 25;

    [Header("Anti-Gravity Spell")]    
    public float gravReduction;

    public bool graved = false;
    public bool floating = false;//set to private    

    public float gravCdRate;
    private float gravCdCounter;

    private float gravDuration;
    private float gravCounter;

    private int gravManaCost = 25;

    [Header("Stamina Restore Spell")]
    public float stamRestAmount;

    public bool stamRestored = false;

    private float stamRestCdRate;
    private float stamRestCdCounter;

    private int stamRestHealthCost = 20;

    [Header("Slowdown Time Spell")]
    public float timeReduction;

    public bool timed = false;
    public bool slowed = false;

    private float slowCdRate;
    private float slowCdCounter;

    private float slowDuration;
    private float slowCounter;
        

    private int slowTimeManaCost = 50;

    #endregion

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set spells 'power'
        healAmount = PlayerController.instance.intelligence * 4;
        invulDuration = PlayerController.instance.intelligence * 2;
        gravReduction = PlayerController.instance.intelligence / 2.5f;
        stamRestAmount = PlayerController.instance.intelligence <= 10 ? PlayerController.instance.intelligence * 2 : PlayerController.instance.intelligence * 1.5f;
        //if (PlayerController.instance.intelligence <= 10)
        //{
        //    stamRestAmount = PlayerController.instance.intelligence * 2;
        //}
        //else if (PlayerController.instance.intelligence > 11 && PlayerController.instance.intelligence <=15)
        //{
        //    stamRestAmount = PlayerController.instance.intelligence * 1.5f; 
        //}
        //else
        //{
        //    stamRestAmount = PlayerController.instance.intelligence;
        //}
        timeReduction = PlayerController.instance.intelligence <= 9 ? PlayerController.instance.intelligence * 0.1f : 0.9f + PlayerController.instance.intelligence * 0.01f;

        //set cooldowns
        healCdRate = 300 / PlayerController.instance.willpower;
        healCdCounter = healCdRate;
        invulCdRate = 300 / PlayerController.instance.willpower;
        invulCdCounter = invulCdRate;
        gravCdRate = 300 / PlayerController.instance.willpower;
        gravCdCounter = gravCdRate;
        gravDuration = 3 * PlayerController.instance.willpower;
        gravCounter = gravDuration;
        stamRestCdRate = 15 / PlayerController.instance.willpower;
        stamRestCdCounter = stamRestCdRate;
        slowCdRate = 300 / PlayerController.instance.willpower;
        slowCdCounter = slowCdRate;
        slowDuration = 3 * PlayerController.instance.willpower;
        slowCounter = slowDuration;

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ////set cooldowns without causing counter countdown bug      

        //set spells 'power'
        healAmount = PlayerController.instance.intelligence * 4;
        invulDuration = PlayerController.instance.intelligence * 2;
        gravReduction = PlayerController.instance.intelligence / 2.5f;
        if (PlayerController.instance.intelligence <= 10)
        {
            stamRestAmount = PlayerController.instance.intelligence * 2;
        }
        else if (PlayerController.instance.intelligence > 11 && PlayerController.instance.intelligence <= 15)
        {
            stamRestAmount = PlayerController.instance.intelligence * 1.5f;
        }
        else
        {
            stamRestAmount = PlayerController.instance.intelligence;
        }
        timeReduction = PlayerController.instance.intelligence <= 9 ? PlayerController.instance.intelligence * 0.1f : 0.9f + PlayerController.instance.intelligence * 0.01f;

        //healing
        if (Input.GetKeyDown(KeyCode.C) 
            && PlayerHealthController.instance.currentMana >= healManaCost && healCdCounter == healCdRate 
            && PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth
            )
        {
            healed = true;
            PlayerHealthController.instance.SpendMana(healManaCost);
            PlayerHealthController.instance.currentHealth += Convert.ToInt32(healAmount);

            //Update UI
            UIController.instance.healthSlider.value = PlayerHealthController.instance.currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + PlayerHealthController.instance.currentHealth + "/" + PlayerHealthController.instance.maxHealth;
            UIController.instance.imageHealCooldown.fillAmount = 1;
        }

        //invulnerability
        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerHealthController.instance.currentMana >= invulManaCost && invulCdCounter == invulCdRate)
        {
            invuled = true;
            PlayerHealthController.instance.SpendMana(invulManaCost);
            PlayerHealthController.instance.invincibleLength = invulDuration;
            PlayerHealthController.instance.invincibleCounter = PlayerHealthController.instance.invincibleLength;

            UIController.instance.imageSp1Cooldown.fillAmount = 1;
            UIController.instance.imageSp1Duration.fillAmount = 1;
        }

        //anti-gravity
        if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerHealthController.instance.currentMana >= gravManaCost && gravCdCounter == gravCdRate)
        {
            floating = true;
            graved = true;
            PlayerHealthController.instance.SpendMana(gravManaCost);
            PlayerController.instance.gravityModifier = 2 / gravReduction;

            UIController.instance.imageSp2Cooldown.fillAmount = 1;
            UIController.instance.imageSp2Duration.fillAmount = 1;
        }

        //stamina restore spell
        if (Input.GetKeyDown(KeyCode.X) && PlayerHealthController.instance.currentHealth >= stamRestHealthCost && stamRestCdCounter == stamRestCdRate
            && PlayerHealthController.instance.currentStamina < PlayerHealthController.instance.maxStamina
            )
        {
            stamRestored = true;
            PlayerHealthController.instance.DamagePlayer(stamRestHealthCost);
            PlayerHealthController.instance.currentStamina += Convert.ToInt32(stamRestAmount);

            //Update UI
            UIController.instance.healthSlider.value = PlayerHealthController.instance.currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + PlayerHealthController.instance.currentHealth + "/" + PlayerHealthController.instance.maxHealth;            
            UIController.instance.staminaSlider.value = PlayerHealthController.instance.currentStamina;
            UIController.instance.staminaText.text = "STAMINA: " + PlayerHealthController.instance.currentStamina + "/" + PlayerHealthController.instance.maxStamina;
            UIController.instance.imageStamRestCooldown.fillAmount = 1;
        }

        //slow time spell
        if (Input.GetKeyDown(KeyCode.Alpha4) && PlayerHealthController.instance.currentMana >= slowTimeManaCost && slowCdRate == slowCdCounter)
        {
            timed = true;
            slowed = true;

            PlayerHealthController.instance.SpendMana(slowTimeManaCost);
            Time.timeScale -= timeReduction;
            PlayerController.instance.agility *= PlayerController.instance.intelligence <= 9 ? timeReduction * 2.5f : timeReduction * 50;
            PlayerController.instance.jumpPower = ((PlayerController.instance.agility / 2) + (PlayerController.instance.strength / 2));

            //update ui
            UIController.instance.imageSp3Cooldown.fillAmount = 1;
            UIController.instance.imageSp3Duration.fillAmount = 1;
        }

        //handle healing cooldown
        if (healed)
        {
            healCdCounter -= Time.deltaTime;
            
            if (healCdCounter <= 0)
            {
                healed = false;
                healCdCounter = healCdRate;
            }

            //ui
            UIController.instance.imageHealCooldown.fillAmount -= 1 / healCdRate * Time.deltaTime;
        }

        //handle invulnerability cooldown
        if (invuled)
        {
            invulCdCounter -= Time.deltaTime;
             
            if (invulCdCounter <= 0)
            {
                invuled = false;
                invulCdCounter = invulCdRate;
                PlayerHealthController.instance.invincibleLength = 1f;
            }

            //ui           
            UIController.instance.imageSp1Cooldown.fillAmount -= 1 / invulCdRate * Time.deltaTime;
            
            
        }

        //handle anti-gravity cooldown
        if (graved)
        {
            gravCdCounter -= Time.deltaTime;

            if (gravCdCounter <= 0)
            {
                graved = false;
                gravCdCounter = gravCdRate;                
            }

            //ui
            UIController.instance.imageSp2Cooldown.fillAmount -= 1 / gravCdRate * Time.deltaTime;
        }

        //handle anti-gravity spell duration
        if (floating)
        {
            gravCounter -= Time.deltaTime;
            UIController.instance.imageSp2Duration.fillAmount -= 1 / gravDuration * Time.deltaTime;

            if (gravCounter <= 0)
            {
                floating = false;
                PlayerController.instance.gravityModifier = 2;
                gravCounter = gravDuration;
            }
        }

        //handle stamina restore spell cooldown
        if (stamRestored)
        {
            stamRestCdCounter -= Time.deltaTime;

            if (stamRestCdCounter <= 0)
            {
                stamRestored = false;
                stamRestCdCounter = stamRestCdRate;
            }

            //ui
            UIController.instance.imageStamRestCooldown.fillAmount -= 1 / stamRestCdRate * Time.deltaTime;
        }

        //handle slowtime cooldown
        if (timed)
        {
            slowCdCounter -= Time.deltaTime;
            UIController.instance.imageSp3Cooldown.fillAmount -= 1 / slowCdRate * Time.deltaTime;

            if (slowCdCounter <= 0)
            {
                timed = false;
                slowCdCounter = slowCdRate;
            }            
        }

        //handle slowtime spell duration
        if (slowed)
        {
            slowCounter -= Time.deltaTime;
            UIController.instance.imageSp3Duration.fillAmount -= 1 / slowDuration * Time.deltaTime;

            if (slowCounter <= 0)
            {
                slowed = false;
                Time.timeScale = 1;
                PlayerController.instance.agility /= PlayerController.instance.intelligence <= 9 ? timeReduction * 2.5f : timeReduction * 50;
                PlayerController.instance.jumpPower = ((PlayerController.instance.agility / 2) + (PlayerController.instance.strength / 2)) * 1.5f;
                slowCounter = slowDuration;
            }
        }
        
    }
}
