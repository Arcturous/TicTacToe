using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HintTest
{
    [Test]
    public void TestBestHintPlacementIndex()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            1, -1, 0,
            1, -1, -1,
            -1, 0, -1,
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.AreEqual(indexToPlaceHint, 6);
    }

    [Test]
    public void TestBestHintPlacementIndex2()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            1, -1, 0,   // O _ X
            1, -1, 0,   // O _ X
            0, 0, -1,   // X X _
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.AreEqual(indexToPlaceHint, 4);
    }

    [Test]
    public void TestBestHintPlacementIndex3()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            1, -1, -1,
            1, -1, -1,
            -1, 0, 0,
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.AreEqual(indexToPlaceHint, 6);
    }

    [Test]
    public void TestBestHintPlacementIndex4()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            1, 1, -1,
            -1, 0, -1,
            -1, -1, 0,
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.AreEqual(indexToPlaceHint, 2);
    }

    [Test]
    public void TestBestHintPlacementIndex5()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            1, -1, 0,   // O _ X
            1, -1, 0,   // O _ X
            -1, 0, -1,  // _ X _
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.AreEqual(indexToPlaceHint, 8);
    }

    [Test]
    public void TestBestHintPlacementOnEmptyGrid()
    {
        MoveLogic logic = new MoveLogic();

        int[] grid =
        {
            -1, -1, -1,
            -1, -1, -1,
            -1, -1, -1,
        };

        int indexToPlaceHint = logic.CalculateBestMove(grid, ePlayerSymbol.X);

        Assert.AreEqual(grid[indexToPlaceHint], -1);
        Assert.IsTrue(indexToPlaceHint < grid.Length && indexToPlaceHint >= 0);
        // Assert.AreEqual(indexToPlaceHint, 0);    // i changed the algo to return a random index when the grid is empty, so can't really test it
    }
}
