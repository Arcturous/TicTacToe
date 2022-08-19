using UnityEngine;

[System.Serializable]
public enum eDifficulty
{
    easy,
    medium,
    hard,
}

public class ComputerPlayer : Player
{
    private eDifficulty m_difficulty;
    private TicTacToeLogic m_logic = new TicTacToeLogic();

    public ComputerPlayer(int index = 0, eDifficulty difficulty = eDifficulty.easy) : base(index == 0 ? "Computer" : $"Computer{index}")
    {
        m_difficulty = difficulty;
    }

    public int NextMoveIndex(TicTacToeGrid grid)
    {
        if (m_difficulty == eDifficulty.medium)
        {
            int rand = Random.Range(1, 101);    // 1 - 100
            if (rand < 31)  // 30% to make random choice
                return grid.RandomEmptyIndex();

            // otherwise pick best next move
            return m_logic.CalculateBestMove(grid.Grid, PlayerSymbol);
        }

        if (m_difficulty == eDifficulty.hard)
        {
            // always pick best next move
            return m_logic.CalculateBestMove(grid.Grid, PlayerSymbol);
        }

        // default normal difficulty - always pick random move 
        return grid.RandomEmptyIndex();
    }
}