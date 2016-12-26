using UnityEngine;
using System.Collections;

public class PuzzleRulesAccumulator : PuzzleRules {

    public override void PostMove()
    {
        puzzle.RemoveBlocks(4);
    }
}
