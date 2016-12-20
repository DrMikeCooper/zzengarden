using UnityEngine;
using System.Collections;

public class Looker : MonoBehaviour {

    Lookable current;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // find the object we're currently looking at
        Lookable obj = null;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, 1 << 10))
        {
            obj = hitInfo.transform.GetComponent<Lookable>();
        }

        if (current != obj)
        {
            if (current)
                current.Reset();
            if (obj)
                obj.Reset();

            current = obj;
        }
        else
        {
            current.Grow(Time.deltaTime);
        }

	}
}
