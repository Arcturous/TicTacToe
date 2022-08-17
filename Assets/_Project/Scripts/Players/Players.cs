using System.Collections.Generic;

public enum ePlayerSymbol
{
    X,  // 0
    O   // 1
}

[System.Serializable]
public class Player
{
    public ePlayerSymbol PlayerSymbol { get; set; }
    public string UserName;
    protected List<int> m_PreviousMoves = new List<int>();

    public Player(string userName)
    {
        UserName = userName;
    }

    // public void AddMove(int moveIndex)
    // {
    //     m_PreviousMoves.Add(moveIndex);
    // }

    // public void UndoMove(int moveIndex)
    // {
    //     m_PreviousMoves.Remove(moveIndex);
    // }

    // public void ClearMoves()
    // {
    //     m_PreviousMoves.Clear();
    // }

    // public int BestNextMove()
    // {
    //     // TODO calculate best next move using the TicTacToeGrid to find all nearby options to the previous move this player made 
    //     // if no previous moves - just get random spot
    //     return 0;
    // }
}