using UnityEngine;
using UnityEngine.UI;

public enum eGameMessage
{
    Win,
    Lose,
    Draw,
}

public class MessageScreen : MonoBehaviour
{
    [SerializeField] private Text _messageText;

    private Logger m_Logger = new Logger("MessageScreen");

    public void ShowMessage(eGameMessage message, Player player)
    {
        // TODO animate
        gameObject.SetActive(true);

        string messageToShow = "";

        switch (message)
        {
            case eGameMessage.Win:
                messageToShow = Texts.PLAYER_WIN.Replace("{0}", player.UserName);
                break;
            case eGameMessage.Lose: // Not currently in use, but could be for real multiplayer
                messageToShow = Texts.BETTER_LUCK;
                break;
            case eGameMessage.Draw:
                messageToShow = Texts.DRAW;
                break;
        }

        _messageText.text = messageToShow;

        m_Logger.Log($"Showing message: {messageToShow}");
    }

    public void ShowError(string errorMessage)
    {
        gameObject.SetActive(true);
        _messageText.text = errorMessage;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        // TODO animate
    }
}
