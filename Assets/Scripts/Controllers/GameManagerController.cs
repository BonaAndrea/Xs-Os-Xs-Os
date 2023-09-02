using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManagerController : MonoBehaviour
{

    private int player = 1;

    private GameManagerModel _model;

    private GameManagerView _view;

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
        if (button.transform.parent.transform.parent.GetComponent<Button>())
        {
            Debug.Log("This is a child", button.gameObject);
            Button parentButton = button.transform.parent.transform.parent.GetComponent<Button>();
            CheckSlotAndPlay(button, parentButton);
        }
        else
        {
            Debug.Log("This is a parent");
            CheckSlotAndPlay(button);
        }
    }

    private void CheckSlotAndPlay(Button button, Button parentButton)
    {
        bool playSuccessful = _model.matrixes[parentButton.identifier][button.identifier] == 0;

        if (playSuccessful)
        {
            _model.matrixes[parentButton.identifier][button.identifier] = player;
            _view.SetIcon(player, button, true);
            if (CheckForWin(_model.matrixes[parentButton.identifier]))
            {
                _view.SetIcon(player, parentButton, false);
            }
        }
        _view.MoveCamera(0);
    }
    
    private void CheckSlotAndPlay(Button button)
    {
        if (_model.matrixes[0][button.identifier-1] == 0) //la casella principale è ancora indeterminata
        {
            if (_nextSchema == -1)
            {
                _view.MoveCamera(button.identifier);
            }
        }

        CheckForWin(_model.matrixes[0]);
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        player %= 2;
        player++;
    }
    
    private bool CheckForWin(int[]board)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (board[row * 3] == player && board[row * 3 + 1] == player && board[row * 3 + 2] == player)
                return true;
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (board[col] == player && board[col + 3] == player && board[col + 6] == player)
                return true;
        }

        // Check diagonals
        if (board[0] == player && board[4] == player && board[8] == player)
            return true;

        if (board[2] == player && board[4] == player && board[6] == player)
            return true;

        return false;
    }
    
    
}
