using UnityEngine;
using System.Collections;

public class ProcMeshes {

    static Mesh square = null;

    public static Vector2[] GetSquareUV(float span)
    {
        float sp = span * 0.5f;
        Vector2[] uv =  { new Vector2(-1, -sp), new Vector2(-1, sp), new Vector2(1, -sp), new Vector2(1, sp) };
        return uv;
    }

    public static Mesh GetSquare()
    {
        if (square == null)
        {
            square = new Mesh();
            Vector3[] verts = { new Vector3(-1, 0, -1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(1, 0, 1) };
            Vector3[] normals = { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
            Vector2[] uvs = GetSquareUV(2);
            int[] indices = { 0, 1, 2, 2, 1, 3 };
            square.vertices = verts;
            square.normals = normals;
            square.uv = uvs;
            square.SetIndices(indices, MeshTopology.Triangles, 0, true);
        }
        return square;
    }
}
