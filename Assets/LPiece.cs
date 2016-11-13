using UnityEngine;
using System.Collections;

public class LPiece : MonoBehaviour {

    [System.Serializable]
    public class LKeyFrame
    {
        public float t;
        public Vector3 position;
        public Vector3 rotation;
        public float localScale = 1;
        public Vector3 scale = new Vector3(1,1,1);
    }

    Vector3 startPos;
    Vector3 startAngles;

    public float t; // current time
    public float timeOffset;
    public LKeyFrame[] keyFrames;
    Transform meshChild;
    LPiece[] children;

    // Use this for initialization
    void Start() {

        startPos = transform.localPosition;
        //startAngles = transform.eulerAngles;

        // search our immediate children for LPieces and store this list
        ArrayList list = new ArrayList();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            LPiece sub = child.GetComponent<LPiece>();
            if (sub)
                list.Add(sub);
            //else if (meshChild == null && child.GetComponent<MeshFilter>())
            //    meshChild = child; // store a non-lpiece child with a meshfilter on it
        }
        children = new LPiece[list.Count];
        for (int i = 0; i < list.Count; i++)
            children[i] = list[i] as LPiece;

        // check keyframes for time sequence
        for (int i = 0; i < keyFrames.Length-1; i++)
        {
            if (keyFrames[i].t >= keyFrames[i + 1].t)
                Debug.Log("Bad Keyframe Sequence");
        }

        MoveMeshToChild();
	}

    void MoveMeshToChild()
    {
        MeshFilter mf2 = GetComponent<MeshFilter>();
        MeshRenderer mr2 = GetComponent<MeshRenderer>();
        if (mf2 == null || mr2 == null)
            return;

        GameObject obj = new GameObject();
        obj.name = name + "_body";
        meshChild = obj.transform;
        meshChild.parent = transform;
        meshChild.localPosition = Vector3.zero;
        meshChild.localRotation = Quaternion.identity;
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        
        mf.mesh = mf2.mesh;
        mr.material = mr2.material;
        Destroy(mr2);
        Destroy(mf2);
    }

	// Update is called once per frame
	void Update () {
        // update the time for each child in our list
        for (int i = 0; i < children.Length; i++)
        {
            children[i].t = t - children[i].timeOffset;
        }

        // search our keyframes for the current time
        if (keyFrames.Length == 0)
            return;

        int end = keyFrames.Length - 1;

        
        if (t <= keyFrames[0].t)
        {
            // before the first one
            transform.localPosition = startPos + keyFrames[0].position;
            transform.localEulerAngles = startAngles + keyFrames[0].rotation;
            transform.localScale = new Vector3(keyFrames[0].localScale, keyFrames[0].localScale, keyFrames[0].localScale);
            if (meshChild!=null)
                meshChild.localScale = keyFrames[0].scale;
        }
        else if (t >= keyFrames[end].t)
        {
            // after the last one
            transform.localPosition = startPos + keyFrames[end].position;
            transform.localEulerAngles = startAngles + keyFrames[end].rotation;
            transform.localScale = new Vector3(keyFrames[end].localScale, keyFrames[end].localScale, keyFrames[end].localScale);
            if (meshChild != null)
                meshChild.localScale = keyFrames[end].scale;
        }
        else
        {
            // between two frames - interpolate
            for (int i = 0; i < end; i++)
            {
                if (t >= keyFrames[i].t && t < keyFrames[i + 1].t)
                {
                    float alpha = (t - keyFrames[i].t) / (keyFrames[i + 1].t - keyFrames[i].t);
                    transform.localPosition = startPos + Vector3.Lerp(keyFrames[i].position, keyFrames[i + 1].position, alpha);
                    transform.localEulerAngles = startAngles + Vector3.Lerp(keyFrames[i].rotation, keyFrames[i + 1].rotation, alpha);
                    float sc = Mathf.Lerp(keyFrames[i].localScale, keyFrames[i + 1].localScale, alpha);
                    transform.localScale = new Vector3(sc, sc, sc);
                    if (meshChild != null)
                        meshChild.localScale = Vector3.Lerp(keyFrames[i].scale, keyFrames[i + 1].scale, alpha);
                }
            }
        }
    }
}
