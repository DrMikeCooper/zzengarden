using UnityEngine;
using System.Collections;

public class GazeFollower : MonoBehaviour {

    public Transform[] indicators;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion q = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.Head);
        Vector3 dir = q * Camera.main.transform.forward;

        foreach (Transform t in indicators)
            t.up = dir;

        Ray ray = new Ray(Camera.main.transform.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            transform.position = hit.point;
        }
    }
}

