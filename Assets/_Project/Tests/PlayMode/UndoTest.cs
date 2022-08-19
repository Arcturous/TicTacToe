using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// This test is on play mode because we are using a monobehaviour class (GameManager), and it needs to have its "Awake" function called
public class UndoTest
{
    private void CompareGrids(int[] grid1, int[] grid2)
    {
        for (int i = 0; i < grid1.Length; i++)
        {
            Assert.AreEqual(grid1[i], grid2[i]);
        }
    }

    private void CompareStackToClicks(Stack<int> stack, int[] clicks)
    {
        int stackSize = stack.Count;
        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = stack.Pop();
            Assert.AreEqual(undoIndex, clicks[i]);
        }
    }

    [Test]
    public void UndoTest4Items()     // 4 items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new ComputerPlayer(0) { PlayerSymbol = ePlayerSymbol.O });

        int[] clickedIndices = new int[] { 1, 4, 7, 8 };
        int[] clickedIndicesAfterUndo = new int[] { 1, 4 };

        // these should remain after undo
        manager.OnClickGridButton(clickedIndices[0]);   // X
        manager.OnClickGridButton(clickedIndices[1]);   // O

        manager.OnClickGridButton(clickedIndices[2]);   // X
        manager.OnClickGridButton(clickedIndices[3]);   // O

        int[] testGrid = {
            -1,0,-1,
            -1,1,-1,
            -1,0,1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,0,-1,
            -1,1,-1,
            -1,-1,-1
        };

        CompareGrids(testGridAfterUndo, grid.Grid);

        testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndicesAfterUndo);
    }

    [Test]
    public void UndoTest3Items()     // 3 items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new ComputerPlayer(0) { PlayerSymbol = ePlayerSymbol.O });

        int[] clickedIndices = new int[] { 1, 4, 7 };
        int[] clickedIndicesAfterUndo = new int[] { 1 };

        // these should remain after undo
        manager.OnClickGridButton(clickedIndices[0]);   // X

        manager.OnClickGridButton(clickedIndices[1]);   // O
        manager.OnClickGridButton(clickedIndices[2]);   // X

        int[] testGrid = {
            -1,0,-1,
            -1,1,-1,
            -1,0,-1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,0,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        CompareGrids(testGridAfterUndo, grid.Grid);

        testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndicesAfterUndo);
    }

    [Test]
    public void UndoTest2Items()     // 2 items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new ComputerPlayer(0) { PlayerSymbol = ePlayerSymbol.O });


        int[] clickedIndices = new int[] { 1, 6 };

        manager.OnClickGridButton(clickedIndices[0]);   // X
        manager.OnClickGridButton(clickedIndices[1]);   // O

        int[] testGrid = {
            -1,0,-1,
            -1,-1,-1,
            1,-1,-1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        CompareGrids(testGridAfterUndo, grid.Grid);

        Assert.AreEqual(manager.UndoStack.Count, 0);
    }

    [Test]
    public void UndoTest1Item()     // 1 item
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new ComputerPlayer(0) { PlayerSymbol = ePlayerSymbol.O });

        int[] clickedIndices = new int[] { 1 };

        manager.OnClickGridButton(clickedIndices[0]);   // X

        int[] testGrid = {
            -1,0,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        CompareGrids(testGridAfterUndo, grid.Grid);

        Assert.AreEqual(manager.UndoStack.Count, 0);
    }

    [Test]
    public void UndoTestNoItems()     // no items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new ComputerPlayer(0) { PlayerSymbol = ePlayerSymbol.O });

        int[] testGrid = {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        Assert.AreEqual(manager.UndoStack.Count, 0);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo
        manager.Undo();

        Assert.AreEqual(manager.UndoStack.Count, 0);

        CompareGrids(testGrid, grid.Grid);
    }

    [Test]
    public void UndoTestNoPcPlayers()     // no pc players
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;
        grid.Reset(); // if pc player has first turn, he will fill a random spot which would ruin these tests

        manager.Players.Clear();    // create 2 non-pc players
        manager.Players.Add(new Player("Player1") { PlayerSymbol = ePlayerSymbol.X });
        manager.Players.Add(new Player("Player2") { PlayerSymbol = ePlayerSymbol.O });

        int[] clickedIndices = new int[] { 1, 4, 7, 8 };

        // these ALL should remain after undo
        manager.OnClickGridButton(clickedIndices[0]);   // X
        manager.OnClickGridButton(clickedIndices[1]);   // O
        manager.OnClickGridButton(clickedIndices[2]);   // X
        manager.OnClickGridButton(clickedIndices[3]);   // O

        int[] testGrid = {
            -1,0,-1,
            -1,1,-1,
            -1,0,1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);

        CompareGrids(testGrid, grid.Grid);

        // Call Undo - it should return before doing anything as there are no pc players
        manager.Undo();

        testStack = new Stack<int>(manager.UndoStack);
        CompareStackToClicks(testStack, clickedIndices);    // should remain the same as before the "Undo"

        CompareGrids(testGrid, grid.Grid);    // should remain the same as before the "Undo"
    }
}

