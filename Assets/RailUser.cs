using UnityEngine;
using System.Collections;

public class RailUser : MonoBehaviour {

    public RailNode startNode;
    public RailNode currentNode;
    public RailEdge currentEdge;
    public RailEdge newEdge;
    public float edgeTimer = 0;

    float alpha; //lerp value along edge
    public float speed;

	// Use this for initialization
	void Start () {
        currentNode = startNode;
	}
	
	// Update is called once per frame
	void Update () {
        Transform cam = Camera.main.transform;

        float angle = cam.eulerAngles.x;
        Vector3 gaze = cam.forward;
        if (angle > 10 && angle < 25) // ducking our head, so move forwards
        {
            // along an edge either way if we're on one
            if (currentEdge != null)
            {
                if (Vector3.Dot(gaze, currentEdge.direction) > 0.9f)
                {
                    alpha += speed / currentEdge.span * Time.deltaTime;
                    if (alpha >= 0.98f)
                    {
                        currentNode = currentEdge.node2;
                        currentEdge = null;
                    }
                }
                else if (Vector3.Dot(gaze, currentEdge.direction) < -0.9f)
                {
                    alpha -= speed / currentEdge.span * Time.deltaTime;
                    if (alpha <= 0.02f)
                    {
                        currentNode = currentEdge.node1;
                        currentEdge = null;
                    }

                }
            }

            // on a node, find the best edge suiting our direction of gaze
            if (currentNode != null)
            {
                float bestDot = 0.9f;
                RailEdge bestEdge = null;
                for (int i = 0; i < currentNode.edges.Count; i++)
                {
                    RailEdge edge = currentNode.edges[i] as RailEdge;
                    bool atStart = (edge.node1 == currentNode);
                    Vector3 dir = atStart ? edge.direction : -edge.direction;
                    float dot = Vector3.Dot(gaze, dir);
                    if (dot > bestDot)
                    {
                        alpha = atStart ? 0 : 1;
                        bestDot = dot;
                        bestEdge = edge;
                    }
                }
                bool switched = true;
                if (bestEdge)
                {
                    if (bestEdge == newEdge)
                    {
                        switched = false;
                        edgeTimer += Time.deltaTime;
                        if (edgeTimer > 1.0f)
                        {

                            currentNode = null;
                            currentEdge = bestEdge;
                        }
                    }
                }
                if (switched)
                {
                    newEdge = bestEdge;
                    edgeTimer = 0;
                }
            }
        }

        // update our position
        if (currentNode != null)
            transform.position = currentNode.transform.position;
        if (currentEdge != null)
            transform.position = Vector3.Lerp(currentEdge.node1.transform.position, currentEdge.node2.transform.position, alpha);

	}
}
