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
    float camz = 0f;
    Vector3 t;
    float MaxTime = .5f;
    float CurrentTime;
    Director director;
    [SerializeField] float damper = .25f;
    [SerializeField] float camdamper = .25f;
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
    float FovVelocity = 1.05f;
    float currentFieldOfViewVelocity;
    [SerializeField]
    float ScreenCenterRadius = .3f;
    Camera Camera;
    float desiredFOV;
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
        currentFieldOfViewVelocity = FovVelocity;
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
        RaycastHit[] hits= Physics.RaycastAll(r,(closest - this.transform.position).magnitude, -1, QueryTriggerInteraction.Collide);
        //on a hit, if the hit object has the Scoot component, tell it to move.
        if(hits.Length>0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.GetComponent<Scoot>())
                {
                    Scoot s = hit.transform.GetComponent<Scoot>();
                    s.SetInTheWay();
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
        
        ///Adjust Camera FOV to fit all players comfortably
        desiredFOV = Mathf.Clamp(desiredFOV, MinimumFOV, MaximumFOV);
        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, desiredFOV, ref camz, camdamper);
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
        desiredFOV = Camera.fieldOfView;
        float accel = -Mathf.Infinity;
        Debug.Log(director.PlayerList.Count+" Players");
        Debug.Log(director.GetGobs().Count + " Gobs");
        foreach (GameObject p in director.PlayerList)
        {
            GameObject q = p.GetComponentInChildren<CharacterMovement>().gameObject;
            Vector3 screenPos = Camera.WorldToScreenPoint(q.transform.position);
            //get the distance of the player from the center of the screen
            float distance;
            //x axis
            distance = Mathf.Abs( (screenPos.x / Camera.pixelWidth) - .5f );
            //y axis
            distance = Mathf.Max(Mathf.Abs( (screenPos.y / Camera.pixelHeight) - .5f), distance);
            //Debug.Log("Player " + p.name + " is this far away from the center: " + distance);
            //Account for the margin
            distance -= ScreenCenterRadius;
            //overwrite with max value
            accel = Mathf.Max(accel, distance);
            
        }
        foreach (GameObject p in director.GetGobs())
        {
            if (p.activeSelf)
            {
                GameObject q = p.GetComponentInChildren<CharacterMovement>().gameObject;
                Vector3 screenPos = Camera.WorldToScreenPoint(q.transform.position);
                //get the distance of the player from the center of the screen
                float distance;
                //x axis
                distance = Mathf.Abs((screenPos.x / Camera.pixelWidth) - .5f);
                //y axis
                distance = Mathf.Max(Mathf.Abs((screenPos.y / Camera.pixelHeight) - .5f), distance);
                //Debug.Log("Player " + p.name + " is this far away from the center: " + distance);
                //Account for the margin
                distance -= ScreenCenterRadius;
                //overwrite with max value
                accel = Mathf.Max(accel, distance);
            }
        }
        desiredFOV = (Camera.fieldOfView + accel * 10 * FovVelocity);
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
