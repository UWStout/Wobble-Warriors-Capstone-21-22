using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DESCRIPTION: used to determine if an interactable or pickup is nearby
//AUTHOR: Brian Bauch
public class InteractRadius : MonoBehaviour
{
    public bool interactable = false; //tells the weapon swap script whether an interaction can happen or not
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //if the interact radius collides with something
    private void OnTriggerEnter(Collider other)
    {
        //if the collided object has the "Weapon" tag AND interactable is currently false
        if(other.CompareTag("Weapon") && !interactable)
        {
            //set interactable to true and set the pickup as the collided object
            interactable = true;
            transform.parent.gameObject.GetComponent<WeaponSwap>().SetPickup(other.gameObject);
        }
    }

    //once the interact stops colliding with something
    private void OnTriggerExit(Collider other)
    {
        //if the collided object has the "Weapon" tag
        if (other.CompareTag("Weapon"))
        {
            //set interactable to false
            interactable =  false;
        }
    }
}
