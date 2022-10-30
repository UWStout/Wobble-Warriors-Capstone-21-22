using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTexture : MonoBehaviour
{

    [SerializeField] Vector2 animateSpeed;
    private Material mat;
    private Vector2 values;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        values += animateSpeed * Time.deltaTime/2.5f;
        mat.mainTextureOffset = values;//modifies the texture offset by animate speed * time.deltatime/ 2 1/2
    }

    public Vector2 getASpeed()
    {
        return animateSpeed;
    }
}
