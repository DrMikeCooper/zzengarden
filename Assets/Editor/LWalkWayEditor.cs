using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LWalkWay))]
public class LWalkWayEditor : Editor
{

    public void OnSceneGUI()
    {
        LWalkWay ww = target as LWalkWay;

        Vector3[] pts = ww.GetPoints();

        // and spawn our children
        Quaternion q = Quaternion.LookRotation(ww.GetDirection());
        for (int i = 0; i < pts.Length; i++)
        {
            Vector3 pos = pts[i];
            Handles.CubeCap(i, pos, q, 0.5f);
        }
    }
}
