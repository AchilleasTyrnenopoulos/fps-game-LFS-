using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour
{
    public Text strPointsText;
    public Text aglPointsText;
    public Text intPointsText;
    public Text endPointsText;
    public Text willPointsText;
    public Text availablePointsText;

    public Image blackScreen;
    public float blackScreenFade = 2f;

    private int availablePoints = 3;


    // Start is called before the first frame update
    void Start()
    {
        strPointsText.text = Convert.ToString(AttributesManager.instance.globalStrength);
        aglPointsText.text = Convert.ToString(AttributesManager.instance.globalAgility);
        intPointsText.text = Convert.ToString(AttributesManager.instance.globalIntelligense);
        endPointsText.text = Convert.ToString(AttributesManager.instance.globalEndurance);
        willPointsText.text = Convert.ToString(AttributesManager.instance.globalWillpower);
        availablePointsText.text = Convert.ToString(availablePoints);
    }

    // Update is called once per frame
    void Update()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, blackScreenFade * Time.deltaTime));

        strPointsText.text = Convert.ToString(AttributesManager.instance.globalStrength);
        aglPointsText.text = Convert.ToString(AttributesManager.instance.globalAgility);
        intPointsText.text = Convert.ToString(AttributesManager.instance.globalIntelligense);
        endPointsText.text = Convert.ToString(AttributesManager.instance.globalEndurance);
        willPointsText.text = Convert.ToString(AttributesManager.instance.globalWillpower);
        availablePointsText.text = Convert.ToString(availablePoints);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        
    }

    public void StrengthAdd()
    {
        if (availablePoints > 0)
        {
            AttributesManager.instance.globalStrength++;
            availablePoints--;
        }
    }
    public void AgilityAdd()
    {
        if (availablePoints > 0)
        {
            AttributesManager.instance.globalAgility++;
            availablePoints--;
        }
    }
    public void IntelligenseAdd()
    {
        if (availablePoints > 0)
        {
            AttributesManager.instance.globalIntelligense++;
            availablePoints--;
        }
    }
    public void EnduranceAdd()
    {
        if (availablePoints > 0)
        {
            AttributesManager.instance.globalEndurance++;
            availablePoints--;
        }
    }
    public void WillpowerAdd()
    {
        if (availablePoints > 0)
        {
            AttributesManager.instance.globalWillpower++;
            availablePoints--;
        }
    }

    public void StrengthSubtract()
    {
        if (availablePoints < 3 && AttributesManager.instance.globalStrength > 5)
        {
            AttributesManager.instance.globalStrength--;
            availablePoints++;
        }
    }
    public void AgilitySubtract()
    {
        if (availablePoints < 3 && AttributesManager.instance.globalAgility > 5)
        {
            AttributesManager.instance.globalAgility--;
            availablePoints++;
        }
    }
    public void IntelligenseSubtract()
    {
        if (availablePoints < 3 && AttributesManager.instance.globalIntelligense > 5)
        {
            AttributesManager.instance.globalIntelligense--;
            availablePoints++;
        }
    }
    public void EnduranceSubtract()
    {
        if (availablePoints < 3 && AttributesManager.instance.globalEndurance > 5)
        {
            AttributesManager.instance.globalEndurance--;
            availablePoints++;
        }
    }
    public void WillpowerSubtract()
    {
        if (availablePoints < 3 && AttributesManager.instance.globalWillpower > 5)
        {
            AttributesManager.instance.globalWillpower--;
            availablePoints++;
        }
    }

}
