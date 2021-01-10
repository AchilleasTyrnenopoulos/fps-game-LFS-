using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionRaycast : MonoBehaviour
{
    private GameObject raycastedGo;

    [SerializeField] private int raylength = 10;
    [SerializeField] private LayerMask layermaskInteract;

    [SerializeField] private Image uiCrosshair;

    private void Update()
    {
        RaycastHit hit;        

        if (Physics.Raycast(PlayerController.instance.camTrans.transform.position, PlayerController.instance.camTrans.forward, out hit, raylength, layermaskInteract.value))
        {
            raycastedGo = hit.collider.gameObject;
            //change the crooshair
            CrosshairActive();

            //handle door interactables
            if (hit.collider.CompareTag("BronzeDoor"))
            {                
                if (Input.GetKeyDown(KeyCode.E) && ConsumablesController.instance.hasBronzeKey)
                {                    
                    raycastedGo.GetComponentInParent<Animator>().enabled = true;
                }    
            }
            if (hit.collider.CompareTag("SilverDoor"))
            {
                if (Input.GetKeyDown(KeyCode.E) && ConsumablesController.instance.hasSilverKey)
                {
                    raycastedGo.GetComponentInParent<Animator>().enabled = true;
                }
            }
            if (hit.collider.CompareTag("GoldenDoor"))
            {                
                if (Input.GetKeyDown(KeyCode.E) && ConsumablesController.instance.hasGoldKey)
                {
                    raycastedGo.GetComponentInParent<Animator>().enabled = true;
                }
            }
        }
        else
        {
            //crosshair normal
            CrosshairNormal();
        }
    }

    void CrosshairActive()
    {
        uiCrosshair.color = Color.green;
    }

    void CrosshairNormal()
    {
        uiCrosshair.color = Color.white;
    }
}
