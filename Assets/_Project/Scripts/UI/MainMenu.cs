using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameSettings _settings;

    [SerializeField] GameObject _modeButtons;
    [SerializeField] GameObject _difficultyButtons;

    private Vector2 m_OriginalBtnPosition;
    private Logger m_logger = new Logger("MainMenu");

    void Start()
    {
        m_OriginalBtnPosition = _modeButtons.transform.position;

        // reset settings to default
        _settings.mode = eGameMode.PvPC;
        _settings.difficulty = eDifficulty.easy;
        _settings.GridDimension = 3;
    }

    public void SetGameMode(int mode)
    {
        _settings.mode = (eGameMode)mode;

        m_logger.Log($"new mode {_settings.mode}");

        if (_settings.mode == eGameMode.PvP)
        {
            _modeButtons.transform.position = new Vector2(m_OriginalBtnPosition.x, m_OriginalBtnPosition.y - 0.3f);
            _difficultyButtons.SetActive(false);
        }
        else
        {
            _modeButtons.transform.position = m_OriginalBtnPosition;
            _difficultyButtons.SetActive(true);
        }
    }

    public void SetDifficulty(int difficulty)
    {
        _settings.difficulty = (eDifficulty)difficulty;
        m_logger.Log($"new difficulty {_settings.difficulty}");
    }

    public void SetGridDimension(int dimension)
    {
        _settings.GridDimension = dimension;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Reskin()
    {

    }
}
