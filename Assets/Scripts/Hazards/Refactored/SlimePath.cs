using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePath : MonoBehaviour
{
    [SerializeField] GameObject Waypoints;
    [SerializeField] GameObject Slime;
    Vector3 Vel = Vector3.zero;
    int CurrentPoint = 0;
    [SerializeField] float Damper = .2f;
    List<Vector3> WaypointList = new List<Vector3>();
    [SerializeField] float Precision = 1f;
    [SerializeField] float HangTime = 1f;
    float currentTime=0f;

    void Start()
    {
        if (Waypoints != null)
        {
            for(int i=0; i<Waypoints.transform.childCount; i++)
            {
                WaypointList.Add(Waypoints.transform.GetChild(i).position);
            }
        }
        if (WaypointList.Count == 0)
        {
            WaypointList.Add(Waypoints.transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Slime.transform.position = Vector3.SmoothDamp(Slime.transform.position, WaypointList[CurrentPoint], ref Vel, Damper);
        if (Vector3.Magnitude(Vel) > 0)
        {
            Slime.transform.rotation = Quaternion.LookRotation(Vel);
        }
        if (Vector3.Magnitude(Slime.transform.position - WaypointList[CurrentPoint]) < Precision)
        {
            currentTime += Time.deltaTime;
        
        }
        if (currentTime > HangTime)
        {
            CurrentPoint = (CurrentPoint + 1) % WaypointList.Count;
            currentTime = 0.0f;
        }
        
    }
}
