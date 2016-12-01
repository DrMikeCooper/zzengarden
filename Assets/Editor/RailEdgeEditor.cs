using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RailEdge))]
public class RailEdgeEditor : Editor
{

    public void OnSceneGUI()
    {
        RailEdge edge = target as RailEdge;

        if (edge.node1 && edge.node2)
            Handles.DrawLine(edge.node1.transform.position, edge.node2.transform.position);
    }
}
