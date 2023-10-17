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
    [SerializeField] private Image _currentPlayerIcon;
    [SerializeField] private CanvasGroupManager _endScreenCanvas;
    [SerializeField] private GameObject _tieText, _winText;
    [SerializeField] private Image _winner;
    private GameManagerController _controller;
    private GameManagerModel _model;
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
        _controller = transform.GetComponent<GameManagerController>();
        _model = transform.GetComponent<GameManagerModel>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _cameraManager.OnCoroutineFinished.AddListener(_controller.HandleGroups);
    }

    public void SetIcon(int player, Button b, bool hasParent)
    {
        if (!hasParent)
        {
            b.GetComponentInChildren<CanvasGroupManager>().FadeOut();
        }

        if (player == 1)
        {
            b.t.gameObject.SetActive(true);
            b.i.sprite = X;
        }

        if (player == 2)
        {
           b.t.gameObject.SetActive(true);
            b.i.sprite = O;
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

    public void WinScreen(int mainWinSituation)
    {
        _endScreenCanvas.FadeIn();
        Debug.Log("Winner: " + mainWinSituation);
        switch (mainWinSituation)
        {
            case 2:
                _tieText.SetActive(true);
                break;
            case 1:
                _winText.SetActive(true);
                _winner.sprite = X;
                break;
            case -1:
                _winText.SetActive(true);
                _winner.sprite = O;
                break;
        }
    }
    
    public void StopAllOtherCoroutines(){
        StopAllCoroutines();
    }
    
    public IEnumerator MoveCameraCoroutine(int identifier, Action onMoveComplete)
    {
        // Avvia l'animazione della telecamera
        yield return StartCoroutine(_cameraManager.MoveToPosition((identifier>0)?identifier:identifier+1));

        if (_controller.NextSchema == -1)
        {
            while (_cameraManager.IsAnimating)
                yield return null;
        }

        onMoveComplete?.Invoke();
    }

    
    
}
