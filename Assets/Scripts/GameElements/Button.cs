using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public int identifier = 0;
    public GameManagerController GameManager;
    public CanvasGroup GroupAboveMe;
    public CanvasGroupManager ManagerAboveMe;
    public CanvasGroup GroupBelowMe;
    public CanvasGroupManager ManagerBelowMe;
    public Button ParentButton = null;
    public Transform t;
    public Image i;


    private void Awake()
    {
        GameManager = FindObjectOfType<GameManagerController>();
        if (transform.parent.transform.parent.GetComponent<Button>())
        {
            ParentButton = transform.parent.transform.parent.GetComponent<Button>();
        }

        t = transform.Find("Result");
        if(t.GetComponent<Image>()){
            i = t.GetComponent<Image>();
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
