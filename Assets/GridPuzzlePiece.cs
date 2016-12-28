using UnityEngine;
using System.Collections;

public class GridPuzzlePiece : Lookable {

    [HideInInspector]
    public GridPuzzle puzzle;

    public int index; // which piece this one is, eg what colour or shape
    public int x0;
    public int y0;
    public int pieceListIndex;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Sparkle()
    {
        GetComponent<ParticleSystem>().Play();
    }

    public override void Activate()
    {
        base.Activate();
        puzzle.ClickOnPiece(this);
    }

    public void ScaleTo(float scale, float delay = 0, float dur = 1)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("time", dur, "scale", new Vector3(scale, scale, scale), "easeType", "easeOutBack", "delay", delay));
    }

    public void MoveTo(Vector3 position, float delay = 0, float dur = 1)
    {
        iTween.MoveTo(gameObject, iTween.Hash("time", dur, "position", position, "easeType", "easeOutBack", "delay", delay));
    }
}
