using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    public float timer;
    private int timerMin;

    public float timeLimit;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        timerMin = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealthController.instance.currentHealth > 0)
            timer += Time.deltaTime;

        if (timer >= 59)
        {
            timer = 0;
            timerMin++;
        }

        if (timer >= timeLimit)
        {
            PlayerHealthController.instance.gameObject.SetActive(false);
            PlayerHealthController.instance.currentHealth = 0;

            GameManager.instance.PlayerDied();           
        }

        if (timeLimit > 59)
        {
            UIController.instance.timerText.text = Convert.ToString(timerMin) + " : " + (int)timer + " / " + (timeLimit / 60 + " : " + timeLimit % 60);
            UIController.instance.timerText2.text = Convert.ToString(timerMin) + " : " + (int)timer + " / " + (timeLimit / 60 + " : " + timeLimit % 60);
        }
        else
        {
            UIController.instance.timerText.text = Convert.ToString(timerMin) + " : " + (int)timer + " / " + ("00 : " + timeLimit);
            UIController.instance.timerText2.text = Convert.ToString(timerMin) + " : " + (int)timer + " / " + ("00 : " + timeLimit);
        }

    }
}
