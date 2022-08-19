using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// This test is on play mode because we are using a monobehaviour class (GameManager), and it needs to have its "Awake" function called
public class UndoTest
{
    [Test]
    public void UndoTest4Items()     // 4 items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;

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
        int stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndices[i]);
        }

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,0,-1,
            -1,1,-1,
            -1,-1,-1
        };

        for (int i = 0; i < testGridAfterUndo.Length; i++)
        {
            Assert.AreEqual(testGridAfterUndo[i], grid.Grid[i]);
        }

        testStack = new Stack<int>(manager.UndoStack);
        stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndicesAfterUndo[i]);
        }
    }

    [Test]
    public void UndoTest3Items()     // 3 items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;

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
        int stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndices[i]);
        }

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,0,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        for (int i = 0; i < testGridAfterUndo.Length; i++)
        {
            Assert.AreEqual(testGridAfterUndo[i], grid.Grid[i]);
        }

        testStack = new Stack<int>(manager.UndoStack);
        stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndicesAfterUndo[i]);
        }
    }

    [Test]
    public void UndoTest1Item()     // 1 item
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;

        int[] clickedIndices = new int[] { 1 };

        manager.OnClickGridButton(clickedIndices[0]);   // X

        int[] testGrid = {
            -1,0,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        Stack<int> testStack = new Stack<int>(manager.UndoStack);
        int stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndices[i]);
        }

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        // Call Undo
        manager.Undo();

        int[] testGridAfterUndo = {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        for (int i = 0; i < testGridAfterUndo.Length; i++)
        {
            Assert.AreEqual(testGridAfterUndo[i], grid.Grid[i]);
        }

        Assert.AreEqual(manager.UndoStack.Count, 0);
    }

    [Test]
    public void UndoTestNoItems()     // no items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;

        int[] testGrid = {
            -1,-1,-1,
            -1,-1,-1,
            -1,-1,-1
        };

        Assert.AreEqual(manager.UndoStack.Count, 0);

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        // Call Undo
        manager.Undo();

        Assert.AreEqual(manager.UndoStack.Count, 0);

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }
    }

    [Test]
    public void UndoTestNoPcPlayers()     // no items
    {
        GameObject obj = new GameObject();
        GameManager manager = obj.AddComponent<GameManager>();
        TicTacToeGrid grid = manager.Grid;

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
        int stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndices[i]);
        }

        for (int i = 0; i < testGrid.Length; i++)
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        // Call Undo - it should return before doing anything as there are no pc players
        manager.Undo();

        for (int i = 0; i < testGrid.Length; i++)   // should remain the same as before the "Undo"
        {
            Assert.AreEqual(testGrid[i], grid.Grid[i]);
        }

        testStack = new Stack<int>(manager.UndoStack);
        stackSize = testStack.Count;

        for (int i = 0; i < stackSize; i++)
        {
            int undoIndex = testStack.Pop();
            Assert.AreEqual(undoIndex, clickedIndices[i]);    // should remain the same as before the "Undo"
        }
    }
}

