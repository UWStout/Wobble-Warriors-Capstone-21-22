using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoot : MonoBehaviour
{
    private Vector3 OrPos;
    private Vector3 TargetPos;
    private Vector3 z = Vector3.zero;
    public float damper = .1f;
    [SerializeField] float DesiredHeight = 3f;
    // Start is called before the first frame update
    void Start()
    {
        OrPos = transform.position;
        TargetPos = OrPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref z, damper);
        unmove();
    }
    public void move()
    {
        TargetPos.y = Mathf.Max(TargetPos.y-.2f, OrPos.y-(10-DesiredHeight));
    }
    public void unmove()
    {
        TargetPos.y = Mathf.Min(TargetPos.y + .1f, OrPos.y);
    }
    public void ResetTargetPos()
    {
        TargetPos = OrPos;
    }
}
