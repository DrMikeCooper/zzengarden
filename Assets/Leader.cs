using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class Leader : MonoBehaviour {

    public AICharacterControl follower;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
            follower.SetTarget(transform);
        if (Input.GetKeyDown(KeyCode.O))
            follower.SetTarget(null);
	}
}
