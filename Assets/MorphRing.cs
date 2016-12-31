using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MorphRing : Morph {

    public int numSegments = 16; // number of segments making the larger ring
    public int numFaces = 4; // number of faces in each segment, eg its a triangular prism (==3) or square prism (==4)
    public float totalAngleOffset = 90; // total twist around the whole ring - eg 180 for a mobius strip
    float angleOffset = 0; // angular offset in degrees per face

    // first ring goes: 
    // 0 2    4 6   8 10 
    // 1 3    5 7   9 11  etc.
    Vector3[] verts;
    Vector3[] norms;

    public float radius1 = 1;
    public float radius2 = 0.25f;
    float deltaTheta, deltaPhi;

	// Use this for initialization
	void Start () {
        meshFilter = GetComponent<MeshFilter>();

        int numVerts = numSegments * numFaces * 4;
        verts = new Vector3[numVerts];
        norms = new Vector3[numVerts];

        // uvs and topology don't change
        Vector2[] uvs = new Vector2[numVerts];
        int[] indices = new int[numSegments* numFaces*6];

        int k = 0;
        for (int i = 0; i < numSegments; i++)
        {
            for (int j = 0; j < numFaces; j++)
            {
                int ibase = (i * numFaces + j) * 4;
                indices[k] = ibase; k++;
                indices[k] = ibase+2; k++;
                indices[k] = ibase+1; k++;
                indices[k] = ibase+1; k++;
                indices[k] = ibase+2; k++;
                indices[k] = ibase+3; k++;

                uvs[ibase] = new Vector2(((float)i) / ((float)numSegments), ((float)j) / ((float)numFaces));
                uvs[ibase+1] = new Vector2(((float)i+1) / ((float)numSegments), ((float)j) / ((float)numFaces));
                uvs[ibase+2] = new Vector2(((float)i) / ((float)numSegments), ((float)j+1) / ((float)numFaces));
                uvs[ibase+3] = new Vector2(((float)i+1) / ((float)numSegments), ((float)j+1) / ((float)numFaces));
            }
        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.normals = norms;
        meshFilter.mesh.uv = uvs;
        meshFilter.mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        angleOffset = Mathf.Deg2Rad * totalAngleOffset / numSegments;
        deltaPhi = 1.0f / numFaces;
        deltaTheta = 1.0f / numSegments;
    }
	
	// Update is called once per frame
	void Update () {
        if (alpha != alphaLast)
        {
            // update vertices
            float angle0 = alpha;

            for (int i = 0; i < numSegments; i++)
            {
                float angle1 = 2 * Mathf.PI * (i * deltaTheta);
                Vector3 centre1 = new Vector3(Mathf.Sin(angle1), 0, Mathf.Cos(angle1));
                float angle2 = 2 * Mathf.PI * ((i+1) * deltaTheta);
                Vector3 centre2 = new Vector3(Mathf.Sin(angle2), 0, Mathf.Cos(angle2));

                for (int j = 0; j < numFaces; j++)
                {
                    float phi1 = angle0 + 2 * Mathf.PI * (j * deltaPhi) + i * angleOffset;
                    float phi2 = angle0 + 2 * Mathf.PI * ((j+1) * deltaPhi) + i * angleOffset;

                    int ibase = (i * numFaces + j) * 4;
                    verts[ibase] = centre1 * (radius1 + radius2 * Mathf.Sin(phi1)) + Vector3.up * radius2 * Mathf.Cos(phi1);
                    verts[ibase+1] = centre2 * (radius1 + radius2 * Mathf.Sin(phi1+angleOffset)) + Vector3.up * radius2* Mathf.Cos(phi1+angleOffset);
                    verts[ibase+2] = centre1 * (radius1 + radius2 * Mathf.Sin(phi2)) + Vector3.up * radius2 * Mathf.Cos(phi2);
                    verts[ibase+3] = centre2 * (radius1 + radius2 * Mathf.Sin(phi2+angleOffset)) + Vector3.up * radius2 * Mathf.Cos(phi2+angleOffset);

                    Vector3 norm = Vector3.Cross(verts[ibase + 2] - verts[ibase], verts[ibase + 1] - verts[ibase]).normalized;

                    norms[ibase] = norm;
                    norms[ibase + 1] = norm;
                    norms[ibase + 2] = norm;
                    norms[ibase + 3] = norm;
                }
            }
            meshFilter.mesh.vertices = verts;
            meshFilter.mesh.normals = norms;

            meshFilter.mesh.RecalculateBounds();
            alphaLast = alpha;
        }
	}
}
