using UnityEngine;
using System.Collections;

public class ZenTree : MonoBehaviour {

    public GameObject[] branchSet;
    public GameObject[] leafSet;

    ArrayList growing = new ArrayList();

	// Use this for initialization
	void Start () {
        GrowChild(gameObject, 1);
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < growing.Count; i++)
        {
            GameObject obj = growing[i] as GameObject;
            ZenBranch br = obj.GetComponent<ZenBranch>();

            float scale = obj.transform.localScale.x;
            scale += br.speed;
            obj.transform.localScale = new Vector3(scale, scale, scale);
            if (scale >= 1.0f)
            {
                for (int k = 0; k < obj.transform.childCount; k++)
                {
                    Transform ch = obj.transform.GetChild(k);
                    if (ch.GetComponent<MeshRenderer>() == null)
                        GrowChild(ch.gameObject, br.generation + 1);
                }
                growing.Remove(obj);
            }
        }
	}

    void GrowChild(GameObject root, int counter)
    {
        if (counter > 4)
            return;

        GameObject branch = branchSet[Random.Range(0, branchSet.Length)];
        GameObject child = Instantiate(branch);
        child.transform.parent = root.transform;
        child.transform.localPosition = new Vector3(0, 0, 0);
        child.transform.localRotation = new Quaternion();
        child.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        ZenBranch br = child.GetComponent<ZenBranch>();
        br.generation = counter;
 
        growing.Add(child);
    }
}
