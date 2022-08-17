using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text _TimerText;
    [SerializeField] private int _PlayerTurnTime = 5;

    public readonly GameEvent<int> eOnTimerEnd = new GameEvent<int>();

    private int m_turnTimeLeft = 5;
    private Coroutine m_RunningTimer;

    public int TurnTimeLeft
    {
        get { return m_turnTimeLeft; }
    }

    public void TurnOn()
    {
        TurnOff();
        UpdateText();
        m_RunningTimer = StartCoroutine(CountTime());
    }

    public void Reset()
    {
        TurnOff();
        m_turnTimeLeft = _PlayerTurnTime;
        UpdateText();
    }

    public void TurnOff()
    {
        if (m_RunningTimer != null)
            StopCoroutine(m_RunningTimer);
    }

    private IEnumerator CountTime()
    {
        while (m_turnTimeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            m_turnTimeLeft--;
            UpdateText();
        }

        // my custom events require a parameter, this number is meaningless
        eOnTimerEnd.Trigger(-1);
    }

    private void UpdateText()
    {
        // TODO get text from xml/json for translation by langCode
        _TimerText.text = "Time Left: " + m_turnTimeLeft;
        if (m_turnTimeLeft <= 0)
        {
            _TimerText.text = "Time's Up!";
        }
    }
}