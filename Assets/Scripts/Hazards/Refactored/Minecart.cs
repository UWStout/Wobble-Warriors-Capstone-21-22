using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : Hazard
{
    [SerializeField] GameObject StartTunnel;
    Vector3 StartPoint;
    [SerializeField] GameObject EndTunnel;
    Vector3 EndPoint;
    [SerializeField] GameObject Cart;
    [SerializeField, Range(0.0f, 30.0f), Tooltip("How long between runs")]float Delay= 1;
    [SerializeField, Range(0.0f, 30.0f), Tooltip("How long it takes for the minecart to move from start to finish")] float MoveTime = 2;
    float currentTime=0;
    // Start is called before the first frame update
    void Start()
    {
        StartPoint = StartTunnel.transform.position;
        EndPoint = EndTunnel.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        Debug.Log(currentTime);
        if(currentTime > Delay) { 
            Cart.transform.position = Vector3.Lerp(StartTunnel.transform.position, EndTunnel.transform.position, Mathf.Max(currentTime - Delay, 0) / (MoveTime)); 
        }
        Cart.transform.LookAt(EndTunnel.transform);
        if (currentTime > Delay + MoveTime)
        {
            currentTime = 0;
        }
    }
}
