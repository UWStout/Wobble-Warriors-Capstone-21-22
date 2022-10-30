using System.Collections;
using System.Collections.Generic;
//Bryce Niles
using UnityEngine;
//Script designed to return displaced characters to the level.
//Goblins are returned to their respective rooms
//Players are returned to the current room
public class KillPlane : MonoBehaviour
{
    //Director for finding current room
    Director Director;
    //Room associated with this object assigned in editor optionally
    public Transform Room;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch director on startup
        Director = GameObject.Find("Director").GetComponent<Director>();
    }
    //OnTriggerEnter is called when a collider enters this object's collider
    private void OnTriggerEnter(Collider other)
    {
        //Get the script of the colliding object if it has one.
        CharacterMovement boi = other.GetComponent<CharacterMovement>();
        //If collided object has a charactermovement script then it is a character and needs to be teleported back
        //if this object has no associated room, teleport the character to the current room
        if(boi != null){
            if (Room != null)
            {
                boi.transform.position = Room.transform.position;
            }
            else
            {
                boi.transform.position = Director.CurrentRoomPosition();
            }
        }
    }
}
