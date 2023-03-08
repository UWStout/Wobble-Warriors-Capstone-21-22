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
    [SerializeField] float damper = .25f;
    float shakeTime = 0f;
    float shakeStrength = .7f;
    [SerializeField]
    float ShakeStrengthExternalModifier = 1f;
    bool widenFOV = false;
    bool accelFOV = false;
    [SerializeField]
    float MinimumFOV = 30f;
    [SerializeField]
    float MaximumFOV = 120f;
    [SerializeField]
    float FovVelocity = 1.01f;
    [SerializeField]
    float ScreenBoundary = .3f;
    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dir = GameObject.Find("Director");
        director = dir.GetComponent<Director>();
        director.setCamera(this);
        CurrentTime = MaxTime;
        Camera = this.gameObject.GetComponent<Camera>();
        here = this.transform.position;
        there = here;
    }

    // Update is called once per frame
    void Update()
    {
        widenFOV = false;
        accelFOV = false;
        ///Move smoothly to player center
        //Find and store center point of all players
        t = DynamicTarget();
        //use SmoothDamp to move smoothly towards player cente
        this.transform.position = Vector3.SmoothDamp(this.transform.position, t, ref z, damper);

        ///Check if there's walls in the way of any players or enemies
        //find the closest moving object to the camera aka the wall
        Vector3 closest = GetClosestPositionAndCheckScreenSpace();
        //Generate a ray from the camera to that object
        Ray r = new Ray(transform.position, closest - this.transform.position);
        //Store every object in that raycast in hits
        RaycastHit[] hits= Physics.RaycastAll(r,(closest - this.transform.position).magnitude);
        //on a hit, if the hit object has the Scoot component, tell it to move.
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

        ///Camera Shake
        //Shake the camera if necessary
        if (shakeTime > 0)
        {
            //shake the camera
            //Debug.Log("I should be shaking");
            this.transform.localPosition += Random.insideUnitSphere * shakeStrength * ShakeStrengthExternalModifier;

            shakeTime-=Time.deltaTime;
        }
        //reset the shake timer to 0 if below
        else
        {
            shakeTime = 0f;
        }
        float desiredFOV = Camera.fieldOfView;
        ///Adjust Camera FOV to fit all players comfortably
        if (widenFOV)
        {
            if (accelFOV)
            {
                desiredFOV *= FovVelocity;
            }
            else
            {
                desiredFOV *= (FovVelocity * FovVelocity);
            }
        }
        else
        {
            if (accelFOV)
            {
                desiredFOV /= FovVelocity;
            }
            else
            {
                desiredFOV /= (FovVelocity * FovVelocity);
            }
        }
        desiredFOV = Mathf.Clamp(desiredFOV, MinimumFOV, MaximumFOV);
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, desiredFOV, .3f) ;
    }
    //Returns the center of the players
    Vector3 DynamicTarget()
    {
        return director.GetPlayerCenter()+CameraOffset;
    }
    //Returns the position vector of the player or enemy closest to the camera
    Vector3 GetClosestPositionAndCheckScreenSpace()
    {
        //Use the player center as a failsafe vector to avoid funky rays
        Vector3 closest = director.GetPlayerCenter();
        //Check the distance of every player from the camera
        foreach (GameObject p in director.PlayerList)
        {
            Vector3 screenPos = Camera.WorldToScreenPoint(p.transform.position);
            if (screenPos.x < 0 || screenPos.y < 0 || screenPos.x > Camera.pixelWidth || screenPos.y > Camera.pixelHeight)
            {
                //Debug.Log("oh no");
                widenFOV = true;
                accelFOV = true;
            }
            if  (   screenPos.x < Camera.pixelWidth*ScreenBoundary
                ||  screenPos.x > Camera.pixelWidth-(Camera.pixelWidth * ScreenBoundary)
                ||  screenPos.y < Camera.pixelHeight*ScreenBoundary
                ||  screenPos.y > Camera.pixelHeight-(Camera.pixelHeight*ScreenBoundary))
            {
                //Debug.Log("Player is at (" + screenPos.x + ", " + screenPos.y + ")");
                widenFOV = true;
            }
            if ((closest - this.transform.position).magnitude > (p.transform.position - this.transform.position).magnitude)
            {
                //overwrite closest if there is a closer player
                closest = p.transform.position;
            }
        }
        //Check the distance of every enemy from the camera
        /*
        if (director.GetGobs()!=null&&director.GetGobs().Count>0)
        {
            foreach (GameObject g in director.GetGobs())
            {
                Vector3 screenPos = Camera.WorldToScreenPoint(g.transform.position);
                if (screenPos.x < 0 || screenPos.y < 0 || screenPos.x > Camera.pixelWidth || screenPos.y > Camera.pixelHeight)
                {
                    widenFOV = true;
                    accelFOV = true;
                }
                if (screenPos.x < Camera.pixelWidth * ScreenBoundary
                    || screenPos.x > Camera.pixelWidth - (Camera.pixelWidth * ScreenBoundary)
                    || screenPos.y < Camera.pixelHeight * ScreenBoundary
                    || screenPos.y > Camera.pixelHeight - (Camera.pixelHeight * ScreenBoundary))
                {
                    widenFOV = true;
                }
                if ((closest - this.transform.position).magnitude > (g.transform.position - this.transform.position).magnitude)
                {
                    //overwrite closest if there is a closer enemy
                    closest = g.transform.position;
                }
            }
        }
        */
        //V
        return closest;
    }
    //Public function to set shake duration and severity
    public void ShakeCam(float duration, float severity)
    {
        //shake
        Debug.Log("shake");
        shakeTime = duration;
        shakeStrength = severity;
    }
}
