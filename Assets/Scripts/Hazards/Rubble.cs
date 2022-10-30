using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : MonoBehaviour
{
    [SerializeField] private float TimeToDestroy = 2;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeToDestroy);
        gameObject.GetComponent<Rigidbody>().AddForce(0, -100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
