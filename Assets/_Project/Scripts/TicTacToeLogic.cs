using UnityEngine;

public class TicTacToeLogic
{
    public bool IsWin(int[] grid, int gridDimension, ePlayerSymbol playerSymbol)
    {
        int matchesNumberHorizontal = 0;
        int matchesNumberVertical = 0;
        int matchesNumberDiagonal = 0;
        int matchesNumberReverseDiagonal = 0;

        //  check horizontal and vertical lines
        for (int i = 0; i < gridDimension; i++)
        {
            matchesNumberHorizontal = 0;
            matchesNumberVertical = 0;
            for (int j = 0; j < gridDimension; j++)
            {
                // 0 1 2
                // 3 4 5
                // 6 7 8
                if (grid[i * gridDimension + j] == (int)playerSymbol)
                {
                    matchesNumberHorizontal++;
                }

                // 0 3 6
                // 1 4 7
                // 2 5 8
                if (grid[j * gridDimension + i] == (int)playerSymbol)
                {
                    matchesNumberVertical++;
                }

                //  player won
                if (matchesNumberHorizontal == gridDimension || matchesNumberVertical == gridDimension)
                {
                    return true;
                }
            }
        }

        int diagonalIndex = 0;

        //  check diagonal line
        for (int i = 0; i < gridDimension; i++)
        {
            if (i == 0)
            {
                diagonalIndex = 0;
            }
            else
            {
                diagonalIndex = i * gridDimension + i;
            }

            if (grid[diagonalIndex] == (int)playerSymbol)
            {
                matchesNumberDiagonal++;
            }

            //  player won
            if (matchesNumberDiagonal == gridDimension)
            {
                return true;
            }
        }

        //  check reverse diagonal line
        for (int i = gridDimension - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                diagonalIndex = gridDimension - 1;
            }
            else
            {
                diagonalIndex = i * gridDimension + gridDimension - i - 1;
            }

            if (grid[diagonalIndex] == (int)playerSymbol)
            {
                matchesNumberReverseDiagonal++;
            }

            //  player won
            if (matchesNumberReverseDiagonal == gridDimension)
            {
                return true;
            }
        }

        return false;
    }
}