using UnityEngine;
using System.Collections;

public class PuzzleRulesRows : PuzzleRules {

    public int criticalMass = 4;

    public override void PostMove()
    {
        puzzle.RemoveRows(criticalMass);
    }
}
