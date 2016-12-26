using UnityEngine;
using System.Collections;

public class PuzzleRules : MonoBehaviour {

    protected GridPuzzle puzzle = null;

    public void SetPuzzle(GridPuzzle p)
    {
        puzzle = p;
    }
    
    public bool AreAdjacent(GridPuzzlePiece a, GridPuzzlePiece b)
    {
        return (a.transform.position - b.transform.position).magnitude < 1.2f;
    }
    
    public virtual bool TryMove(GridPuzzlePiece a, GridPuzzlePiece b)
    {
        return AreAdjacent(a,b);
    }

    public virtual void PostMove()
    {
        
    }
}
