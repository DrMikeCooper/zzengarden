using UnityEngine;
using System.Collections;

public class GridPuzzlePiece : Lookable {

    [HideInInspector]
    public GridPuzzle puzzle;

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
}
