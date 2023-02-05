using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SeedCounter.seedAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Seed collected");
            Destroy(gameObject);
            SeedCounter.seedAmount += 1;  
        }
    }
}
