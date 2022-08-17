using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    [SerializeField] private Button _undoBtn;
    [SerializeField] private Button _mainMenuBtn;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _hintBtn;
    [SerializeField] private GameSettings _settings;

    // TODO enable/disable buttons according to game mode
    // Place buttons with padding according to screen size
    void Start()
    {
        // this will disable the hint and undo btns if its pvp mode
        DisableButtons();
        EnableButtons();
    }

    public void DisableButtons()
    {
        _restartBtn.interactable = false;
        _mainMenuBtn.interactable = false;
        _hintBtn.interactable = false;
        _undoBtn.interactable = false;
    }

    public void EnableButtons()
    {
        _restartBtn.interactable = true;
        _mainMenuBtn.interactable = true;
        if (_settings?.mode == eGameMode.PvP) return;
        _hintBtn.interactable = true;
        _undoBtn.interactable = true;
    }
}