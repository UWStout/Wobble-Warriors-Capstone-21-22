using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whacker : Hazard
{
    [SerializeField] bool WallVariant = false;
    [SerializeField] bool CounterClockwise = false;

    [SerializeField] float ExtendAngle=70.0f;
    [SerializeField, Range(0.0f, 5f), Tooltip("How long it takes for the whacker to extend")] 
    float ExtendTime = .25f; /*How long it takes for the whacker to extend*/
    [SerializeField, Range(0.0f, 5f), Tooltip("How long the whacker stays extended")] 
    float HangTime = .5f; /*How long the whacker stays extended*/
    [SerializeField, Range(0.0f, 5f), Tooltip("How long it takes for the whacker to return to neutral")] 
    float RetractTime = 1.0f; /*How long it takes for the whacker to return to neutral*/
    [SerializeField, Range(0.0f, 5f), Tooltip("How long before the Whacker can be reactivated after returning to neutral")] 
    float DownTime = 1.0f; /*How long before the Whacker can be reactivated after returning to neutral*/
    
    bool triggered = false;
    Vector3 initialRotation;
    float currentTime;
    float duration = Mathf.Infinity;
    int state; //States are 0: Inactive 1:Extending 2:Hanging 3:Retracting
    Vector3 ExtendedRotation;

    void Start()
    {
        initialRotation = transform.eulerAngles;
        if (WallVariant)
        {
            if (CounterClockwise)
            {
                ExtendedRotation = initialRotation + (new Vector3(0, -ExtendAngle, 0));
            }
            else
            {
                ExtendedRotation = initialRotation + (new Vector3(0, ExtendAngle, 0));
            }
        }
        else
        {
            ExtendedRotation = initialRotation + (new Vector3(ExtendAngle, 0, 0));
        }
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime > duration||triggered) {
            triggered = false;
            currentTime = 0;
            state = (state + 1) % 4;
            if (state == 1)
            {
                kbMod = 1;
            }
            duration = ResetDuration();
        }
        if (state == 1)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation, ExtendedRotation, currentTime / duration));
        }
        if (state == 3)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(ExtendedRotation, initialRotation, currentTime / duration));
        }
    }
    //Resets the duration based on current state.
    float ResetDuration()
    {
        if(state == 1)
        {
            return ExtendTime;
        }
        else if (state == 2)
        {
            return HangTime;
        }
        else if (state == 3)
        {
            return RetractTime;
        }
        else
        {
            return Mathf.Infinity;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        ActiveRagdoll f = other.GetComponentInParent<ActiveRagdoll>();
        if (f != null&&state==0&&currentTime>DownTime&&(other.tag=="Player"||other.tag=="Enemy"))
        {
            triggered = true;
        }
    }
}
