using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public static AttributesManager instance;

    public string nextScene;
    public string previousScene;

    public float globalStrength = 5;
    public float globalAgility = 5;
    public float globalIntelligense = 5;
    public float globalEndurance = 5;
    public float globalWillpower = 5;

 

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        globalStrength = PlayerController.instance.strength;
        globalAgility = PlayerController.instance.agility;
        globalIntelligense = PlayerController.instance.intelligence;
        globalEndurance = PlayerController.instance.endurance;
        globalWillpower = PlayerController.instance.willpower;
    }

    // Update is called once per frame
    void Update()
    {
        //globalStrength = PlayerController.instance.strength;
        //globalAgility = PlayerController.instance.agility;
        //globalIntelligense = PlayerController.instance.intelligence;
        //globalEndurance = PlayerController.instance.endurance;
        //globalWillpower = PlayerController.instance.willpower;

        //for testing 
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(nextScene);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(previousScene);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
