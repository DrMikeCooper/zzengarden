using UnityEngine;
using System.Collections;

public class LCycle : MonoBehaviour {

    public float speed=1;
    public float start=0;
    public float end=1;
    bool forwards = true;
    float current;
    LPiece lpiece;

	// Use this for initialization
	void Start () {
        current = start;
        lpiece = GetComponent<LPiece>();
	}
	
	// Update is called once per frame
	void Update () {
        if (forwards)
        {
            current += speed * Time.deltaTime;
            if (current > end)
            {
                current = end;
                forwards = false;
            }
        }
        else
        {
            current -= speed * Time.deltaTime;
            if (current < start)
            {
                current = start;
                forwards = true;
            }
        }
        lpiece.t = current;
    }
}
