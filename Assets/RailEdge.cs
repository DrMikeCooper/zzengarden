using UnityEngine;
using System.Collections;

public class RailEdge : MonoBehaviour {

    public RailNode node1;
    public RailNode node2;

    public Vector3 direction;
    public float span;

    
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

    LineRenderer line = null;
    void DebugDraw()
    {
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.SetColors(Color.yellow, Color.yellow);
            Vector3[] pos = new Vector3[2];
            pos[0] = node1.transform.position;
            pos[1] = node2.transform.position;
            line.SetPositions(pos);
        }
    }
}
