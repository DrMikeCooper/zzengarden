using UnityEngine;
using System.Collections;

public class PuzzleConsole : Lookable {

    GridPuzzle puzzle;
    Transform player;
    bool activated = false;
    bool firstUpdate = true;
    public int[] scores;
    public TextMesh[] scoreHUDs;

	// Use this for initialization
	void Start () {
        puzzle = GetComponentInChildren<GridPuzzle>();
        puzzle.console = this;
        scores = new int[puzzle.numTypes];
        scoreHUDs = new TextMesh[puzzle.numTypes];
        player = GameObject.FindGameObjectWithTag("Player").transform;
        activated = false;

        for (int i = 0; i < puzzle.numTypes; i++)
        {
            Transform obj = transform.FindChild("Score" + (1 + i));
            if (obj)
            {
                scoreHUDs[i] = obj.GetComponent<TextMesh>();
                if (scoreHUDs[i])
                    scoreHUDs[i].color = puzzle.colors[i];
            }
        };
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

        for (int i = 0; i < puzzle.numTypes; i++)
        {
            if (scoreHUDs[i])
                scoreHUDs[i].text = scores[i].ToString("000");
        }
    }

    public override void Activate()
    {
        base.Activate();
        iTween.ScaleTo(puzzle.gameObject, iTween.Hash("time", 1, "scale", new Vector3(1, 1, 1), "easeType", "easeOutBack"));
        activated = true;
    }
}
