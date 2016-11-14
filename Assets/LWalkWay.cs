using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RailEdge))]
[RequireComponent(typeof(LPiece))]
public class LWalkWay : MonoBehaviour {


    // stripe these from either end
    public GameObject[] templates;
    public float spacing;


	// Use this for initialization
	void Start ()
    {
        float startDist = 3;
        float endDist = 3; // TODO

        RailEdge edge = GetComponent<RailEdge>();
        float span = (edge.node2.transform.position - edge.node1.transform.position).magnitude;
        float innerDist = span - startDist - endDist; 
        int num = (int)(innerDist / spacing);

        // and spawn our children
        Vector3 direction = (edge.node2.transform.position - edge.node1.transform.position).normalized;
        for (int i = 0; i < num; i++)
        {
            int index = (i >= num / 2) ? num - i : i;
            index = index % templates.Length;
            GameObject obj = Instantiate(templates[index]);
            obj.transform.position = edge.node1.transform.position + (startDist + i * spacing) * direction;
            obj.transform.forward = direction;
            obj.transform.parent = transform;
            obj.name = name + "_step_" + i;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
