﻿using UnityEngine;
using System.Collections;

public class Faller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 10, 0);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

	}
}
