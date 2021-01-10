using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapon : MonoBehaviour
{
    public static ThrowingWeapon instance;

    public GameObject throwingWeapon;

    public float fireRate;
    [HideInInspector]//variable is accessible for other scripts but can't be edited in inspector
    public float fireCounter;

    public int currentAmmo;

    MeshRenderer mr;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mr = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
        
        if (currentAmmo <= 0)
            mr.enabled = false;
        else
            mr.enabled = true;

    }
}
