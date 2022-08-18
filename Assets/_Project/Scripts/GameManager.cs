using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // TODO init buttons dynamically in code
    [SerializeField] private GridButton[] _buttons = new GridButton[9];
    [SerializeField] private Text _playerText;
    [SerializeField] private GameTimer _timer;
    [SerializeField] private MessageScreen _screen;
    [SerializeField] private Hint _hint;
    [SerializeField] private GameSettings _settings;
    [SerializeField] private SideMenu _sideMenu;
    [SerializeField] private Image _backgroundImage;

    private int _gridDimension = 3;
    private Stack<int> m_undoStack = new Stack<int>();
    private Logger m_logger = new Logger("GameManager");
    private int m_turn = 0;
    private TicTacToeGrid m_grid;
    private Player m_currentPlayer;
    private MoveLogic m_moveLogic = new MoveLogic();
    private List<Player> m_players = new List<Player>();
    private Coroutine m_playPCTurnRoutine;
    private int turn
    {
        get { return m_turn; }
        set
        {
            m_turn = value;
            m_currentPlayer = m_turn % 2 == 0 ? m_players.Find((p) => p.PlayerSymbol == ePlayerSymbol.X) : m_players.Find((p) => p.PlayerSymbol == ePlayerSymbol.O);
            if (_playerText)
                _playerText.text = m_currentPlayer.UserName;
        }
    }

    private bool isCurrentPlayerPC
    {
        get { return CurrentPlayer.UserName.Contains("Computer"); }
    }

    private bool areAllPlayersPC
    {
        get { return m_players.TrueForAll((p) => p.UserName.Contains("Computer")); }
    }

    private void Start()
    {
        _backgroundImage.sprite = Sprite.Create(_settings.textureBG, new Rect(0, 0, _settings.textureBG.width, _settings.textureBG.height), new Vector2(0, 0));

        m_grid = new TicTacToeGrid(_gridDimension);
        // setup the onclick action for all buttons in the grid
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].SetOnClick(OnClickGridButton, i);
        }

        // listen to timer end event, and end the game with the other player being the winner
        // TODO the "num" param is useless, but my custom events require a param, need to make it optional in future
        _timer.eOnTimerEnd.On((num) =>
        {
            turn++;
            if (isCurrentPlayerPC && !areAllPlayersPC)
            {
                EndGame(eGameMessage.Lose);
                return;
            }
            EndGame(eGameMessage.Win);
        });

        SetupGameBySettings();
        Reset();
    }

    private void SetupGameBySettings()
    {
        if (!_settings) // default go PvPC
        {
            m_players.Add(new Player("Player"));
            m_players.Add(new ComputerPlayer(0));
            return;
        }

        _gridDimension = _settings.gridDimension;
        if (_gridDimension < 3)
        {
            LockAllButtons();
            _screen.ShowError("Grid dimension too low - Minimum 3");
        }

        switch (_settings.mode)
        {
            case eGameMode.PvP:
                m_players.Add(new Player("Player1"));
                m_players.Add(new Player("Player2"));
                break;
            case eGameMode.PCvPC:
                m_players.Add(new ComputerPlayer(1, _settings.difficulty));
                m_players.Add(new ComputerPlayer(2, _settings.difficulty));
                break;
            default:  // eGameMode.PvPC
                m_players.Add(new Player("Player"));
                m_players.Add(new ComputerPlayer(0, _settings.difficulty));
                break;
        }
    }

    private Player CurrentPlayer
    {
        get { return m_currentPlayer; }
    }

    public void OnClickHint()
    {
        int emptyIndex = m_moveLogic.CalculateBestMove(m_grid.Grid, CurrentPlayer.PlayerSymbol);
        Vector2 buttonPos = _buttons[emptyIndex].transform.position;
        _hint.Show(new Vector2(buttonPos.x, buttonPos.y + 0.5f));
    }

    public void OnClickGridButton(int buttonIndex)
    {
        SetButtonToPlayer(buttonIndex);

        m_undoStack.Push(buttonIndex);
        _hint.Hide();

        if (CheckWin())
        {
            if (isCurrentPlayerPC && !areAllPlayersPC)
            {
                EndGame(eGameMessage.Lose);
                return;
            }
            EndGame(eGameMessage.Win);
            return;
        }

        turn++;

        if (turn >= _buttons.Length)
        {
            EndGame(eGameMessage.Draw);
            return;
        }

        _timer.Reset();
        _timer.TurnOn();

        if (isCurrentPlayerPC)
        {
            ActivatePcTurn();
        }
    }

    private void AssignPlayerSymbols()
    {
        Array values = Enum.GetValues(typeof(ePlayerSymbol));
        System.Random random = new System.Random();
        ePlayerSymbol randomPlayerSymbol = (ePlayerSymbol)values.GetValue(random.Next(values.Length));

        m_players[0].PlayerSymbol = randomPlayerSymbol;

        // TODO make this scalable with more players in future feature? the 1- solution is only fitting for 2 players..
        m_players[1].PlayerSymbol = 1 - randomPlayerSymbol;
    }

    private void SetButtonToPlayer(int buttonIndex)
    {
        m_logger.Log("setting " + buttonIndex + " to symbol " + CurrentPlayer.PlayerSymbol);

        m_grid.MarkGrid(buttonIndex, CurrentPlayer.PlayerSymbol);

        _buttons[buttonIndex]?.SetTexture(CurrentPlayer.PlayerSymbol == ePlayerSymbol.X ? _settings.textureX : _settings.textureO);
    }

    private bool CheckWin()
    {
        // cannot win before having a player with at least {_gridDimension} marks on the grid

        // 2+1 = 3  --->  X O X (turn == 2)
        // 3+1 = 4  --->  X O X O X (turn == 4)
        // 4+1 = 5  --->  X O X O X O X (turn == 6)
        // 5+1 = 6  --->  X O X O X O X O X (turn == 8)
        // 6+1 = 7  --->  X O X O X O X O X O X (turn == 10)
        // 20+1 = 21  --->  X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X (turn == 38)

        // _gridDimension == 2 -> if(turn < 2)
        // _gridDimension == 3 -> if(turn < 4)
        // _gridDimension == 4 -> if(turn < 6)
        // _gridDimension == 5 -> if(turn < 8)
        // _gridDimension == 6 -> if(turn < 10)
        // _gridDimension == 20 -> if(turn < 38)

        if (turn < TurnToStartCheckingWin) return false;
        m_logger.Log($"Reached Enough turns to start checking win. GridDimension - {_gridDimension}, Turn - {turn}", "CheckWin");

        return m_grid.IsWin(CurrentPlayer.PlayerSymbol);
    }

    public int TurnToStartCheckingWin
    {
        get { return _gridDimension + 1 + (_gridDimension - 3); }
    }

    public void Reset()
    {
        if (m_playPCTurnRoutine != null)
        {
            StopCoroutine(m_playPCTurnRoutine);
        }

        AssignPlayerSymbols();

        turn = 0;

        m_undoStack.Clear();

        m_grid.Reset();
        _hint.Hide();

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Reset();
        }

        UnlockAllButtons();

        if (areAllPlayersPC)
            LockGridButtons();

        _timer.Reset();
        _timer.TurnOn();

        if (isCurrentPlayerPC)
        {
            ActivatePcTurn();
        }
    }

    public void Undo()
    {
        if (m_undoStack == null || m_undoStack.Count == 0) return;

        // right now we only have 1 pc player max, but in the future maybe more
        int computerPlayerCount = m_players.FindAll((p) => p.UserName.Contains("Computer")).Count;

        if (computerPlayerCount == 0) return;   // will disable "undo" button in UI, but want to make sure players can't use it so disable here as well

        // computerPlayerCount+1 will make sure it does at least one undo (if we want to add this feature to PvP in the future
        int undoAmount = areAllPlayersPC ? computerPlayerCount : computerPlayerCount + 1;

        for (int i = 0; i < undoAmount; i++)
        {
            if (m_undoStack.Count == 0) return;

            // if PC had the first move by being the X, don't undo his move
            Player pcPlayer = m_players.Find((p) => p.UserName.Contains("Computer"));    // can improve this by saving the pc player index and then just getting him instead of using Find each time..
            if (!areAllPlayersPC && m_undoStack.Count == 1 && pcPlayer != null && pcPlayer.PlayerSymbol == ePlayerSymbol.X) return;

            int gridIndex = m_undoStack.Pop();

            // cancel last action
            _buttons[gridIndex].Reset();
            m_grid.RemoveMark(gridIndex);
            turn--;
        }
    }

    private void EndGame(eGameMessage message)
    {
        LockAllButtons();
        // TODO lock side buttons
        _timer.TurnOff();
        _screen.ShowMessage(message, CurrentPlayer);
    }

    private void ActivatePcTurn()
    {
        // TODO lock all buttons
        LockGridButtons();

        if (m_playPCTurnRoutine != null)
        {
            StopCoroutine(m_playPCTurnRoutine);
        }
        m_playPCTurnRoutine = StartCoroutine(WaitAndPerformPCTurn());
    }

    private IEnumerator WaitAndPerformPCTurn()
    {
        yield return new WaitForSeconds(1);
        ComputerPlayer pcPlayer = CurrentPlayer as ComputerPlayer;
        if (pcPlayer != null)
        {
            int emptyIndex = pcPlayer.NextMoveIndex(m_grid);
            OnClickGridButton(emptyIndex);
        }

        // TODO release all buttons
        UnlockAllButtons();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void LockAllButtons()
    {
        _sideMenu?.DisableButtons();

        LockGridButtons();
    }

    private void LockGridButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].LockButton();
        }
    }

    private void UnlockAllButtons()
    {
        _sideMenu?.EnableButtons();

        if (areAllPlayersPC) return;
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].UnlockButton();
        }
    }
}
