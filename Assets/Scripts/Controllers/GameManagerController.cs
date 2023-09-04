using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManagerController : MonoBehaviour
{

    [SerializeField]
    private int _player = 1;
    public int Player
    {
        get { return _player; }
        set
        {
            if (_player != value)
            {
                _player = value;
                OnPlayerChanged();
            }
        }
    }

    private void OnPlayerChanged()
    {
        _view.UpdateCurrentPlayerIcon(_player);
    }


    private GameManagerModel _model;

    private GameManagerView _view;

    [SerializeField]
    private Button[] _buttons;
    
    [SerializeField]
    private int _nextSchema = -1;
    // Update is called once per frame
    void Awake()
    {
        _model = FindObjectOfType<GameManagerModel>();
        _view = FindObjectOfType<GameManagerView>();
    }

    public void ButtonPress(Button button)
    {
        Debug.Log("Pressed button " + button.identifier);
        CheckSlotAndPlay(button);
    }

    private void CheckSlotAndPlay(Button button)
    {
        if (button.ParentButton == null)
        {
            if (_model.matrixes[0][button.identifier-1] == 0) //la casella principale è ancora indeterminata
            {
                if (_nextSchema == -1)
                {
                    _view.BackButtonEnabled = true;
                }

                _view.MoveCamera(button.identifier);
                button.SetGroupBelow(true);
                Debug.Log("Acting on button", button.gameObject);
            }
        }
        else
        {
            bool playSuccessful = _model.matrixes[button.ParentButton.identifier][button.identifier] == 0;

            if (playSuccessful)
            {
                _model.matrixes[button.ParentButton.identifier][button.identifier] = _player;
                _view.SetIcon(_player, button, true);
                int winSituation = CheckForWin(_model.matrixes[button.ParentButton.identifier]);
                if (winSituation==1)
                {
                    _view.SetIcon(_player, button.ParentButton, false);
                    _model.matrixes[0][button.ParentButton.identifier-1] = _player;
                }
                else if (winSituation == -1)
                {
                    _model.matrixes[0][button.ParentButton.identifier-1] = -1;
                }

                SetAllSubGroupsDisabled();
                
                //settare nuovo valore di nextSchema
                _nextSchema = (_model.matrixes[0][((button.ParentButton==null)?button.identifier+1:(button.identifier))] != 0) ? -1 :button.identifier+1;
                Debug.Log("Next Schema: " + _nextSchema);
                _view.MoveCamera((_nextSchema == -1) ? 0 : _nextSchema);
                if (_nextSchema > 0)
                {
                    _buttons[_nextSchema - 1].SetGroupBelow(true);
                }

                _view.BackButtonEnabled = false;
                CheckForWin(_model.matrixes[0]);
                SwitchPlayer();
            }
        }
    }

    private void SwitchPlayer()
    {
        Player %= 2;
        Player++;
    }
    
    private int CheckForWin(int[] board)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (board[row * 3] == _player && board[row * 3 + 1] == _player && board[row * 3 + 2] == _player)
                return 1; // Giocatore ha vinto
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (board[col] == _player && board[col + 3] == _player && board[col + 6] == _player)
                return 1; // Giocatore ha vinto
        }

        // Check diagonals
        if (board[0] == _player && board[4] == _player && board[8] == _player)
            return 1; // Giocatore ha vinto

        if (board[2] == _player && board[4] == _player && board[6] == _player)
            return 1; // Giocatore ha vinto

        // Nessuno ha ancora vinto, controlla se è un pareggio
        bool isFull = true;
        foreach (int cell in board)
        {
            if (cell == 0)
            {
                isFull = false; // Ci sono celle vuote, il gioco non è ancora finito
                break;
            }
        }

        if (isFull)
            return -1; // Pareggio

        return 0; // Nessuno ha ancora vinto e ci sono ancora mosse possibili
    }

    public void BackToMainGame()
    {
        _view.MoveCamera();
        _view.BackButtonEnabled = false;
    SetAllSubGroupsDisabled();  
    }

    public void SetAllSubGroupsDisabled()
    {
        foreach (Button b in _buttons)
        {
            b.GroupBelowMe.interactable=false;
            b.GroupBelowMe.blocksRaycasts=false;
        }
    }
    
    
}
