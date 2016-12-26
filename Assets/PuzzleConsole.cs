using UnityEngine;
using System.Collections;

public class PuzzleConsole : Lookable {

    GridPuzzle puzzle;
    Transform player;
    bool activated = false;
    public int[] scores;
    bool firstUpdate = true;

	// Use this for initialization
	void Start () {
        puzzle = GetComponentInChildren<GridPuzzle>();
        puzzle.console = this;
        scores = new int[puzzle.numTypes];
        player = GameObject.FindGameObjectWithTag("Player").transform;
        activated = false;
    }

    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            puzzle.transform.localScale = new Vector3(0, 0, 0);
        }

        if (activated)
        {
            if ((transform.position - player.position).magnitude > 3.0f)
            {
                iTween.ScaleTo(puzzle.gameObject, iTween.Hash("time", 1, "scale", new Vector3(0, 0, 0), "easeType", "easeOutBack"));
                activated = false;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        iTween.ScaleTo(puzzle.gameObject, iTween.Hash("time", 1, "scale", new Vector3(1, 1, 1), "easeType", "easeOutBack"));
        activated = true;
    }
}
