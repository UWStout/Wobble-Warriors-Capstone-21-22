using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoot : MonoBehaviour
{
    private Vector3 OrPos;
    private Vector3 TargetPos;
    private Vector3 z = Vector3.zero;
    [SerializeField] float MoveDuration = 1f;
    [SerializeField] float Distance = 7f;
    [SerializeField] bool up = false;
    float dt = 0;
    bool InTheWay = false;
    // Start is called before the first frame update
    void Start()
    {
        OrPos = transform.position;
        TargetPos = OrPos;
        if (up)
        {
            OrPos.y += Distance;
        }
        else
        {
            OrPos.y -= Distance;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InTheWay)
        {
            dt += Time.deltaTime;
        }
        else
        {
            dt -= Time.deltaTime;
        }
        dt = Mathf.Clamp(dt, 0, MoveDuration);
        //transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref z, damper);
        transform.position = Vector3.Lerp(transform.position, TargetPos, dt/MoveDuration);
        InTheWay = false;
    }
    public void SetInTheWay()
    {
        InTheWay = true;
    }
}
