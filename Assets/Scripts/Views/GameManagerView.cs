using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{

    public Sprite X, O;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private CanvasGroupManager _backButton;
    [SerializeField] private bool backButtonEnabled = false;
    private GameManagerController _gameManagerController;
    [SerializeField] private Image _currentPlayerIcon;

    public bool BackButtonEnabled
    {
        get { return backButtonEnabled; }
        set
        {
            if (backButtonEnabled != value)
            {
                backButtonEnabled = value;
                OnBackButtonEnabledChanged();
            }
        }
    }

    private void Awake()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
        _gameManagerController = FindObjectOfType<GameManagerController>();
    }

    public void SetIcon(int player, Button i, bool hasParent)
    {
        Transform t = i.transform.Find("Result");
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

    public void MoveCamera(int identifier = 0)
    {
        StartCoroutine(_cameraManager.MoveToPosition(identifier));
    }

    private void OnBackButtonEnabledChanged()
    {
        _backButton.SetProperties(backButtonEnabled);
    }

    public void UpdateCurrentPlayerIcon(int player)
    {
        _currentPlayerIcon.sprite = (player == 1) ? X : O;
    }
}
