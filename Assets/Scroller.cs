using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

    Material mat;

	// Use this for initialization
	void Start () {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mat = mr.material;
	}
	
	// Update is called once per frame
	void Update () {
	    float timer = 12.0f * Mathf.Sin(Time.time*0.1f);
        mat.SetFloat("_Timer", timer);
	}
}
