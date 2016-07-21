using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

    public int number;
    public float angle;
    public Vector3 offset;
    public GameObject template;

    int cNumber = 0;
    float cAngle = 0;
    Vector3 cOffset = new Vector3(0, 0, 0);

    GameObject[] steps = new GameObject[0];

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (number != cNumber || cAngle != angle || cOffset != offset)
        {
            CreateSteps();
        }

        cNumber = number;
        cAngle = angle;
        cOffset = offset;
	}

    void CreateSteps()
    {
        foreach (GameObject step in steps)
            Destroy(step);

        steps = new GameObject[number];
        for (int i = 0; i < number; i++)
        {
            steps[i] = Instantiate(template);
            Transform prev = (i == 0) ? transform : steps[i - 1].transform;
            steps[i].transform.position = prev.position + prev.TransformDirection(offset);
            steps[i].transform.eulerAngles = prev.eulerAngles + new Vector3(0,angle,0);
            steps[i].name = "step_" + i;
            steps[i].transform.parent = transform;
        }
    }
}
