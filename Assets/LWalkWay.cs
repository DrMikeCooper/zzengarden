using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RailEdge))]
[RequireComponent(typeof(LPiece))]
public class LWalkWay : MonoBehaviour {


    // stripe these from either end
    public GameObject[] templates;
    public float spacing = 3;
    public float phase = 1;
    public float startDist = 3;
    public float endDist = 3;
    Vector3 direction;

    public Vector3[] GetPoints()
    {
        RailEdge edge = GetComponent<RailEdge>();
        float span = (edge.node2.transform.position - edge.node1.transform.position).magnitude;
        float innerDist = span - startDist - endDist;
        int num = 1 + (int)(innerDist / spacing);

        Vector3[] pts = new Vector3[num];

        // and spawn our children
        direction = (edge.node2.transform.position - edge.node1.transform.position).normalized;
        for (int i = 0; i < num; i++)
        {
            pts[i] = edge.node1.transform.position + (startDist + i * spacing) * direction;
        }

        return pts;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    // Use this for initialization
    void Start ()
    {
        Vector3[] pts = GetPoints();

        int num = pts.Length;

        // and spawn our children
        for (int i = 0; i < num; i++)
        {
            int index = (i >= num / 2) ? num - i : i;
            int index0 = index % templates.Length;
            GameObject obj = Instantiate(templates[index0]);
            obj.transform.position = pts[i];
            obj.transform.right = GetDirection();
            obj.transform.parent = transform;
            obj.name = name + "_step_" + i;
            LPiece lp = obj.GetComponent<LPiece>();
            if (lp)
                lp.timeOffset = index * phase;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
