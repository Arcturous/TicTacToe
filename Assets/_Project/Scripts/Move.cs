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