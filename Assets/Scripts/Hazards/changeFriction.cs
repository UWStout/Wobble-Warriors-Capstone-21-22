using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeFriction : MonoBehaviour
{
    private float Dfriction = 1;
    private float Sfriction = 1;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<ConfigurableJoint>())//if the object in the collider is a character then sets their dynamic and static friction to 0
        {
            if (collider.material)
            {

                Dfriction = collider.material.dynamicFriction;
                Sfriction = collider.material.staticFriction;

                collider.material.dynamicFriction = 0;
                collider.material.staticFriction = 0;
            }
            
        }
    }

    public float getDfriction()
    {
        return Dfriction;
    }

    public float getSfriction()
    {
        return Sfriction;
    }



    private void OnTriggerExit(Collider collider)//when a object leaves the collider their friction is set back to what it was before they left
    {
        if (collider.gameObject.GetComponent<ConfigurableJoint>())
        {
            if (collider.material)
            {
                collider.material.dynamicFriction = Dfriction;
                collider.material.staticFriction = Sfriction;
            }

        }
    }
}
