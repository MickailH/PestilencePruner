using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorManager : MonoBehaviour
{
    public int sceneID;
    public int SeedsNeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SeedCounter.seedAmount/2 >= SeedsNeed)
        {
            SceneManager.LoadScene(sceneID);
        }
    }

}
