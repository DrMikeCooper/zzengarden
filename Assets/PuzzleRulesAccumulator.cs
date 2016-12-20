using UnityEngine;
using System.Collections;

public class PuzzleRulesAccumulator : PuzzleRules {

    public override void PostMove(GridPuzzlePiece a, GridPuzzlePiece b)
    {
        puzzle.RemoveBlocks(4);
    }
}
