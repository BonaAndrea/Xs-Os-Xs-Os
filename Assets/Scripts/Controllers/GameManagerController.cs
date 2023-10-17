using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;


[System.Serializable]
public struct BestMove
{
    public int subTrisIndex;
    public int cellIndex;
    public int score;
}



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

    public int NextSchema
    {
        get { return _nextSchema; }
        set { _nextSchema = value; }
    }

    [SerializeField] private AudioClip _moveSound;
    [SerializeField]
    private AudioSourceController _audioSourceController;
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        _model = FindObjectOfType<GameManagerModel>();
        _view = FindObjectOfType<GameManagerView>();
    }

    public void ButtonPress(Button button)
    {
        if (_model.GameMode == GameMode.Single && _player != 1) return;
        CheckSlotAndPlay(button);
    }

private void CheckSlotAndPlay(Button button)
{
    if (button.ParentButton == null)
    {
        if (_model.matrixes[0][button.identifier - 1] == 0) //la casella principale è ancora indeterminata
        {
            if (_nextSchema == -1)
            {
                _view.BackButtonEnabled = true;
            }

            _view.MoveCamera(button.identifier);
            button.GroupBelowMe.alpha = 1f;
            button.SetGroupAbove(false);
            Debug.Log("Acting on button", button.gameObject);
        }
    }
    else
    {
        int playerSymbol = (_player == 1) ? 1 : -1;
        bool playSuccessful = _model.matrixes[button.ParentButton.identifier][button.identifier] == 0;

        if (playSuccessful)
        {
            _model.matrixes[button.ParentButton.identifier][button.identifier] = playerSymbol;
            _view.SetIcon(_player, button, true);
            int winSituation = CheckForWin(_model.matrixes[button.ParentButton.identifier]);
            if (winSituation != 0)
            {
                _view.SetIcon(_player, button.ParentButton, false);
                _model.matrixes[0][button.ParentButton.identifier - 1] = playerSymbol;
            }
            else if (winSituation == -1)
            {
                _model.matrixes[0][button.ParentButton.identifier - 1] = -1;
            }

            SetAllSubGroupsDisabled();

            int mainWinSituation = CheckForWin(_model.matrixes[0]);
            if (mainWinSituation != 0)
            {
                _view.MoveCamera();
                _view.WinScreen(mainWinSituation);
                return;
            }

            //settare nuovo valore di nextSchema
            _nextSchema = (_model.matrixes[0][((button.ParentButton == null) ? button.identifier + 1 : (button.identifier))] != 0) ? -1 : button.identifier + 1;
            Debug.Log("Next Schema: " + _nextSchema);
            _view.MoveCamera((_nextSchema == -1) ? 0 : _nextSchema);
            if (_nextSchema > 0)
            {
                _buttons[_nextSchema - 1].GroupBelowMe.alpha = 1f;
            }

            _view.BackButtonEnabled = false;
            SwitchPlayer();
        }
    }
}


    private void SwitchPlayer()
    {
        Player %= 2;
        Player++;
        if (_model.GameMode == GameMode.Single)
        {
            if (Player == 2)
            {
                PerformComputerMove();
            }
        }
    }
    
    private int CheckForWin(int[] board)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (board[row * 3] == 1 && board[row * 3 + 1] == 1 && board[row * 3 + 2] == 1)
                return 1; // Giocatore 1 ha vinto
            if (board[row * 3] == -1 && board[row * 3 + 1] == -1 && board[row * 3 + 2] == -1)
                return -1; // Giocatore 2 ha vinto
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (board[col] == 1 && board[col + 3] == 1 && board[col + 6] == 1)
                return 1; // Giocatore 1 ha vinto
            if (board[col] == -1 && board[col + 3] == -1 && board[col + 6] == -1)
                return -1; // Giocatore 2 ha vinto
        }

        // Check diagonals
        if (board[0] == 1 && board[4] == 1 && board[8] == 1)
            return 1; // Giocatore 1 ha vinto
        if (board[0] == -1 && board[4] == -1 && board[8] == -1)
            return -1; // Giocatore 2 ha vinto

        if (board[2] == 1 && board[4] == 1 && board[6] == 1)
            return 1; // Giocatore 1 ha vinto
        if (board[2] == -1 && board[4] == -1 && board[6] == -1)
            return -1; // Giocatore 2 ha vinto

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
            return 2; // Pareggio

        return 0; // Nessuno ha ancora vinto e ci sono ancora mosse possibili
    }


    public void BackToMainGame()
    {
        _view.MoveCamera();
        _view.BackButtonEnabled = false;
        SetAllSubGroupsDisabled();  
        SetAllTopGroupsEnabled();
    }

    public void SetAllSubGroupsDisabled()
    {
        for(int i=0; i<9; i++) 
        {
            _buttons[i].GroupBelowMe.interactable=false;
            _buttons[i].GroupBelowMe.blocksRaycasts=false;
        }
    }
    
    public void SetAllTopGroupsEnabled()
    {
        foreach (Button b in _buttons)
        {
            b.SetGroupAbove(true);
        }
    }

    public void HandleGroups(int identifier)
    {
        if (identifier == 0)
        {
            SetAllTopGroupsEnabled();
        }
        else
        {
            _buttons[identifier - 1].SetGroupBelow(true);
        }
    }
    public void DisableAllSubGroups()
    {
        for(int i=0; i<9; i++){
            _buttons[i].GroupBelowMe.interactable=false;
            _buttons[i].GroupBelowMe.blocksRaycasts=false;
            _buttons[i].GroupBelowMe.alpha = 0f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        foreach (var matrix in _model.matrixes)
        {
            for (int j = 0; j < 9; j++)
            {
                matrix[j] = 0;
            }
        }

        var buttons = FindObjectsOfType<Button>(true);
        
        foreach (var b in buttons)
        {
            b.i.sprite = null;
            b.t.GameObject().SetActive(false);
        }
    }
    
private BestMove MiniMaxForPlayer2(int depth, bool isMaximizing)
{
    // Verifica se il gioco è finito
    if (CheckForWin(_model.matrixes[0]) != 0 || depth == 0)
        return new BestMove { subTrisIndex = -1, cellIndex = -1, score = EvaluateBoard(isMaximizing) };

    // Genera una mossa casuale se la profondità di ricerca è bassa
    if (depth < 3)
    {
        int subTrisIndex = Random.Range(1, 10);
        int cellIndex = Random.Range(0, 9);

        return new BestMove { subTrisIndex = subTrisIndex, cellIndex = cellIndex, score = EvaluateBoard(isMaximizing) };
    }

    // Esegui il normale algoritmo MiniMax
    int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
    BestMove bestMove = new BestMove { subTrisIndex = -1, cellIndex = -1, score = bestScore };
    int randomMovesCount = 0;

    for (int subTrisIndex = 1; subTrisIndex <= 9; subTrisIndex++)
    {
        if (_nextSchema != -1 && _nextSchema != subTrisIndex)
            continue;  // Se è specificato un sottoschema, salta gli altri

        foreach (int cellIndex in GetAvailableCells(subTrisIndex))
        {
            _model.matrixes[subTrisIndex][cellIndex] = 2;
            int score = MiniMaxForPlayer1(depth - 1, !isMaximizing).score;
            _model.matrixes[subTrisIndex][cellIndex] = 0;

            if (isMaximizing && score > bestScore || !isMaximizing && score < bestScore)
            {
                bestScore = score;
                bestMove = new BestMove { subTrisIndex = subTrisIndex, cellIndex = cellIndex, score = bestScore };
            }
        }
    }

    // Se non è stata trovata alcuna mossa valida nel sottoschema assegnato o se non c'è un sottoschema assegnato
    if (bestMove.subTrisIndex == -1)
    {
        if (_nextSchema != -1)
            randomMovesCount++;  // Incrementa solo se non c'è un sottoschema specifico

        // Se il sottoschema indicato è valido, fai una mossa casuale in quel sottoschema
        foreach (int cellIndex in GetAvailableCells(_nextSchema))
        {
            return new BestMove { subTrisIndex = _nextSchema, cellIndex = cellIndex, score = EvaluateBoard(isMaximizing) };
        }
    }
    else
    {
        return bestMove;  // Se c'è una mossa migliore trovata nel sottoschema assegnato, restituisci quella
    }

    // Fai una mossa casuale in qualsiasi sottoschema se non ci sono condizioni precedenti
    return new BestMove { subTrisIndex = Random.Range(1, 10), cellIndex = Random.Range(0, 9), score = EvaluateBoard(isMaximizing) };
}



// Funzione MiniMax semplificata per il giocatore 1 (umano)
private BestMove MiniMaxForPlayer1(int depth, bool isMaximizing)
{
    int gameResult = CheckForWin(_model.matrixes[0]);
    if (gameResult != 0 || depth == 0)
        return new BestMove { subTrisIndex = -1, cellIndex = -1, score = EvaluateBoard(isMaximizing) };

    int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
    BestMove bestMove = new BestMove { subTrisIndex = -1, cellIndex = -1, score = bestScore };

    for (int subTrisIndex = 1; subTrisIndex <= 9; subTrisIndex++)
    {
        foreach (int cellIndex in GetAvailableCells(subTrisIndex))
        {
            _model.matrixes[subTrisIndex][cellIndex] = 1;
            int score = MiniMaxForPlayer2(depth - 1, !isMaximizing).score;

            if (isMaximizing)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = new BestMove { subTrisIndex = subTrisIndex, cellIndex = cellIndex, score = bestScore };
                }
            }
            else
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = new BestMove { subTrisIndex = subTrisIndex, cellIndex = cellIndex, score = bestScore };
                }
            }

            _model.matrixes[subTrisIndex][cellIndex] = 0;
        }
    }

    return bestMove;
}

    private List<int> GetAvailableCells(int subTrisIndex)
    {
        List<int> availableCells = new List<int>();

        // Verifica solo se il sotto-tris è ancora giocabile
        if (_model.matrixes[0][subTrisIndex - 1] == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_model.matrixes[subTrisIndex][i] == 0)
                    availableCells.Add(i);
            }
        }

        return availableCells;
    }
    private bool IsPlayer1AboutToWin()
    {
        // Controlla se il player 1 ha due caselle occupate in una tris
        for (int i = 0; i < 3; i++)
        {
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i + 3] == 1 && _model.matrixes[0][i + 6] == 0)
                return true;
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i + 1] == 1 && _model.matrixes[0][i + 2] == 0)
                return true;
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i * 3] == 1 && _model.matrixes[0][i * 6] == 0)
                return true;
        }

        return false;
    }

    private int EvaluateBoard(bool isMaximizing)
    {
        int playerValue = isMaximizing ? 2 : 1;  // Assign the value based on the player
        int opponentValue = 3 - playerValue;  // The value for the opponent (1 if playerValue is 2, 2 if playerValue is 1)

        int gameResult = CheckForWin(_model.matrixes[0]);
        if (gameResult == playerValue)
            return 100;
        else if (gameResult == opponentValue)
            return -100;

        // Controlla se il player 1 sta per vincere
        if (IsPlayer1AboutToWin())
            return -10000;

        // Assegna un valore di priorità alle mosse che occupano la casella centrale
        if (_model.matrixes[0][4] == 0)
            return 1000;

        // Assegna un valore di priorità alle mosse che bloccano le tris del player 1
        int priority = 0;
        for (int i = 0; i < 3; i++)
        {
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i + 3] == 1 && _model.matrixes[0][i + 6] == 0)
                priority += 1000;
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i + 1] == 1 && _model.matrixes[0][i + 2] == 0)
                priority += 1000;
            if (_model.matrixes[0][i] == 1 && _model.matrixes[0][i * 3] == 1 && _model.matrixes[0][i * 6] == 0)
                priority += 1000;
        }

        return priority;
    }


    private void PerformComputerMove()
    {
        StartCoroutine(PerformComputerMoveCoroutine());
    }

    private IEnumerator PerformComputerMoveCoroutine()
    {
        bool cameraMoveCompleted = false;
        bool bestMoveCalculated = false;
        BestMove bestMove = new BestMove();
        int playerSymbol = (_player == 1) ? 1 : -1;

        // Avvia l'animazione della telecamera
        Coroutine cameraMoveCoroutine = StartCoroutine(_view.MoveCameraCoroutine(_nextSchema, () => cameraMoveCompleted = true));

        // Calcola la mossa migliore in background
        StartCoroutine(CalculateBestMoveInBackground((result) =>
        {
            bestMove = result;
            bestMoveCalculated = true;
        }));
        // Attendiamo che la telecamera completi il movimento
        yield return new WaitUntil(() => cameraMoveCompleted);

        // Attendiamo che il calcolo della mossa migliore sia completato
        yield return new WaitUntil(() => bestMoveCalculated);

        if (_nextSchema == -1)
        {
            cameraMoveCompleted = false;
            _audioSourceController.PlaySound(_moveSound);
            cameraMoveCoroutine = StartCoroutine(_view.MoveCameraCoroutine(bestMove.subTrisIndex, () => cameraMoveCompleted = true));
            yield return new WaitUntil(() => cameraMoveCompleted);
            yield return new WaitForSecondsRealtime(0.2f);
        }

        // Esegui la mossa migliore calcolata dall'algoritmo MiniMax
        if (bestMove.subTrisIndex != -1 && bestMove.cellIndex != -1)
        {
            Button buttonToPress = _buttons[(bestMove.subTrisIndex) * 9 + bestMove.cellIndex];
            // Esegui la mossa
            _model.matrixes[bestMove.subTrisIndex][bestMove.cellIndex] = playerSymbol;
            Debug.Log("Best move: " + bestMove.subTrisIndex + ", " + bestMove.cellIndex);
            Debug.Log("Button: ", buttonToPress.gameObject);
            _view.SetIcon(_player, buttonToPress, true);
            _audioSourceController.PlaySound(_moveSound);
            int winSituation = CheckForWin(_model.matrixes[bestMove.subTrisIndex]);
            if (winSituation!=0)
            {
                _view.SetIcon(_player, buttonToPress.ParentButton, false);
                _model.matrixes[0][bestMove.subTrisIndex-1] = playerSymbol;
            }
            else if (winSituation == 2)
            {
                _model.matrixes[0][bestMove.subTrisIndex-1] = 2;
            }

            SetAllSubGroupsDisabled();
                
            int mainWinSituation = CheckForWin(_model.matrixes[0]);
            if (mainWinSituation != 0)
            {
                _view.MoveCamera();
                _view.WinScreen(mainWinSituation);
                yield break;
            }
                
            //settare nuovo valore di nextSchema
            _nextSchema = (_model.matrixes[0][((buttonToPress.ParentButton==null)?buttonToPress.identifier+1:(buttonToPress.identifier))] != 0) ? -1 :buttonToPress.identifier+1;
            _view.MoveCamera((_nextSchema == -1) ? 0 : _nextSchema);
            if (_nextSchema > 0)
            {
                _buttons[_nextSchema - 1].GroupBelowMe.alpha = 1f;
            }

            _view.BackButtonEnabled = false;
            
        }

        SwitchPlayer();
    }

    private IEnumerator CalculateBestMoveInBackground(Action<BestMove> onBestMoveCalculated)
    {
        yield return new WaitForSeconds(2f);

        // Esempio di una mossa migliore calcolata
        BestMove bestMove = MiniMaxForPlayer2(5, true);

        onBestMoveCalculated?.Invoke(bestMove);
    }



    public void SetGameMode(int value)
    {
        switch (value)
        {
            case 0:
                _model.GameMode = GameMode.Single;
                break;
            case 1:
                _model.GameMode = GameMode.Local;
                break;
            case 2:
                _model.GameMode = GameMode.Online;
                break;
        }
    }

    public void ResetGameCamera()
    {
        _view.MoveCamera();
        _model.GameMode = GameMode.Uninitialized;
        _player = 1;
        StopAllCoroutines();
    }


}
