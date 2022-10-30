using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    [SerializeField] float yValue = .75f;
    [SerializeField] Transform shadowPlane;

    void Update()
    {
        shadowPlane.position = new Vector3(transform.position.x,yValue,transform.position.z);
    }
}
