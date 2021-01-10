using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public static PickupController instance;

    public int itemsAmount;

    public bool isHealthPot;
    public bool isStamPot;
    public bool isManaPot;
    public bool isShuriken;
    public bool isBronzeKey;
    public bool isSilverKey;
    public bool isGoldKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isHealthPot)
            {
                ConsumablesController.instance.AddHealthPot(itemsAmount);
            }
            if (isStamPot)
            {
                ConsumablesController.instance.AddStamPot(itemsAmount);
            }
            if (isManaPot)
            {
                ConsumablesController.instance.AddManaPot(itemsAmount);
            }
            if (isShuriken)
            {
                PlayerController.instance.activeThrWeapon.currentAmmo += itemsAmount;
                //update ui
                UIController.instance.ammoText.text = "SHURIKEN: " + PlayerController.instance.activeThrWeapon.currentAmmo;
            }
            if (isBronzeKey)
            {
                ConsumablesController.instance.hasBronzeKey = true;
            }
            if (isSilverKey)
            {
                ConsumablesController.instance.hasSilverKey = true;
            }
            if (isGoldKey)
            {
                ConsumablesController.instance.hasGoldKey = true;
            }


            Destroy(gameObject);
        }
    }
}
