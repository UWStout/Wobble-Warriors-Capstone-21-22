using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capstan : MonoBehaviour
{

    [HideInInspector] public float RotationSpeed = 1;//animation speed randomized while running every seconds that are randomized between 10-20 seconds
    private int timeChange;

    [SerializeField] private Animator myRotator = null;

    [SerializeField] private string ORotator = "CapRotation";

    [SerializeField] private float MinSpeed = 0.5f;//min speed of capstan rotation
    [SerializeField] private float MaxSpeed = 1.5f;//max speed of capstan rotation

    [SerializeField] private int MinSpeedChangeInterval = 10;//min time before capstan speed changes
    [SerializeField] private int MaxSpeedChangeInterval = 20;//max time before capstan speed changes

    public float getMinSpeed()
    {
        return MinSpeed;
    }

    public float getMaxSpeed()
    {
        return MaxSpeed;
    }

    public int getMinSpeedInterval()
    {
        return MinSpeedChangeInterval;
    }

    public int getMaxSpeedInterval()
    {
        return MaxSpeedChangeInterval;
    }

    public string getCapstanAnimator()
    {
        return ORotator;
    }





    // Start is called before the first frame update
    void Start()
    {//starts the rotation of the capstan
    }

    void OnEnable()
    {
        if (myRotator == null)
        {
            Debug.Log("missing capstan animator");
        }



        StartCoroutine(rotator());
        StartCoroutine(changeSpeed());
    }

   

    void FixedUpdate()
    {//starts the coroutine to change the rotation speed of the capstan
        
    }

    private IEnumerator rotator()
    {//runs constantly

            myRotator.Play(ORotator, 0, 0.0f);//animation that rotates capstan
            yield return new WaitForSeconds(0);
    }

    private IEnumerator changeSpeed()//changes the speed of the rotation animation within the range of 1/2 speed and 2* speed This ocurs randomly with the range of 10-20 seconds
    {
        while (true)
        {//sets speed equal to a random float between 0 and 2
            RotationSpeed = Random.Range(MinSpeed, MaxSpeed);
            timeChange = Random.Range(MinSpeedChangeInterval, MaxSpeedChangeInterval);//sets timechange randomly so speed is randomized every 10-20 seconds
            myRotator.speed = RotationSpeed;//changes speed of animation
            yield return new WaitForSeconds(timeChange);//time to wait before speed is randomizes
            
        } 
    }
}
