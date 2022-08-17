using System;

public class TicTacToeGrid
{
    private int[] m_grid;
    private int m_gridDimension = 3;
    private Logger m_Logger = new Logger("TicTacToeGrid");
    private TicTacToeLogic m_logic = new TicTacToeLogic();

    public int[] Grid
    {
        get { return m_grid; }
    }

    public TicTacToeGrid(int gridDimension)
    {
        m_gridDimension = gridDimension;
        m_grid = new int[gridDimension * gridDimension];
    }

    private int[] GetEmptyIndices()
    {
        int[] indiceArray = new int[m_grid.Length];

        // fill it with the indices
        for (int i = 0; i < indiceArray.Length; i++)
        {
            indiceArray[i] = i;
        }

        // filter which indices are empty
        int[] emptyindices = Array.FindAll(indiceArray, (index) => m_grid[index] == -1);

        return emptyindices;
    }

    public void MarkGrid(int index, ePlayerSymbol playerSymbol)
    {
        m_grid[index] = (int)playerSymbol;
    }

    public void RemoveMark(int index)
    {
        m_grid[index] = -1;
    }

    public void Reset()
    {
        for (int i = 0; i < m_grid.Length; i++)
        {
            RemoveMark(i);
        }
    }

    public int RandomEmptyIndex()
    {
        int[] emptyIndices = GetEmptyIndices();

        int index = UnityEngine.Random.Range(0, emptyIndices.Length);
        return emptyIndices[index];
    }

    public bool IsWin(ePlayerSymbol symbol)
    {
        return m_logic.IsWin(m_grid, m_gridDimension, symbol);
    }
}