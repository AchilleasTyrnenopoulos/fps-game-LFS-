using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablesController : MonoBehaviour
{
    public static ConsumablesController instance;

    [Header("Health Pots")]
    public int healthPots;
    public int healthPotAmount;
    
    [Header("Stamina Pots")]
    public int stamPots;
    public int stamPotAmount;

    [Header("Mana Pots")]
    public int manaPots;
    public int manaPotAmount;

    [Header("Keys")]
    public bool hasBronzeKey = false;
    public bool hasSilverKey = false;
    public bool hasGoldKey = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set potions 'strength'
        healthPotAmount = (int)PlayerController.instance.intelligence * 10;
        stamPotAmount = (int)PlayerController.instance.intelligence * 5;
        manaPotAmount = (int)PlayerController.instance.intelligence * 2;

        UpdatePotsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && healthPots > 0)
        {
            PlayerHealthController.instance.HealPlayer(healthPotAmount);
            healthPots--;

            UpdatePotsUI();
        }

        if (Input.GetKeyDown(KeyCode.T) && stamPots > 0)
        {
            PlayerHealthController.instance.StaminaRestore(stamPotAmount);
            stamPots--;

            UpdatePotsUI();
        }

        if (Input.GetKeyDown(KeyCode.G) && manaPots > 0)
        {
            PlayerHealthController.instance.ManaRestore(manaPotAmount);
            manaPots--;

            UpdatePotsUI();
        }
    }

    public void UpdatePotsUI()
    {
        UIController.instance.healthPotText.text = healthPots + "";
        UIController.instance.staminaPotText.text = stamPots + "";
        UIController.instance.manaPotText.text = manaPots + "";
    }

    public void AddHealthPot (int _itemsAmount)//make a 'universal' add items function
    {        
        healthPots += _itemsAmount;
        UpdatePotsUI();
    }

    public void AddStamPot (int _itemsAmount)
    {
        stamPots += _itemsAmount;
        UpdatePotsUI();        
    }

    public void AddManaPot (int _itemsAmount)
    {
        manaPots += _itemsAmount;
        UpdatePotsUI();
    }
}
