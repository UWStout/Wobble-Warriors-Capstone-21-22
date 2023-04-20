using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : Hazard
{

    [SerializeField] float rotationSpeed = 10;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = new Vector3(0, rotationSpeed, 0);
        kbMod = rotationSpeed;
    }
}
