using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    #region Variables
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth, maxStamina, currentStamina, maxMana, currentMana;
    private float decimalCurrentStamina ;
    //private int xStamina = 5;

    //Counters and Rates
    public float healthRegenCounter;
    public float staminaRegenCounter;
    public float manaRegenCounter;
    public float healthRegenRate;
    public float staminaRegenRate;
    public float manaRegenRate;

    private Vector3 lastPosition = new Vector3 (0f, 0f, 0f);
    public bool notMoving = true;

    public float invincibleLength = 1f;
    public float invincibleCounter = 0f;
    #endregion

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = gameObject.transform.position;
       
        //set regenaration rates
        healthRegenRate = 25 / AttributesManager.instance.globalEndurance;
        staminaRegenRate = 5 / AttributesManager.instance.globalWillpower;
        manaRegenRate = 25 / AttributesManager.instance.globalWillpower;

        //set health variables        
        maxHealth = Convert.ToInt32(AttributesManager.instance.globalStrength) * 20;
        currentHealth = maxHealth;
        //set stamina variables
        maxStamina = Convert.ToInt32(AttributesManager.instance.globalEndurance) * 20;
        currentStamina = maxStamina;
        //set mana variables
        maxMana = Convert.ToInt32(AttributesManager.instance.globalIntelligense) * 10;
        currentMana = maxMana;

        //set regenaration counters
        healthRegenCounter = healthRegenRate;
        staminaRegenCounter = staminaRegenRate;
        manaRegenCounter = manaRegenRate;

        //UI
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        
        UIController.instance.staminaSlider.maxValue = maxStamina;
        UIController.instance.staminaSlider.value = currentStamina;
        UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
        
        UIController.instance.manaSlider.maxValue = maxMana;
        UIController.instance.manaSlider.value = currentMana;
        UIController.instance.manaText.text = "MANA: " + currentMana + "/" + maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        //set maxHealth, maxStamina, maxMana
        maxHealth = Convert.ToInt32(PlayerController.instance.strength) * 20;
        maxStamina = Convert.ToInt32(PlayerController.instance.endurance) * 20;
        maxMana = Convert.ToInt32(PlayerController.instance.intelligence) * 10;

        healthRegenRate = 25 / PlayerController.instance.endurance;
        staminaRegenRate = 5 / PlayerController.instance.willpower;
        manaRegenRate = 25 / PlayerController.instance.willpower;

        //check if player is moving
        if (lastPosition == gameObject.transform.position)
        {
            notMoving = true;
        }
        else
        {            
            notMoving = false;
            lastPosition = gameObject.transform.position;
        }

        //invicibility
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            UIController.instance.imageSp1Duration.fillAmount -= 1 / invincibleLength * Time.deltaTime;
           
        }
        
        if (invincibleCounter <= 0)
        {
            invincibleLength = 1;
        }



        //Health
        if (currentHealth != maxHealth)
            healthRegenCounter -= Time.deltaTime;

        if (healthRegenCounter <= 0)
        {
            HealthRegen();
            healthRegenCounter = healthRegenRate;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }

        //Stamina
        if (currentStamina != maxStamina)
            staminaRegenCounter -= Time.deltaTime;

        if (staminaRegenCounter <=0)
        {
            StaminaRegen();
            staminaRegenCounter = staminaRegenRate;
        }        
        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            UIController.instance.staminaSlider.value = currentStamina;
            UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
        }

        //Mana
        if (currentMana != maxMana)
            manaRegenCounter -= Time.deltaTime;

        if (manaRegenCounter <= 0)
        {
            ManaRegen();
            manaRegenCounter = manaRegenRate;
        }
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
            UIController.instance.manaSlider.value = currentMana;
            UIController.instance.manaText.text = "MANA: " + currentMana + "/" + maxMana;
        }    
    }

    #region Functions
    public void DamagePlayer(int damageAmount)
    {
        //reset health regeneration counter / can't regenerate when taking damage
        healthRegenCounter = healthRegenRate;

        if (invincibleCounter <= 0 && !GameManager.instance.levelEnding)
        {
            //AudioManager.instance.PlaySFX(7);

            currentHealth -= damageAmount;

            //death
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;

                GameManager.instance.PlayerDied();

                //AudioManager.instance.StopBGM();
                //AudioManager.instance.PlaySFX(6);
                //AudioManager.instance.StopSFX(7);
            }

            invincibleCounter = invincibleLength;

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
            UIController.instance.ShowDamage();
        }
    }

    public void HealthRegen()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += maxHealth /100;

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

    public void SpendStamina(int _amount)
    {
        if (currentStamina > 0 && notMoving == false)
        {
            currentStamina -= _amount;
            UIController.instance.staminaSlider.value = currentStamina;
            UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
            UIController.instance.staminaSlider.value = currentStamina;
            UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
        }
    }

    public void SpendMana(int _amount)
    {
        if (currentMana > 0)
        {
            currentMana -= _amount;
            UIController.instance.manaSlider.value = currentMana;
            UIController.instance.manaText.text = "STAMINA: " + currentMana + "/" + maxMana;
        }

        if (currentMana < 0)
        {
            currentMana = 0;
            UIController.instance.manaSlider.value = currentMana;
            UIController.instance.manaText.text = "STAMINA: " + currentMana + "/" + maxMana;
        }
    }

    public void StaminaRegen()
    {
        if (currentStamina < maxStamina && !Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift) && notMoving == true)
        {
            //decimalCurrentStamina += Time.deltaTime;
            //currentStamina = Mathf.RoundToInt(decimalCurrentStamina);
            currentStamina++;
            /*currentStamina += maxStamina / 10*/;

            UIController.instance.staminaSlider.value = currentStamina;
            UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
        }
    }

    public void ManaRegen()
    {
        if (currentMana < maxMana)
        {
            currentMana += 1;

            UIController.instance.manaSlider.value = currentMana;
            UIController.instance.manaText.text = "MANA: " + currentMana + "/" + maxMana;
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void StaminaRestore(int restoreAmount)
    {
        currentStamina += restoreAmount;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        UIController.instance.staminaSlider.value = currentStamina;
        UIController.instance.staminaText.text = "STAMINA: " + currentStamina + "/" + maxStamina;
    }

    public void ManaRestore(int restoreAmount)
    {
        currentMana += restoreAmount;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        UIController.instance.manaSlider.value = currentMana;
        UIController.instance.manaText.text = "MANA: " + currentMana + "/" + maxMana;
    }
    #endregion

}