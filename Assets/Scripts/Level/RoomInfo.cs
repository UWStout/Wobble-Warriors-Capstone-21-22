using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    //Internal indeces for mapping level progression
    public int row=0;
    public int column=0;
    
    //bool CanTeleport=false;
    //True if all enemies have been defeated
    [SerializeField ]bool cleared = false;
    //Reference for director is assigned on awake
    Director Director;
    //Reference for list of players is assigned on awake
    List<GameObject> PlayerList;
    //Maximum number of enemies that can be spawned into the room
    [SerializeField] int MaxGobs=8;
    //Reference to the spawn point/parent object where enemies are spawned, is attached in inspector. This is where spawned enemies are parented to and positioned relative to.
    [SerializeField] GameObject GobStopper;
    //Reference to enemy object. This is what is spawned when spawn gob is called
    [SerializeField] GameObject EnemyCharacter;
    //Vector 3 set in editor. This is how far away an enemy can be from a room before it is teleoprted to the GobStopper's position
    [SerializeField] Vector3 leash= new Vector3(0.0f, 0.0f, 0.0f);
    //Float set in editor. This is The maximum distance in the x and z axes from the Gobstopper that an enemy can be spawned
    [SerializeField] float SpawnRadius = 5.0f;
    public bool isBossRoom = false;

    [SerializeField] private List<DisableHazards> HazardList;

    private void Start()
    {
        //Disabling hazards when the level loads
        setRoomHazards(false);
        if (transform.GetComponent<StartingRoom>() == null)
        {
            DoAToggle(1.0f, false);
        }
    }
    void Awake()
    {
        //Get the director here, It doesn't exist in the scene outside of runtime so you can't assign it in editor
        Director = GameObject.Find("Director").GetComponent<Director>();
        //Get the list of players from the director
        PlayerList = Director.PlayerList;
        


    }
    // Update is called once per frame
    void Update()
    {
        //If a enemy or player gets too far away, this should teleport them back to the center of the room
        if (Director.GetCurrentRoom() == this.gameObject)
        {
            //Get the transform of the gobstopper, i.e. the point where things get teleported to
            Transform gsTransform = GobStopper.transform;
            //iterate through enemies
            for (int i = 0; i < GobStopper.transform.childCount; i++)
            {
                //get the enemy's transform
                Transform gob = GobStopper.transform.GetChild(i).GetComponent<ActiveRagdoll>().getRoot();
                //if the enemy is too far away from the room, bring it back
                if (gob!=null && (Mathf.Abs(gob.position.x - GobStopper.transform.position.x) > leash.x || Mathf.Abs(gob.position.y - GobStopper.transform.position.y) > leash.y || Mathf.Abs(gob.position.z - GobStopper.transform.position.z) > leash.z))
                {
                    //Setting the enemy's position equal to the gobstopper's position, which is supposed to be the center of the room
                    gob.transform.position = gsTransform.position;
                }
            }
            //iterate through players
            for (int i = 0; i < PlayerList.Count; i++)
            {
                //same process as enemies, but players are stored directly, rather than being gotten from a script
                Transform boi = Director.PlayerList[i].transform;
                if (boi != null && (Mathf.Abs(boi.position.x - GobStopper.transform.position.x) > leash.x || Mathf.Abs(boi.position.y - GobStopper.transform.position.y) > leash.y || Mathf.Abs(boi.position.z - GobStopper.transform.position.z) > leash.z))
                {
                    //Set the object's position equal to GobStopper's position. Usueally this is the center of the room.
                    boi.transform.position = gsTransform.position;
                }
            }
        }

    }
    public void DoAToggle(float time, bool toggle)
    {
        StartCoroutine(TogglePerformanceMode(time, toggle));
    }
    public IEnumerator TogglePerformanceMode(float time, bool toggle)
    {

        if(GobStopper.transform.childCount != 0)
        {
            yield return 0;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t && t.gameObject.layer != 13)
            {
                string s = "Toggling " + t.name + " in " + this.name;
                //Debug.Log(s);
                if (t.gameObject.tag != "Floor" && t.gameObject.tag != "Wall")
                {
                    t.gameObject.SetActive(toggle);
                }
            }
        }
    }
    //Enables or disables hazards in this room. Returns false if there are no hazards in the room
    public bool setRoomHazards(bool e)
    {
        //Check that the room has hazards
        if (HazardList.Count > 0)
        {
            //If enabling
            if (e)
            {
                //Enable each hazard
                foreach (DisableHazards h in HazardList)
                {
                    if (h)
                    {
                        h.EnableHazard();
                    }
                }
            }
            //If disabling
            else
            {
                //Disable each hazard
                foreach (DisableHazards h in HazardList)
                {
                    if (h)
                    {
                        h.DisableHazard();
                    }
                }
            }
            return true;
        }
        //If the room doesn't have hazards, return false.
        else
        {
            return false;
        }
    }
    //Getter for gobstopper
    public GameObject GetGobStopper()
    {
        return GobStopper;
    }
    //Spawn an enemy goblin at the gobstopper's location with an offset from it's center
    public GameObject SpawnGob(Vector3 offset)
    {
        //Create a new enemy at the gobstopper and store it's reference
        GameObject newGob = Instantiate(EnemyCharacter, GobStopper.transform);
        //Move the new enemy to a passed in position
        newGob.transform.position += offset;
        //Set the new enemy's rotation to a random direction
        newGob.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Return the new enemy
        return newGob;
    }
    //Getter for hazard list
    public List<DisableHazards> getHazardList()
    {
        return HazardList;
    }
    //Getter for the maximum number of enemies a room can hold
    public int getMaxGobs()
    {
        return MaxGobs;
    }
    //Setter for whether the room has been beaten before.
    public void setRoomCleared(bool cleared)
    {
        this.cleared = cleared;
    }
    //Getter for whether the room has been beaten before.
    public bool getRoomCleared()
    {
        return cleared;
    }
    //Getter for the Spawn Radius. Director handles the spawn offset.
    public float getSpawnRadius()
    {
        return SpawnRadius;
    }
}
