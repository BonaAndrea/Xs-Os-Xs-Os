using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public int identifier = 0;
    public GameManagerController GameManager;
    public CanvasGroup GroupAboveMe;
    public CanvasGroupManager ManagerAboveMe;
    public CanvasGroup GroupBelowMe;
    public CanvasGroupManager ManagerBelowMe;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManagerController>();
    }

    public void OnButtonPress()
    {
        GameManager.ButtonPress(this);
    }
    
}
