﻿using UnityEngine;
using System.Collections;

public class PuzzleRulesAccumulator : PuzzleRules {

    public int criticalMass = 4;

    public override void PostMove()
    {
        puzzle.RemoveBlocks(criticalMass);
    }
}
