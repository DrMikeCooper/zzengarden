using UnityEngine;
using System.Collections;

public class Lookable : MonoBehaviour {

    // use this in derived classes to show progress
    float alpha = 0;
    public float activationTime = 1;

    public void Reset()
    {
        alpha = 0;
    }

    public void Grow(float delta)
    {
        alpha += delta;
        if (alpha >= activationTime)
            Activate();
    }

    // override in derived classes
    public virtual void Activate()
    {
        Reset();
    }
}
