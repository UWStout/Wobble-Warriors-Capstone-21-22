using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : Hazard
{
    [SerializeField] float rotationSpeed = 10;

    void Start()
    {
    }

    void FixedUpdate()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
