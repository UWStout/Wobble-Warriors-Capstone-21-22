using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyor : MonoBehaviour
{

    [SerializeField] private float speed;//speed of object on conveyor belt

    private float speedScalar = 100;
    //[SerializeField] private float visualSpeedScalar;

    //use this above variable for animation scaling

    private Vector3 direction;//direction of movement on conveyor
    private float currentScroll;//current speed of animation



    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if(enabled == true)
        {
            if (collision.gameObject.GetComponent<ConfigurableJoint>())
            {
                //Get direction of the conveyor belt, the player will move in that direction while on the conveyor belt
                direction = transform.forward;
                direction *= speed * speedScalar;

                //moves the character at a set speed along the conveyor belt
                Transform characterCenter = collision.transform.GetComponentInParent<ActiveRagdoll>().getRoot();//gets the center of the character on the conveyor belt
                characterCenter.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);//moves the character at a set speed to in the set direction at a fixed rate

            }
        }
        
    }

    public float getSpeedScalar() 
    {
        return speedScalar;
    }



    public float getSpeed()
    {
        return speed;
    }

}
