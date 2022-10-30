using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    Vector3 here;
    Vector3 there;
    float MaxTime = .5f;
    float CurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = MaxTime;
        here = this.transform.position;
        there = here;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(here, there, Mathf.Sqrt( CurrentTime) / MaxTime);
        CurrentTime = Mathf.Min(CurrentTime + Time.deltaTime, MaxTime);
    }
    public Vector3 NewTarget(Vector3 Pos)
    {
        here = this.transform.position;
        there = Pos;
        CurrentTime = 0f;
        return there;
    }
}
