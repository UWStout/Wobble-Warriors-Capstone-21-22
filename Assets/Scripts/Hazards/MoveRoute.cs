using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoute : MonoBehaviour
{

    [SerializeField] private Transform[] route;//array of gameobjects that the gameobject moves between

    [SerializeField] private float speed = 3;//determines move speed per frame, keep low

    [SerializeField] private float RotationSpeed = 1.0f;//determines turn speed 

    [SerializeField] private bool DeleteAfterRoute = false;//if true after the object completes it's route once it is deleted

    [SerializeField] private GameObject self = null;//stores the gameobject that will be deleted after the route is done.

    [SerializeField] private bool RotateDuringRoute = false;

    [SerializeField] private float ErrorMarginRangeToPoint = 0.5f;

    [SerializeField] private bool MoveForward = false;

    private float RotationRamp = 0;
    private int TargetIndex; //used for initialization


    //public HazardKnockBack HKnockback = null;


    public float getSpeed()
    {
        return speed;
    }



    public bool getRouteDelete()
    {
        return DeleteAfterRoute;
    }

    public bool getRotateRoute()
    {
        return RotateDuringRoute;
    }

    public float getRotationSpeed()
    {
        return RotationSpeed;
    }

    public GameObject getSelf()
    {
        return self;
    }

    public float getRRamp()
    {
        return RotationRamp;
    }

    public float getErrorMarginToPoint()
    {
        return ErrorMarginRangeToPoint;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (route.Length < 0)
        {
            Debug.Log("Route has negative size. this is a problem");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        if (route.Length>0)
        {
            float distanceToTarget = Vector3.Distance(transform.position, route[TargetIndex].position);
           if(distanceToTarget > ErrorMarginRangeToPoint)
            {//rotates using the look at function




                if (RotateDuringRoute)
                {//if objects rotate during route they will move only in the direction they are facing and turn toward the next route cordinate. When magnitude is less then between target and current position, the object moves toward the next point in the route.
                    
                    transform.rotation = lookAtSlowly(transform, route[TargetIndex].position, RotationSpeed + RotationRamp);
                    RotationRamp = RotationRamp + Time.deltaTime/2;
                    transform.position = (transform.position + transform.forward*speed * Time.deltaTime);
                    
                }
                else if(MoveForward)
                {
                    transform.rotation = lookAtSlowly(transform, route[TargetIndex].position, RotationSpeed + RotationRamp* Time.deltaTime);
                    Vector3 pos = Vector3.MoveTowards(transform.position, route[TargetIndex].position, speed);
                    transform.position = pos;//moves toward target location
                }
                else
                {
                    //finds the position to move towards each frame
                    Vector3 pos = Vector3.MoveTowards(transform.position, route[TargetIndex].position, speed* Time.deltaTime);
                    transform.position = pos;//moves toward target location
                }
                
            }
            else
            {
                if (DeleteAfterRoute)
                {
                    Destroy(self);
                }
                RotationRamp = 0;
                TargetIndex = (TargetIndex + 1) % route.Length;//iterates current but if current is equal to the length of the route away, current is set to 0
            }
        }

    }
    //returns the target vector that is a rotation toward a transform using a objects current transform and target's transform
    private Vector3 LookAt(Transform position, Transform target)
    {
        //determines direction to look at
        Vector3 targetDir = position.position - transform.position;

        float singleStep = RotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);

        
        return newDirection;
    }

    private Quaternion lookAtSlowly(Transform t, Vector3 target , float speed)
    {
        /*(t) is the gameobject transform
         * (target) is the location that (t) looks at
         * speed is how fast the rotation occurs
         */

        Vector3 relativePos = target - t.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        return Quaternion.Lerp(t.rotation, toRotation, speed * Time.deltaTime);
    }
}


