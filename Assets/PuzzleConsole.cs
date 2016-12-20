using UnityEngine;
using System.Collections;

public class PuzzleConsole : Lookable {

    Transform puzzle;

	// Use this for initialization
	void Start () {
        puzzle = GetComponentInChildren<GridPuzzle>().transform;
        puzzle.localScale = new Vector3(0, 0, 0);
	}

    public override void Activate()
    {
        base.Activate();
        iTween.ScaleTo(puzzle.gameObject, iTween.Hash("time", 1, "scale", new Vector3(1, 1, 1), "easeType", "easeOutBack"));
    }
}
