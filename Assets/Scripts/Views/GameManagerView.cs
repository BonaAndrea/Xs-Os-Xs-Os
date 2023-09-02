using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{

    public Sprite X, O;
    private CameraManager _cameraManager;

    private void Awake()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    public void SetIcon(int player, Button i, bool hasParent)
    {   Transform t = i.transform.Find("Result");
        if (!hasParent)
        {
            i.GetComponentInChildren<CanvasGroupManager>().FadeOut();
        }
        if (player == 1)
        {
            t.gameObject.SetActive(true);
            t.GetComponent<Image>().sprite = X;
        }
        if (player == 2)
        {
            t.gameObject.SetActive(true);
            t.GetComponent<Image>().sprite = O;
        }
    }

    public void MoveCamera(int identifier, bool hasParent = false)
    {
        if (!hasParent)
        {
            StartCoroutine(_cameraManager.MoveToPosition(identifier));
        }
        else
        {
            StartCoroutine(_cameraManager.MoveToPosition(0));

        }
    }
    
    
}
