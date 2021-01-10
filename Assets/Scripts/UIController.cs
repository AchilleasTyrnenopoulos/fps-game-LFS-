using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Variables

    public static UIController instance;//to make accessible from every other script

    [Header("SLIDERS & TEXTS")]
    public Slider healthSlider;
    public Text healthText;
    
    public Slider staminaSlider;
    public Text staminaText;
    
    public Slider manaSlider;
    public Text manaText;

    public Text healthPotText;
    public Text staminaPotText;
    public Text manaPotText;

    public Text ammoText;

    public Image imageSp1Cooldown;
    public Image imageSp1Duration;
    public Image imageSp2Cooldown;
    public Image imageSp2Duration;
    public Image imageSp3Cooldown;
    public Image imageSp3Duration;
    public Image imageSp4Cooldown;

    public Image imageHealCooldown;
    public Image imageStamRestCooldown;

    public Text timerText;
    public Text timerText2;



    //public Text ammoText;

    [Header("FX")]
    public Image damageEffect;
    public float damageAlpha = .25f, damageFadeSpeed = 3f;

    public GameObject pauseScreen;

    public Image blackScreen;
    public float fadeSpeed = 1.5f;

    #endregion

    private void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (damageEffect.color.a != 0)
        {
            damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }

        //fade in/out
        if (!GameManager.instance.levelEnding)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
        else
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        }    
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
    }
}
