using UnityEngine;
using System.Collections;

public class RailEdge : MonoBehaviour {

    public RailNode node1;
    public RailNode node2;

    public Vector3 direction;
    public float span;
    public float width = 0.1f;

    MeshFilter meshFilter = null;

	// Use this for initialization
	void Start () {
        node1.edges.Add(this);
        node2.edges.Add(this);

        direction = (node2.transform.position - node1.transform.position).normalized;
        span = (node2.transform.position - node1.transform.position).magnitude;

    }
	
	// Update is called once per frame
	void Update () {
        DebugDraw();
	}

    void DebugDraw()
    {
        if (width > 0.1f)
            width -= 0.01f;

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = ProcMeshes.GetSquare();
            meshFilter.mesh.uv = ProcMeshes.GetSquareUV(span);
            transform.position = 0.5f * (node2.transform.position + node1.transform.position);
            transform.forward = direction;
        }

        transform.localScale = new Vector3(width * 0.5f, 1, span * 0.5f);
    }
}
