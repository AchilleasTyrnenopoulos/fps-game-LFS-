using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;
    //private float waitAfterVictory = 2f;

    [HideInInspector]
    public bool levelEnding;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//not moving the mouse outside the area of the game 
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //handle victory scene
        //VictoryScene();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    #region Functions
    //'respawn' when player dies
    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo());

        //reset spells
        Time.timeScale = 1;

        //instant respawn instead of coroutine
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //coroutine 
    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUnpause()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            //PlayerController.instance.footstepsFast.UnPause();
            //PlayerController.instance.footstepsSlow.UnPause();
        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;

            //PlayerController.instance.footstepsFast.Pause();
            //PlayerController.instance.footstepsSlow.Pause();
        }
    }


    //handle victory scene
    //public void VictoryScene()
    //{


    //    if (GameObject.FindWithTag("Enemy") == null)
    //    {

    //        waitAfterVictory -= Time.deltaTime;

    //        if (waitAfterVictory <= 0)
    //        {

    //            SceneManager.LoadScene("VictoryScene");

    //            Cursor.lockState = CursorLockMode.None;
    //        }

    //    }

    //}
    #endregion
}
