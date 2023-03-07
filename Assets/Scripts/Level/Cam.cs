using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]
    Vector3 CameraOffset = new Vector3(0f, 10.8999996f, -11.1199999f);
    Vector3 here;
    Vector3 there;
    Vector3 z = Vector3.zero;
    Vector3 t;
    float MaxTime = .5f;
    float CurrentTime;
    Director director;
    [SerializeField] float damper = .1f;
    float shakeTime = 0f;
    float shakeStrength = .7f;
    [SerializeField]
    float ShakeStrengthExternalModifier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dir = GameObject.Find("Director");
        director = dir.GetComponent<Director>();
        director.setCamera(this);
        CurrentTime = MaxTime;

        here = this.transform.position;
        there = here;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving to center looking at average of players
        t = DynamicTarget();
        this.transform.position = Vector3.SmoothDamp(this.transform.position, t, ref z, damper);
        //Look at center of players
        //this.transform.LookAt(director.GetPlayerCenter());
        //Check if there's a wall in the way of any players or gobs
        Vector3 closest = GetClosestPosition();
        Ray r = new Ray(transform.position, closest - this.transform.position);
        RaycastHit[] hits= Physics.RaycastAll(r,(closest - this.transform.position).magnitude);
        if(hits.Length>0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.GetComponent<Scoot>())
                {
                    Scoot s = hit.transform.GetComponent<Scoot>();
                    s.move();
                }
            }
        }
        //Camera Shake
        if (shakeTime > 0)
        {
            //shake the camera
            Debug.Log("I should be shaking");
            this.transform.localPosition += Random.insideUnitSphere * shakeStrength * ShakeStrengthExternalModifier;

            shakeTime-=Time.deltaTime;
        }
        else
        {
            //shakeTime = 0f;
        }

    }
    Vector3 DynamicTarget()
    {
        return director.GetPlayerCenter()+CameraOffset;
    }
    Vector3 GetClosestPosition()
    {
        Vector3 closest = director.GetPlayerCenter();
        foreach(GameObject p in director.PlayerList)
        {
            if ((closest - this.transform.position).magnitude > (p.transform.position - this.transform.position).magnitude)
            {
                closest = p.transform.position;
            }
        }
        if (director.GetGobs().Count>0)
        {
            foreach (GameObject g in director.GetGobs())
            {
                if ((closest - this.transform.position).magnitude > (g.transform.position - this.transform.position).magnitude)
                {
                    closest = g.transform.position;
                }
            }
        }
        return closest;
    }
    public void ShakeCam(float duration, float severity)
    {
        //shake
        Debug.Log("shake");
        shakeTime = duration;
        shakeStrength = severity;
    }
}
