using UnityEngine;
using System.Collections;

public class Looker : MonoBehaviour {

    Lookable current;
    public Transform reticle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // find the object we're currently looking at
        Lookable obj = null;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, 8.0f, 1 << 10))
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
            if (current)
                current.Grow(Time.deltaTime);
        }

        if (reticle)
        {
            float scale = current == null ? 0 : 0.5f * current.alpha / current.activationTime;
            reticle.localScale = new Vector3(scale, scale, scale);
        }

	}
}
