using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    private Vector3 origin;
    private float time = 0;
    [SerializeField] float bobSpeed = 2;
    [SerializeField] float bobIntensity = .25f;//the height of the bob
    [SerializeField] float rotateSpeed = 6;

    void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        time += bobSpeed * Time.deltaTime;
        if(time >= Mathf.PI * 2) //reset timer to avoid overflow
        {
            time = 0;
        }
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime); //rotate the model
        transform.position = new Vector3(origin.x, origin.y + Mathf.Sin(time) * bobIntensity, origin.z); //change the y position relative to the original y position based on a sin wave
    }
}
