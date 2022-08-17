using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DrawTest
{
    [Test]
    public void TestDraw()
    {
        var ticTacToeLogic = new TicTacToeLogic();

        int[] grid =
        {
                1, 0, 1,
                1, 0, 0,
                0, 1, 1,
            };

        var isOneWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.O);
        var isZeroWin = ticTacToeLogic.IsWin(grid, 3, ePlayerSymbol.X);

        Assert.IsFalse(isOneWin);
        Assert.IsFalse(isZeroWin);
    }
}
