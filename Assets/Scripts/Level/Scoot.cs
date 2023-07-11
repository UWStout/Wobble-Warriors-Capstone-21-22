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
    Director dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = GameObject.Find("Director").GetComponent<Director>();
        OrPos = transform.position;
        TargetPos = OrPos;
        if (up)
        {
            TargetPos.y += Distance;
        }
        else
        {
            TargetPos.y -= Distance;
        }
        
    }
    private void Update()
    {
        if (dir.GetPlayerCenter().z > this.transform.position.z)
        {
            SetInTheWay();
        }
    }
    // Update is called once per frame
    void LateUpdate()
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
        if (InTheWay)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPos, dt/MoveDuration);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, OrPos, 1-dt/MoveDuration);
        }
        InTheWay = false;
    }
    public void SetInTheWay()
    {
        InTheWay = true;
    }
}
