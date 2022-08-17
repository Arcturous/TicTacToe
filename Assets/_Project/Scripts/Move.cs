using System;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public int index;
    public int score; // weight

    public Move(int index, int score)
    {
        this.index = index;
        this.score = score;
    }
}

public class MoveLogic
{
    private TicTacToeLogic m_TicTacToeLogic = new TicTacToeLogic();

    private ePlayerSymbol m_currentPlayerSymbol;
    private ePlayerSymbol m_currentOpponentSymbol;

    private Dictionary<string, Move> checkedMoves = new Dictionary<string, Move>();

    public int CalculateBestMove(int[] grid, ePlayerSymbol symbol)
    {
        checkedMoves.Clear();

        m_currentPlayerSymbol = symbol;
        m_currentOpponentSymbol = 1 - symbol;   // this only applies because we know theres only 2 symbols "X" and "O", need to change if we want to add more symbols in the future

        if (Array.TrueForAll(grid, element => element == -1))   // if the grid is empty
        {
            return UnityEngine.Random.Range(0, grid.Length);    // return a random spot on the grid
        }

        int gridDimension = (int)Math.Sqrt(grid.Length);
        return MiniMax(grid, gridDimension, symbol).index;
    }

    private int[] GetEmptyIndices(int[] grid)
    {
        int[] indiceArray = new int[grid.Length];

        // fill it with the indices
        for (int i = 0; i < indiceArray.Length; i++)
        {
            indiceArray[i] = i;
        }

        // filter which indices are empty
        int[] emptyindices = Array.FindAll(indiceArray, (index) => grid[index] == -1);

        return emptyindices;
    }

    private Move MiniMax(int[] newGrid, int gridDimension, ePlayerSymbol symbol)
    {
        if (m_TicTacToeLogic.IsWin(newGrid, gridDimension, m_currentOpponentSymbol)) // other player won
        {
            return new Move() { score = -1000 };
        }

        if (m_TicTacToeLogic.IsWin(newGrid, gridDimension, m_currentPlayerSymbol)) // this player won
        {
            return new Move() { score = 10 };
        }

        int[] emptyindices = GetEmptyIndices(newGrid);
        if (emptyindices.Length == 0)
        {
            return new Move() { score = 0 };
        }

        // a list to collect all the possible moves for this grid
        List<Move> movesList = new List<Move>();

        // loop through available spots
        for (int i = 0; i < emptyindices.Length; i++)
        {
            //create an object for each and store the index of that spot 
            Move move = new Move() { index = emptyindices[i] };

            // set the empty spot to the player symbol
            newGrid[emptyindices[i]] = (int)symbol;

            // calculate the score from the other players move - save the result to dictionary so we can save iterations, thus lowering runtime of the calculation for large/empty grids
            Move result;
            if (checkedMoves.ContainsKey((string.Join(", ", newGrid) + $", {(1 - symbol)}")))
            {
                result = checkedMoves[(string.Join(", ", newGrid) + $", {(1 - symbol)}")];
            }
            else    // key doesn't exist
            {
                result = MiniMax(newGrid, gridDimension, 1 - symbol);
                checkedMoves.Add((string.Join(", ", newGrid) + $", {(1 - symbol)}"), result);
            }

            move.score = result.score - 50; // substract score for any move that doesn't lead to a win - thus making us search for the shortest route to win

            // reset the spot to empty
            newGrid[emptyindices[i]] = -1;

            // push the object to the list
            movesList.Add(move);
        }

        // Debug.Log($"moves list {string.Join(", ", movesList.ConvertAll((m) => (m.index, m.score)))}");

        Move bestMove = new Move();
        if (m_currentPlayerSymbol == symbol) // if it is the computer's turn loop over the moves and choose the move with the highest score
        {
            bestMove.score = int.MinValue;   // start with low number and go up to best score possible

            for (int i = 0; i < movesList.Count; i++)
            {
                if (movesList[i].score > bestMove.score)
                {
                    bestMove = movesList[i];
                }
            }
        }
        else // else (opponent turn) loop over the moves and choose the move with the lowest score
        {
            bestMove.score = int.MaxValue; // start with high number and go to lowest score possible
            for (int i = 0; i < movesList.Count; i++)
            {
                if (movesList[i].score < bestMove.score)
                {
                    bestMove = movesList[i];
                }
            }
        }

        // return the chosen move (object) from the moves array
        return bestMove;
    }
}