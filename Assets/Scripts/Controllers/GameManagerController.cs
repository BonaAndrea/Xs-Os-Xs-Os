using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManagerController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress(int identifier)
    {
        Debug.Log("Pressed button " + identifier);
    }
    
    
}
