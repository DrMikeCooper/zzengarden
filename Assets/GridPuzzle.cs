using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class GridPuzzle : MonoBehaviour {

    public int columns;
    public int rows;
    public GameObject piece;

    [HideInInspector]
    GridPuzzlePiece current;

    PuzzleRules rules;

    AudioSource audioSource;
    public AudioClip sparkle;
    public AudioClip select;
    public AudioClip deselect;

    GridPuzzlePiece[] pieceList;
    GridPuzzlePiece[,] pieces;
    Vector3[,] positions;
    int currentIndex;

    Color[] colors = { Color.red, Color.blue, Color.cyan, Color.yellow };

    public Vector3 GetPos(int i, int j)
    {
        float x0 = ((((float)i) / (columns - 1)) - 0.5f) * (columns - 1);
        float y0 = ((((float)j) / (rows - 1)) - 0.5f) * (rows - 1);
        return new Vector3(x0, y0, 0);
    }

	// Use this for initialization
	void Start () {
        pieceList = new GridPuzzlePiece[columns * rows];
        pieces = new GridPuzzlePiece[columns, rows];
        positions = new Vector3[columns, rows];

        int k = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate<GameObject>(piece);
                obj.transform.parent = transform;
                obj.transform.localPosition = GetPos(i, j);
                positions[i, j] = obj.transform.position;
                obj.name = "piece_" + i + "_" + j;
                GridPuzzlePiece gpp = obj.GetComponent<GridPuzzlePiece>();
                gpp.x0 = i;
                gpp.y0 = j;
                gpp.index = Random.Range(0, 4);
                obj.GetComponent<MeshRenderer>().material.color = colors[gpp.index];
                gpp.puzzle = this;
                pieceList[k] = gpp; k++;
            }
        }

        audioSource = GetComponent<AudioSource>();
        rules = GetComponent<PuzzleRules>();
        rules.SetPuzzle(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GridPuzzlePiece piece = hit.collider.GetComponent<GridPuzzlePiece>();
                if (piece != null)
                {
                    ClickOnPiece(piece);
                }

            }
        }
	}

    public void ClickOnPiece(GridPuzzlePiece piece)
    {
        if (piece == current)
        {
            current = null;

            // shrink back down
            //piece.transform.localScale = new Vector3(1, 1, 1);

            piece.ScaleTo(1);
            audioSource.clip = deselect;
            audioSource.Play();
        }
        else
        {
            if (current == null)
            {
                current = piece;

                // scale up
                //piece.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

                piece.ScaleTo(1.4f);
                audioSource.clip = select;
                audioSource.Play();
            }
            else
            {
                // swap piece and current and scale current down
                //Vector3 pos = piece.transform.position;
                //piece.transform.position = current.transform.position;
                //current.transform.position = pos;
                //current.transform.localScale = new Vector3(1, 1, 1);

                if (rules.TryMove(current, piece))
                {
                    Swap(piece, current);
                    piece.Sparkle();
                    current.Sparkle();
                    audioSource.clip = sparkle;

                    rules.PostMove(current, piece);
                }
                else
                {
                    audioSource.clip = deselect;
                }

                audioSource.Play();
                current.ScaleTo(1);
                current = null;
            }
        }
    }

    void Swap(GridPuzzlePiece a, GridPuzzlePiece b, float delay = 0)
    {
        a.MoveTo(b.transform.position, delay);
        b.MoveTo(a.transform.position, delay);
        int x0 = a.x0; a.x0 = b.x0; b.x0 = x0;
        int y0 = a.y0; a.y0 = b.y0; b.y0 = y0;
    }

    // update the grid of piece references
    void UpdatePieces()
    {
        for (int k = 0; k < rows * columns; k++)
        {
            GridPuzzlePiece gpp = pieceList[k];
            pieces[gpp.x0, gpp.y0] = gpp;
        }
    }

    public void RemoveBlocks(int size)
    {
        // list of all pieces to remove
        ArrayList removes = new ArrayList();

        ArrayList matches = new ArrayList();

        // figure out which pieces are due to be removed
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                UpdatePieces();
                matches.Clear();

                currentIndex = pieces[i, j].index;
                AddToMatches(i, j, matches);

                // matches now contains a big list of all matching pieces touching this one.
                if (matches.Count >= size)
                {
                    foreach (GridPuzzlePiece gpp in matches)
                    {
                        if (!removes.Contains(gpp))
                        {
                            removes.Add(gpp);
                        }
                    }
                }
            }
        }

        // process the pieces
        UpdatePieces();
        GridPuzzlePiece[,] pieces0 = new GridPuzzlePiece[columns, rows];
        for (int i = 0; i < columns; i++)
            for (int j = 0; j < rows; j++)
                pieces0[i, j] = pieces[i, j];

        // remove all those that should go
        foreach (GridPuzzlePiece gpp in removes)
        {
            pieces[gpp.x0, gpp.y0] = null;
            gpp.ScaleTo(0);
        }

        // move everything remaining down to fill the gaps
        bool done = false;
        while (!done)
        {
            done = true;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 1; j < rows; j++)
                {
                    // if the one below is empty and you're not, fall down one square!
                    if (pieces[i, j] != null && pieces[i, j - 1] == null)
                    {
                        Swap(pieces0[i, j], pieces0[i, j - 1], 0);
                        GridPuzzlePiece gpp0 = pieces0[i, j]; pieces0[i, j] = pieces0[i, j - 1]; pieces[i, j - 1] = gpp0;
                        pieces[i, j - 1] = pieces[i, j];
                        pieces[i, j] = null;
                        done = false;
                    }
                }
            }
        }

        // distribute the removed pieces into the holes
        int k = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // if the one below is empty and you're not, fall down one square!
                if (pieces[i, j] == null)
                {
                    pieces[i, j] = removes[k] as GridPuzzlePiece; k++;
                    pieces[i, j].ScaleTo(1, 2);
                    pieces[i, j].index = Random.Range(0, 4);
                    pieces[i, j].GetComponent<MeshRenderer>().material.color = colors[pieces[i, j].index];
                }
                pieces[i, j].MoveTo(positions[i, j], 1, 0.1f);
                pieces[i, j].x0 = i;
                pieces[i, j].y0 = j;
            }
        }

    }

    void AddToMatches(int i, int j, ArrayList matches)
    {
        if (pieces[i, j] && pieces[i, j].index == currentIndex)
        {
            matches.Add(pieces[i, j]);

            pieces[i, j] = null;

            // check 4 neighbours
            if (i != 0) AddToMatches(i - 1, j, matches);
            if (i != columns - 1) AddToMatches(i + 1, j, matches);
            if (j != 0) AddToMatches(i, j - 1, matches);
            if (j != rows - 1) AddToMatches(i, j + 1, matches);
        }
    }
}
