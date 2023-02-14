using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ModularDoor : MonoBehaviour
{
    //Bryce Niles
    //Code for modular doors
    //Assumes rooms follow the naming convention "Room_ROW_COLUMN", (ex, start room is named Room_0_0) and that this door is a direct child of such a room.
    //also assumes doors have a complementary door in associated room named based on their cardinal orientation in caps (Doors that lead west are named "WEST", etc.).
    // Start is called before the first frame update

    //Room offset for camera movement
    Vector3 CameraOffset = new Vector3(-2.76999998f, 10.8999996f, -11.1199999f);
    
    //Container object for door
    Transform MyDoor=null;
    
    //Script containing room coordinates
    RoomInfo MyRoom=null;
    
    //Container object for associated door
    Transform TargetDoor=null;
    

    //container object for associated room
    GameObject TargetRoom=null;
    
    //The camera
    private Cam Camera=null;
    
    //Backend object containing player list etc.
    Director Director=null;
    
    //Container object for glowing orbs around door
    [SerializeField]
    List<VisualEffect> IndicatorList = new List<VisualEffect>();
    int[] mdColors = { -2, -2, -2, -2 };
    
    [SerializeField]
    List<AudioClip> SoundList = new List<AudioClip>();
    
    //If door is east or west, this is true
    bool horizontal=false;
    
    //Number of players standing on door plate
    int standing=0;
    
    //Total number of player characters in the scene
    int total=0;
    
    //List of players (from Director)
    public List<GameObject> PlayerList = new List<GameObject>();

    //Time decrement
    private float WaitTime=1.35f;
    [SerializeField] bool keyLocked = false;

    bool VoidDoor=false;
    //The thing that starts stuff
    void Start()
    {
        //Setting variables and objects from the scene that don't exist before runtime.
        if (GameObject.Find("Director"))
        {
            Director = GameObject.Find("Director").GetComponent<Director>();
        }
        total = Director.PlayerList.Count;
        PlayerList = Director.PlayerList;
        GameObject temp = GameObject.Find("Main Camera");
        if (temp) {
            Camera = temp.GetComponent<Cam>();
        }
        MyDoor = transform.parent.parent;
        if (MyDoor)
        {
            MyRoom = MyDoor.parent.GetComponent<RoomInfo>();
        }
        TargetDoor = ComplementDoor(MyRoom.row, MyRoom.column, MyDoor.name);
    }
    // Update is called once per frame
    void Update()
    {
        if (VoidDoor)
        {
            MyDoor.gameObject.SetActive(false);

        }
        //If at least 1 character exists
        if (total > 0)
        {
            float ready = (float)standing / (float)PlayerList.Count;
            //if more than half of all players are standing in front of a door
            if (ready > .5f)
            {
                //If players have been in front of the door for at least n second
                if (WaitTime < 0)
                {
                    //teleport to associated door
                    Teleport();
                    //reset timer for teleportation
                    WaitTime = 1.35f;
                }
                //count down to 0 in seconds
                else { WaitTime -= Time.deltaTime; }
            }
            //If there aren't enough players using the door, reset the timer
            else
            {
                WaitTime = 1.35f;
            }
        }
    }
    //Unity function called when collider enters trigger box
    void OnTriggerEnter(Collider other)
    {
        //Refresh list of players
        PlayerList = Director.PlayerList;
        //refresh number of players
        total = Director.PlayerList.Count;
        //Check if collider is a player's ragdoll
        PlayerCharacter boi =  other.GetComponent<PlayerCharacter>();
        if(boi != null){
            //increment number of players standing in front of this door
            standing++;
            //Enable associated player's teleport indicator
            Indicator(boi.playerNumber, true);
        }
        //Debug.Log(standing + " of " + total);

    }
    //Unity function called when collider leaves trigger box
    void OnTriggerExit(Collider other)
    {
        //Check if collider is a player's ragdoll
        PlayerCharacter boi = other.GetComponent<PlayerCharacter>();//Stores the leaving object's PlayerCharacter script. Intentionally filters out non-player objects
        if (boi != null)//If a player
        {
            standing--;//Decrement players standing on spot
            Indicator(boi.playerNumber, false);//turn off their indicator
        }

    }

    //Toggles a visual effect based on player ID and player color, and plays a corresponding sound effect
    void Indicator(int player, bool enabled)
    {
        //Check if the player ID is valid i.e. from 0 to 3, and that the door is not locked
        if (player >= 0 && player < 4 && !keyLocked)
        {
            //Get the correct color based on the color of the player
            int rightColor = ReadyUpPlayer.colors[player];
            //If toggling on and can be toggled on
            if (enabled && Director.CanTeleport)
            {
                //Failsafe for checking that the indicator matches the color of the dragon
                if (mdColors[player] != rightColor)
                {
                    //Change the color if it's wrong
                    updateIndicator(player, rightColor);
                }
                //Enable the effect
                IndicatorList[player].SendEvent("Start");

                if (SoundList[player])
                {
                    //Play a sound based on player number
                    GetComponent<AudioSource>().PlayOneShot(SoundList[player]);
                }
            }
            //If toggling the effect off
            else
            {
                //Disable the vfx of the corresponding player in the array
                IndicatorList[player].SendEvent("Stop");
            }
        }
        else
        {
            //Debug Statement
            Debug.Log("Error. Requested player number was " + player + ".");
        }
    }
    //Method that finds a door in an adjacent room, based on the name of the door and the room coordinates
    Transform ComplementDoor(int row, int column, string cardinality)
    {
        //If door faces east, look 1 room to the right for west facing door.
        if (cardinality == "EAST")
        {
            //Door faces east/west
            horizontal = true;
            //Find adjacent room to the east
            TargetRoom=GameObject.Find("Room_" + row + "_" + (column+1));
            if(TargetRoom!=null){
                //find western door in eastern room
                return TargetRoom.transform.Find("WEST");
            }
            //Set teleport to own door if a room is not found to prevent errors
            else{
                TargetRoom = MyDoor.parent.gameObject;
                //Debug.Log("EAST CHECK: Could Not Find room "+row+"_"+(column+1));
                VoidDoor = true;
                return MyDoor;
            }
        }
        //If door faces north, look 1 room forward for south facing door.
        else if (cardinality == "NORTH")
        {
            //Door faces north/south
            horizontal = false;
            //find adjacent room to the north
            TargetRoom=GameObject.Find("Room_" + (row+1) + "_" + column);
            if(TargetRoom!=null){
                //find southern door in northern room
                return TargetRoom.transform.Find("SOUTH");
            }
            //Set teleport to own door if a room is not found to prevent errors
            else
            {
                TargetRoom = MyDoor.parent.gameObject;
                //Debug.Log("NORTH CHECK: Could Not Find room "+(row+1)+"_"+column);
                VoidDoor = true;
                return MyDoor;
            }
        }
        //If door faces west, look 1 room left for east facing door.
        else if (cardinality == "WEST")
        {
            //Door faces east/west
            horizontal = true;
            TargetRoom=GameObject.Find("Room_" + (row) + "_" + (column-1));
            if(TargetRoom!=null){
                return TargetRoom.transform.Find("EAST");
            }
            //Set teleport to own door if a room is not found to prevent errors
            else
            {
                TargetRoom = MyDoor.parent.gameObject;
                //Debug.Log("WEST CHECK: Could Not Find room "+(row)+"_"+(column-1));
                VoidDoor = true;
                return MyDoor;
            }
        }
        //If door faces south, look 1 room backward for north facing door.
        else if (cardinality == "SOUTH")
        {
            //Door faces south/north
            TargetRoom=GameObject.Find("Room_" + (row-1) + "_" + (column));
            if(TargetRoom!=null){
                return TargetRoom.transform.Find("NORTH");
            }
            //Set teleport to own door if a room is not found to prevent errors
            else
            {
                TargetRoom = MyDoor.parent.gameObject;
                //Debug.Log("SOUTH CHECK: Could Not Find room "+(row-1)+"_"+(column));
                VoidDoor = true;
                return MyDoor;
            }
            
        }
        //if door is incorrectly named, teleport destination is set to own door to prevent errors.
        else
        {
            TargetRoom = MyDoor.parent.gameObject;
            VoidDoor = true;
            return MyDoor;
        }
    }
    //Function that teleports all players to slightly different locations in the room
    void Teleport()
    {
        //Debug.Log("Teleporting");
        //if room has no enemies remaining
        if (Director.CanTeleport && !keyLocked)
        {
            RoomInfo t=TargetRoom.GetComponent<RoomInfo>();
            RoomInfo m = MyRoom.GetComponent<RoomInfo>();
            if (TargetRoom != MyRoom)
            {
                t.DoAToggle(0.0f, true);
                m.setRoomHazards(false);
                m.DoAToggle(1.0f, false);
            }
            //teleport each player to a slightly different location in front of the target door
            foreach (GameObject boi in Director.PlayerList)
            {
                PlayerCharacter boiBrain = boi.GetComponentInChildren<PlayerCharacter>();
                boiBrain.Revive(false);
                //if door faces horizontally, vary player position on Z-axis
                if (horizontal)
                {
                    boi.transform.SetPositionAndRotation(TargetDoor.position + new Vector3(0, 1f, boi.GetComponent<PlayerCharacter>().playerNumber), boi.transform.rotation);
                }
                //if door faces vertically, vary player position on X-axis
                else
                {
                    boi.transform.SetPositionAndRotation(TargetDoor.position + new Vector3(boi.GetComponent<PlayerCharacter>().playerNumber, 1f, 0), boi.transform.rotation);
                }
            }
            //Disable all player indicators on door. Might be unnecessary
            for (int i = 0; i < PlayerList.Count; i++)
            {
                Indicator(i, false);
            }
            Director.SetDoOver(TargetDoor.GetComponentInChildren<ModularDoor>());
            //Set current room to new room
            Director.SetCurrentRoom(TargetRoom);
            //Enable enemies in new room
            Director.ActivateRoom();
        }
    }
    //used to update the color of the door indicator
    void updateIndicator(int playerNum, int color)
    {
        //Store the component of the two indicators
        VisualEffect tempEffect = IndicatorList[playerNum];
        VisualEffect newEffect = IndicatorList[color];
        //Swap their positions and indices
        IndicatorList[playerNum] = newEffect;
        IndicatorList[playerNum].transform.position = newEffect.transform.position;
        IndicatorList[color] = tempEffect;
        IndicatorList[color].transform.position = tempEffect.transform.position;
        mdColors[playerNum] = color;
    }
    //Called when players use an extra life
    public void DoOver()
    {
        //Teleport players back to the last room they completed
        Teleport();
        //Revive the players
        foreach(GameObject boi in Director.PlayerList)
        {
            boi.GetComponent<ActiveRagdoll>().RagdollForDuration(0.0f);
        }
    }
    //Called when a door is hit by a key
    public void keyHit()
    {
        //Set locked to false
        keyLocked = false;
    }


    //getter for the Vector3 that says how far from the center of the room that the main camera is supposed to be
    public Vector3 getCameraOffset()
    {
        return CameraOffset;
    }
    //Getter for the transform of the outermost parent model of the door
    public Transform getMyDoor()
    {
        return MyDoor;
    }
    //Getter for the roominfo script attached to this door's room. Contains the coordinates for the room.
    public RoomInfo getRoomInfo()
    {
        return MyRoom;
    }
    //Getter for the transform of the outermost parent model of the target door
    public Transform getTargetDoor()
    {
        return TargetDoor;
    }
    //Getter for the transform of the target door's room. Contains the coordinates for the room.
    public GameObject getTargetRoom()
    {
        return TargetRoom;
    }
    //Getter for the main camera, which is a sibling of the doors in the current room
    public Cam getCamera()
    {
        return Camera;
    }
    //Getter for the director. Contains the list of players
    public Director getDirector()
    {
        return Director;
    }
    //Getter for the list of Indicators
    public List<VisualEffect> getIndicators()
    {
        return IndicatorList;
    }
    //Getter for the list of chime clips that play when approached by dragons
    public List<AudioClip> getSoundList()
    {
        return SoundList;
    }
    //Getter for the boolean associated with whether the door faces east/west or north/south
    public bool isHorizontal()
    {
        return horizontal;
    }
    //Returns the number of players who are currently standing in front of this door
    public int getPlayersOnDoor()
    {
        return standing;
    }
    //Returns the total number of players in the scene
    public int getPlayerCount()
    {
        return total;
    }
    //Returns whether this door needs to be opened with a key.
    public bool isLockedDoor()
    {
        return keyLocked;
    }
}
