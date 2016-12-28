using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GridPuzzle : MonoBehaviour {

    // size of the grid
    public int columns;
    public int rows;

    // the template for spawning pieces (to be replaced by array later)
    public GameObject piece;
    public Color[] colors = { Color.red *0.5f, Color.blue *0.5f, Color.cyan *0.5f, Color.yellow*0.5f };
    public int numTypes = 4;

    // currently selected piece
    GridPuzzlePiece current;

    // details of the type of puzzle - sibling component
    PuzzleRules rules;

    // sound and particles
    AudioSource audioSource;
    public AudioClip sparkle;
    public AudioClip select;
    public AudioClip deselect;

    // unchanging list of all gameobject pieces
    GridPuzzlePiece[] pieceList;

    // array of which pieces are where currently by row and column
    GridPuzzlePiece[,] pieces;

    // unchanging list of positions for i,j th piece on grid
    Vector3[,] positions;
    int currentIndex;

    // list of all pieces to remove
    ArrayList removes = new ArrayList();
    float pieceScale = 1;

    public PuzzleConsole console;

    enum State
    {
        Collapsed,
        Waiting,
        Swapping,
        Vanishing, 
        Moving,
        Refilling
    };

    State state;

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

        state = State.Waiting;
	}

    void Update()
    {
        switch (state)
        {
            case State.Waiting:
                UpdateWaiting();
                break;
            case State.Swapping:
                UpdateSwapping();
                break;
            case State.Vanishing:
                UpdateVanishing();
                break;
            case State.Moving:
                UpdateMoving();
                break;
            case State.Refilling:
                UpdateRefilling();
                break;
        }
    }

	// Ready for player input
	void UpdateWaiting()
    {
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

    void UpdateSwapping()
    {
        pieceScale += Time.deltaTime;
        if (pieceScale >= 1.0f)
        {
            pieceScale = 1.0f;
            state = State.Waiting;
            rules.PostMove();
        }
    }

    // making pieces disappear
    void UpdateVanishing()
    {
        pieceScale -= 2*Time.deltaTime;
        if (pieceScale < 0.0f)
        {
            pieceScale = 0.0f;
            state = State.Moving;

            // update colours for all removed pieces
            foreach (GridPuzzlePiece gpp in removes)
            {
                gpp.GetComponent<MeshRenderer>().material.color = colors[gpp.index];
            }
        }

        // scale all pieces that are vanishing
        foreach (GridPuzzlePiece gpp in removes)
        {
            gpp.transform.localScale = new Vector3(pieceScale, pieceScale, pieceScale);
        }
      
    }

    // moving everything to its new position determined by (x0,y0) in the piece
    void UpdateMoving()
    {
        bool stillMoving = false;
        // move all pieces towards their destination point
        foreach (GridPuzzlePiece gpp in pieceList)
        {
            Vector3 target = positions[gpp.x0, gpp.y0];
            gpp.transform.position = Vector3.MoveTowards(gpp.transform.position, target, 2*Time.deltaTime);
            if ((target - gpp.transform.position).magnitude > 0.0001f)
                stillMoving = true;

        }

        if (!stillMoving)
            state = State.Refilling;
    }

    // scaling up the pieces
    void UpdateRefilling()
    {
        pieceScale += 2*Time.deltaTime;
        if (pieceScale > 1.0f)
        {
            pieceScale = 1.0f;
            state = State.Waiting;

            // process again
            rules.PostMove();
        }

        // scale all pieces that are vanishing
        foreach (GridPuzzlePiece gpp in removes)
        {
            gpp.transform.localScale = new Vector3(pieceScale, pieceScale, pieceScale);
        }
    }

    public void ClickOnPiece(GridPuzzlePiece piece)
    {
        if (state != State.Waiting)
            return;

        if (piece == current)
        {
            current = null;

            piece.ScaleTo(1);
            audioSource.clip = deselect;
            audioSource.Play();
        }
        else
        {
            if (current == null)
            {
                current = piece;

                piece.ScaleTo(1.4f);
                audioSource.clip = select;
                audioSource.Play();
            }
            else
            {
                if (rules.TryMove(current, piece))
                {
                    Swap(piece, current);
                    piece.Sparkle();
                    current.Sparkle();
                    audioSource.clip = sparkle;

                    state = State.Swapping;
                    pieceScale = 0;
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
        ArrayList matches = new ArrayList();
        removes.Clear();

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
                    int num = 0;
                    foreach (GridPuzzlePiece gpp in matches)
                    {
                        if (!removes.Contains(gpp))
                        {
                            removes.Add(gpp);
                            num++;
                        }
                    }
                    if (num >= size)
                        console.scores[currentIndex] += num + 1 - size;
                }
            }
        }
        ProcessRemoves();
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

    public void RemoveRows(int size)
    {
        ArrayList matches = new ArrayList();
        removes.Clear();

        UpdatePieces();

        // figure out which pieces are due to be removed
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                matches.Clear();

                CheckForRow(i, j, size, 1, 0, matches);
                CheckForRow(i, j, size, 0, 1, matches);
                CheckForRow(i, j, size, 1, 1, matches);
                CheckForRow(i, j, size, 1, -1, matches);

                if (matches.Count >= size)
                    console.scores[currentIndex] += matches.Count + 1 - size;

                // matches now contains a big list of all matching pieces touching this one.
                foreach (GridPuzzlePiece gpp in matches)
                {
                    if (!removes.Contains(gpp))
                    {
                        removes.Add(gpp);
                    }
                }
            }
        }
        ProcessRemoves();
    }

    void CheckForRow(int i, int j, int size, int di, int dj, ArrayList matches)
    {
        currentIndex = pieces[i, j].index;
        if (i >= -size * di-1 && i <= columns - size * di && j >= -size * dj-1 && j <= rows - size * dj)
        {
            for (int k = 1; k < size; k++)
            {
                if (pieces[i + di * k, j + dj * k].index != currentIndex)
                    return;
            }

            // we have a row
            for (int k = 0; k < size; k++)
            {
                GridPuzzlePiece gpp = pieces[i + di * k, j + dj * k];
                matches.Add(gpp);
            }
        }
    }

    void ProcessRemoves()
    { 
        if (removes.Count == 0)
            return;

        state = State.Vanishing;

        // process the pieces - make a copy of their original settings
        UpdatePieces();
        GridPuzzlePiece[,] pieces0 = new GridPuzzlePiece[columns, rows];
        for (int i = 0; i < columns; i++)
            for (int j = 0; j < rows; j++)
                pieces0[i, j] = pieces[i, j];

        // remove all those that should go
        foreach (GridPuzzlePiece gpp in removes)
        {
            pieces[gpp.x0, gpp.y0] = null;
        }

        // move everything remaining down to fill the gaps, just mixing up the pieces/pieces0 arrays here
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
                        GridPuzzlePiece gpp0 = pieces0[i, j]; pieces0[i, j] = pieces0[i, j - 1]; pieces0[i, j - 1] = gpp0;
                        pieces[i, j - 1] = pieces[i, j];
                        pieces[i, j] = null;
                        done = false;
                    }
                }
            }
        }

        // use pieces0 to fix everyone's x0, y0
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                pieces0[i, j].x0 = i;
                pieces0[i, j].y0 = j;
            }
        }

        // distribute the removed pieces into the holes
        foreach (GridPuzzlePiece gpp in removes)
        {
            gpp.index = Random.Range(0, 4);
        }
    }
}
