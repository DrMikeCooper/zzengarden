using UnityEngine;
using System.Collections;

public class GazeFollower : MonoBehaviour {

    public Transform[] indicators;
    public bool contact;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        Quaternion q = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.Head);
        Vector3 dir = Camera.main.transform.forward;

        foreach (Transform t in indicators)
            t.up = dir;

        Ray ray = new Ray(Camera.main.transform.position, dir);
        RaycastHit hit;
        contact = false;
        if (Physics.Raycast(ray, out hit, 1000.0f, (1<<8)|(1<<9)))
        {
            contact = (hit.transform.gameObject.layer == 8);
            transform.position = hit.point;
        }
    }
}

