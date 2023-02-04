using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene(0);//goto Main menu on escape key
        }
    }

    public void BacktoMain()
    {
        SceneManager.LoadScene(0);
    }
}
