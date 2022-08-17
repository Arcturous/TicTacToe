using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WinTest
{
    [Test]
    public void TestHorizontalWin()
    {
        var ticTacToeLogic = new TicTacToeLogic();

        int[] grid =
        {
                1, -1, 0,
                1, -1, 0,
                0, 0, 0,
            };

        var isXWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.X);
        var isOWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.O);

        Assert.IsFalse(isOWin);
        Assert.IsTrue(isXWin);
    }

    [Test]
    public void TestVerticalWin()
    {
        var ticTacToeLogic = new TicTacToeLogic();

        int[] grid =
        {
                0, -1, 0,
                0, -1, 0,
                0, -1, 0,
            };

        var isXWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.X);
        var isOWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.O);

        Assert.IsFalse(isOWin);
        Assert.IsTrue(isXWin);
    }

    [Test]
    public void TestDiagonalWin()
    {
        var ticTacToeLogic = new TicTacToeLogic();

        int[] grid =
        {
                0, -1, 1,
                0, 0, -1,
                0, -1, 0,
            };

        var isXWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.X);
        var isOWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.O);

        Assert.IsFalse(isOWin);
        Assert.IsTrue(isXWin);
    }

    [Test]
    public void TestReverseDiagonalWin()
    {
        var ticTacToeLogic = new TicTacToeLogic();

        int[] grid =
        {
                1, -1, 0,
                0, 0, 0,
                0, 0, 0,
            };

        var isXWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.X);
        var isOWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.O);

        Assert.IsFalse(isOWin);
        Assert.IsTrue(isXWin);
    }
}
