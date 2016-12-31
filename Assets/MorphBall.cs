using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MorphBall : Morph {

    // parametric value
   

    int numPoints = 5;
    int numVerts;
    // first face is x=1, with vertices 0 1 2 3 4, 5 6 7, 8, 9, etc. 
    // Opposite face x= -1 is 25 - 49
    // y = 1, 50 -74, y=-1, 75-99
    // z = 1, 100 - 124, z=-1, 125 - 149

    // the 3 morph targets - cube, ball and spike
    Vector3[] cubePos;
    Vector3[] cubeNorm;
    Vector3[] ballPos;
    Vector3[] ballNorm;
    Vector3[] spikePos;
    Vector3[] spikeNorm;

    // Use this for initialization
    void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();

        numVerts = numPoints * numPoints * 6;
        cubePos = new Vector3[numVerts];
        cubeNorm = new Vector3[numVerts];
        ballPos = new Vector3[numVerts];
        ballNorm = new Vector3[numVerts];
        spikePos = new Vector3[numVerts];
        spikeNorm = new Vector3[numVerts];

        // 3 points per tri, 2 per quad, 16 quads per face, 6 faces
        int[] indices = new int[3 * 2 * (numPoints-1) * (numPoints-1) * 6];
        Vector2[] uvs = new Vector2[numVerts];

        int iindex = 0;

        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < numPoints; i++)
            {
                float u0 = (float)i / (float)(numPoints - 1);
                for (int j = 0; j < numPoints; j++)
                {
                    float v0 = (float)j / (float)(numPoints - 1);
                    int index = (k * numPoints * numPoints) + i + (j * numPoints);
                    uvs[index] = new Vector2(u0, v0);

                    switch (k)
                    {
                        case 0: //x = 1;
                            cubePos[index] = 0.3f * new Vector3(1, 2 * u0 - 1, 2 * v0 - 1);
                            cubeNorm[index] = new Vector3(1, 0, 0);
                            break;
                        case 1: //x = -1;
                            cubePos[index] = 0.3f * new Vector3(-1, 1 - 2 * u0, 2 * v0 - 1);
                            cubeNorm[index] = new Vector3(-1, 0, 0);
                            break;
                        case 2: //y = 1;
                            cubePos[index] = 0.3f * new Vector3(2 * v0 - 1, 1, 2 * u0 - 1);
                            cubeNorm[index] = new Vector3(0, 1, 0);
                            break;
                        case 3: //y = -1;
                            cubePos[index] = 0.3f * new Vector3(2 * v0 - 1, -1, 1 - 2 * u0);
                            cubeNorm[index] = new Vector3(0, -1, 0);
                            break;
                        case 4: //z = 1;
                            cubePos[index] = 0.3f * new Vector3(2 * u0 - 1, 2 * v0 - 1, 1);
                            cubeNorm[index] = new Vector3(0, 0, 1);
                            break;
                        case 5: //z = -1;
                            cubePos[index] = 0.3f * new Vector3(1- 2 * u0, 2 * v0 - 1, -1);
                            cubeNorm[index] = new Vector3(0, 0, -1);
                            break;
                    }
                    ballNorm[index] = cubePos[index].normalized;
                    ballPos[index] = 0.4f * ballNorm[index];

                    // calculate quads
                    if (i != numPoints-1 && j != numPoints-1)
                    {
                        indices[iindex] = index; iindex++;
                        indices[iindex] = index+1; iindex++;
                        indices[iindex] = index+numPoints; iindex++;

                        indices[iindex] = index+numPoints; iindex++;
                        indices[iindex] = index+1; iindex++;
                        indices[iindex] = index+numPoints+1; iindex++;
                    }
                }
            }
        }

        meshFilter.mesh.vertices = ballPos;
        meshFilter.mesh.normals = ballNorm;
        meshFilter.mesh.uv = uvs;
        meshFilter.mesh.SetIndices(indices, MeshTopology.Triangles, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (alpha != alphaLast)
        {
            if (alpha > 0) // morph from 0(cube) to 1 (sphere)
            {
                if (alpha > 1)
                    alpha = 1;
                Vector3[] verts = new Vector3[numVerts];
                Vector3[] norms = new Vector3[numVerts];
                for (int k = 0; k < numVerts; k++)
                {
                    verts[k] = Vector3.Lerp(cubePos[k], ballPos[k], alpha);
                    norms[k] = Vector3.Lerp(cubeNorm[k], ballNorm[k], alpha);
                }
                meshFilter.mesh.vertices = verts;
                meshFilter.mesh.normals = norms;
                alphaLast = alpha;
            }
        }
	}
}
