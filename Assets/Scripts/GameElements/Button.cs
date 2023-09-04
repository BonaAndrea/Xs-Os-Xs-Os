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
    public Button ParentButton = null;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManagerController>();
        if (transform.parent.transform.parent.GetComponent<Button>())
        {
            ParentButton = transform.parent.transform.parent.GetComponent<Button>();
        }
    }

    public void OnButtonPress()
    {
        GameManager.ButtonPress(this);
    }

    public void SetGroupAbove(bool status)
    {
        if (GroupAboveMe == null || ManagerAboveMe == null) return;
        GroupAboveMe.interactable = status;
    }

    public void SetGroupBelow(bool status)
    {
        if (GroupBelowMe == null || ManagerBelowMe == null) return;
        if (status)
        {
            ManagerBelowMe.FadeIn();
        }
        else
        {
            ManagerBelowMe.FadeOut();
        }
        GroupBelowMe.interactable = status;
        GroupBelowMe.blocksRaycasts = status;
    }

}
