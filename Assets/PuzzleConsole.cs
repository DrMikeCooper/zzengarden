using UnityEngine;
using System.Collections;

public class PuzzleConsole : Lookable {

    Transform puzzle;
    Transform player;
    bool activated = false;

	// Use this for initialization
	void Start () {
        puzzle = GetComponentInChildren<GridPuzzle>().transform;
        puzzle.localScale = new Vector3(0, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        activated = false;
    }

    void Update()
    {
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
