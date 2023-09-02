using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{

    public Sprite X, O;
    [SerializeField]
    private CameraManager _cameraManager;
    [SerializeField]
    private CanvasGroupManager _backButton;

    public bool BackButtonEnabled = false;
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

    public void MoveCamera(int identifier=0)
    {
        StartCoroutine(_cameraManager.MoveToPosition(identifier));
        if (identifier != 0)
        {
            _backButton.FadeIn();
        }
        else
        {
            _backButton.FadeOut();
        }
    }


}
