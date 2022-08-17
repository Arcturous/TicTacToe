using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LoseTest
{
    [Test]
    public void TestLose()
    {
        var ticTacToeLogic = new TicTacToeLogic();
        Player player = new Player("Player1") { PlayerSymbol = ePlayerSymbol.X };
        Player otherPlayer = new Player("Player2") { PlayerSymbol = ePlayerSymbol.O };

        int[] grid =
        {
            1, 0, 1,
            1, 0, 0,
            1, -1, 0,
        };

        var isXWin = ticTacToeLogic.IsWin(grid, 3, player.PlayerSymbol);
        var isOWin = ticTacToeLogic.IsWin(grid, 3, otherPlayer.PlayerSymbol);

        Assert.IsFalse(isXWin);
        Assert.IsTrue(isOWin);
    }
}
