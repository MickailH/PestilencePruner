using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedCounter : MonoBehaviour
{
    private TMP_Text text;
    public static int seedAmount;
    // Start is called before the first frame update
    void Start()
    {
        seedAmount = 0;
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (seedAmount != 0) { 
        text.text = (seedAmount / 2).ToString();
    }
    }
}
