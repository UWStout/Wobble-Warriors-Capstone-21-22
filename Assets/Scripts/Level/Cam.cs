using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]
    Vector3 CameraOffset = new Vector3(0f, 10.8999996f, -11.1199999f);
    Vector3 here;
    Vector3 there;
    float MaxTime = .5f;
    float CurrentTime;
    Director director;
    [SerializeField] float damper = .1f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dir = GameObject.Find("Director");
        director = dir.GetComponent<Director>();
        CurrentTime = MaxTime;

        here = this.transform.position;
        there = here;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Vector3.Lerp(here, there, Mathf.Sqrt( CurrentTime) / MaxTime);
        //CurrentTime = Mathf.Min(CurrentTime + Time.deltaTime, MaxTime);
        Vector3 z = Vector3.zero;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, DynamicTarget(), ref z, damper);
    }
    public Vector3 NewTarget(Vector3 Pos)
    {
        here = this.transform.position;
        there = Pos;
        CurrentTime = 0f;
        return there;
    }
    public Vector3 DynamicTarget()
    {
        return director.GetPlayerCenter()+CameraOffset;
    }
}
