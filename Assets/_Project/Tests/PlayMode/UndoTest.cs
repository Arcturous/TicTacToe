using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class UndoTest
{
    [Test]
    public void UndoTestSimple()
    {
        // Use the Assert class to test conditions

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
}

