using UnityEngine;
using System.Collections;

public class RailTraveller : MonoBehaviour {

    public GazeFollower gaze;
    Vector3 targetPosition;
    Vector3 gazeStart;
    public float timer = 0;
    public bool contact;
    NavMeshAgent nv;

    // Use this for initialization
    void Start () {
        targetPosition = transform.position;
        gazeStart = targetPosition;

        nv = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        contact = gaze.contact; //dbg
        if (gaze.contact)
        {
            float distance = (gazeStart - gaze.transform.position).magnitude;
            if (distance < 1.0f)
                timer += Time.deltaTime;
            else
            {
                gazeStart = gaze.transform.position;
                timer = 0;
            }
            if (timer > 1)
            {
                nv.SetDestination(gaze.transform.position);
                targetPosition = gaze.transform.position;
                timer = 0;
            }
        }
        else
            timer = 0;

        float sc = 0.2f *(1+ timer);
        gaze.transform.localScale = new Vector3(sc,sc,sc);
        //nv.Set
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.1f);
	}
}
