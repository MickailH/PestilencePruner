using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /*
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }*/

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ToControls()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
            Application.Quit();
    }

}



