using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Director : MonoBehaviour
{
    //List of all players
    public List<GameObject> PlayerList;
    //Player Input Manager
    public PlayerInputManager pim;
    //Current room that players are in
    GameObject CurrentRoom;
    //Room Info for room that players are in, and the room itself
    RoomInfo CurrentRoomInfo;
    GameObject CurrentGobStopper=null;
    //Boolean describing whether doors can activate
    [SerializeField] public bool CanTeleport=true;
    //Current number of living gobs in current room
    public float GobCount=0;
    //Distance from the center of the room that enemies can be spawned. Updated on a per-room basis
    private float SpawnRadius = 0.0f;
    //Total number of goblins spawned into the room
    int NumGobsSpawned;
    //Number of goblins that can be spawned in the room
    int MaxGobsInRoom = 0;
    //Current number of players knocked down
    public int PlayerDeathCount = 0;
    //Basically the last door that the player used
    ModularDoor DoOverDoor = null;
    //Current number of extra lives
    [SerializeField] public int ExtraLife = 1;
    int CurrentExtraLife=1;
    //If true, players have an extra life system
    [SerializeField] bool DoExtraLives = false;
    ///Global Variables
    float timer = 0.0f;
    [SerializeField] float EnemySpawnInterval = .15f;
    // Track player gold
    public int gold = 0;
    // audio clip that plays when all goblins in a room are defeated
    [SerializeField] AudioClip unlockSound;
    [SerializeField] bool IMMORTAL = false;
    //Sprites for extra lives
    public Sprite[] livesSprites = new Sprite[10];
    //UI element that displays how many lives the players have
    private Image LivesCountImage=null;
    //List of all Weapon GameObjects
    public GameObject[] WeaponList;
    //List of player's weapon choices
    public GameObject[] PlayerWeapons;
    Cam cam;
    PauseScript ps = null;
    // Start is called before the first frame update
    void Start()
    {
        //if player input manager is not passed in, search for it in the scene
        if (pim == null)
        {
            pim = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
            if (pim == null)
            {
                //Debug.Log("ERROR: PlayerManager not found.");
            }
        }
        
        //construct list of players and array of player existence
        PlayerList = new List<GameObject>();
        //find all playerinfo scripts
        PlayerCharacter[] temp = FindObjectsOfType<PlayerCharacter>();
        //Add all players to list of players
        foreach (PlayerCharacter p in temp)
        {
            //test for unique and bounded player count.
                //add ragdoll to array of players, and set player id to found.
                PlayerList.Add(p.gameObject);
            
        }
        //Sets current room to starting room if it exists and gets its room info, and the enemy container/parent.
        CurrentRoom = GameObject.Find("Room_0_0");
        if (CurrentRoom)
        {
            CurrentRoomInfo = CurrentRoom.GetComponent<RoomInfo>();
        }
        CurrentGobStopper = CurrentRoomInfo.GetGobStopper();
        //Set the number of goblins in the room to be 0, because the starting room has no goblins in it
        NumGobsSpawned = 0;

        //allows teleporting from current room, just in case it's not for some reason
        UnlockDoors();
    }
    // Update is called once per frame
    void Update()
    {
        //keep track of time passed since last enemy was spawned
        timer += Time.deltaTime;
        //If it's been enough time since the last enemy was spawned, and there haven't been enough spawned yet, spawn a new one and update trackers
        if(timer>EnemySpawnInterval && NumGobsSpawned < (MaxGobsInRoom)){
            //Spawn a new goblin with a random position in the room, and randomly generate an offset.
            
            CurrentRoomInfo.SpawnGob(new Vector3(Random.Range(-SpawnRadius, SpawnRadius), 0, Random.Range(-SpawnRadius, SpawnRadius)));
            //Iterate number of total and current gobs
            GobCount++;
            NumGobsSpawned++;
            //Reset timer
            timer = 0;
        }
    }
    //Basically calls start
    public void Reload()
    {
        //Finds Player Input Manager
        if (pim == null)
        {
            pim = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
            if (pim == null)
            {
                Debug.Log("ERROR: PlayerManager not found.");
            }
        }

        //construct list of players and array of player existence
        PlayerList = new List<GameObject>();
        //find all playerinfo scripts
        PlayerCharacter[] temp = FindObjectsOfType<PlayerCharacter>();
        foreach (PlayerCharacter p in temp)
        {
            //test for unique and bounded player count.

            //add ragdoll to array of players, and set player id to found.
            PlayerList.Add(p.gameObject);

            //Debug.Log("Error: Player Overload. Doors Disabled.");
        }
    }
    //Start spawning enemies, disable teleportation
    public void ActivateRoom()
    {
        //Set the gobstopper and spawn radius here, since the alternative is pain
        CurrentGobStopper = CurrentRoomInfo.GetGobStopper();
        SpawnRadius = CurrentRoomInfo.getSpawnRadius();
        //Reset living enemy count, and include enemy count from previous encounters in the room due to do-overs
        GobCount = 0;
        for(int i=0; i<CurrentGobStopper.transform.childCount; i++)
        {
            Transform t = CurrentGobStopper.transform.GetChild(i);
            if (t)
            {
                CharacterMovement cm = t.GetComponentInChildren<CharacterMovement>();
                if (cm)
                {
                    if (!cm.knockedOut)
                    {
                        GobCount++;
                    }
                }
            }
        }
        //reset spawned enemy count
        NumGobsSpawned = CurrentGobStopper.transform.childCount;
        //Set enemy spawning limit equal proportional to player count based on max number of goblins in roominfo, unless the player has cleared the room before, in which case set limit to 0
        if (CurrentRoomInfo.getRoomCleared() == false)
        {
            //Disable teleportation between rooms
            LockDoors();
            //Enemies spawned = maxgobs * (1 + # of players / 4) / 2, unless maxGobs is either 1 or 0, then it's equal to maxgobs for bossfight reasons.
            if (CurrentRoomInfo.getMaxGobs() < 2)
            {
                MaxGobsInRoom = CurrentRoomInfo.getMaxGobs();
                if (CurrentRoomInfo.getMaxGobs() == 0)
                {
                    //If the room is empty, unlock it and set it to cleared as a failsafe
                    UnlockDoors();
                    CurrentRoomInfo.setRoomCleared(true);
                }
            }
            else
            {
                //Calculating max number of gobs based on 4 player max gobs value
                MaxGobsInRoom = Mathf.CeilToInt((float)CurrentRoomInfo.getMaxGobs() * (1.0f + (float)PlayerList.Count / 4.0f) / 2.0f);
            }
            //Debug.Log(MaxGobsInRoom+" gobs in room");
            
        }
        else
        {
            //Enemies is 0, so no enemies spawn because it's already at the limit.
            MaxGobsInRoom = 0;
            UnlockDoors();
        }
        timer = -1;
    }
    //Called when a room is not empty
    public void LockDoors()
    {
        //disable teleportation
        CanTeleport = false;
        //Enable room hazards
        CurrentRoomInfo.setRoomHazards(true);
    }
    //Called when the room is empty
    public void UnlockDoors()
    {
        Debug.Log("Can Now Teleport");
        //Play a sound clip
        if (unlockSound!=null)
        {
            GetComponent<AudioSource>().PlayOneShot(unlockSound, .25f);
        }
        //Set room to cleared
        CurrentRoomInfo.setRoomCleared(true);
        //Disable room hazards
        CurrentRoomInfo.setRoomHazards(false);
        //Allow teleporting
        CanTeleport = true;
    }
    //Getter for the Current Room
    public GameObject GetCurrentRoom()
    {
        return CurrentRoom;
    }
    //Getter for the Current Room Info
    public RoomInfo GetCurrentRoomInfo()
    {
        return CurrentRoomInfo;
    }
    //Setter for Current Room and room info
    public void SetCurrentRoom(GameObject TargetRoom)
    {
        CurrentRoom = TargetRoom;
        CurrentRoomInfo = CurrentRoom.GetComponent<RoomInfo>();

    }
    //Called when an enemy reaches 0 health
    public void GobDied()
    {
        //Decrement total number of living enemies
        GobCount--;
        //Check whether there are any more enemies now
        if (GobCount <= 0)
        {
            //Unlock the room
            UnlockDoors();
            //Heal and revive all players
            foreach (GameObject boi in PlayerList)
            {
                PlayerCharacter boiBrain = boi.GetComponent<PlayerCharacter>();
                boiBrain.health = boiBrain.MaxHealth;
                boiBrain.Revive(true);
            }
            //Set number of living enemies to 0 as a failsafe
            GobCount = 0;
        }
    }
    //Getter for current room transform's position vector
    public Vector3 CurrentRoomPosition()
    {
        return CurrentRoom.transform.position;
    }
    //Method for handling a player getting knocked down
    public void PlayerDied()
    {
        //Increase number of currently downed players
        PlayerDeathCount=Mathf.Max(PlayerDeathCount+1, 1);
        //If every player is down
        if (PlayerDeathCount >= PlayerList.Count && !IMMORTAL){
            //If extra lives are disabled or depleted, call game over function
            if (CurrentExtraLife <= 1|| !DoExtraLives)
            {
                GameOver();
            }
            //If we're doing extra lives and there's at least one extra life remaining
            else
            {
                //Reduce Extra lives
                CurrentExtraLife--;
                //Update the extra lives UI
                if (LivesCountImage)
                {
                    LivesCountImage.sprite = livesSprites[CurrentExtraLife];
                }
                //Call the do over function and the door's do over function
                if (DoOverDoor != null)
                {
                    DoOver();
                    DoOverDoor.DoOver();
                }
            }
        }
    }
    //Called when all players are knocked down
    void GameOver()
    {
        //Load "Game Over" screen.
        Debug.Log("Game Over Called");
        Portal portal = GameObject.FindObjectOfType<Portal>();
        if(portal)
        {
            portal.CloseCurtains();
        }
        else
        {
            Debug.Log("Unable to find the portal");
            SceneManager.LoadSceneAsync("LoseScreen", LoadSceneMode.Single);
        }
    }
    //Called when players run out of lives
    void DoOver()
    {
        //Allow teleportation through doors
        CanTeleport = true;

    }
    //Called when players are revived
    public void PlayerUnDied()
    {
        //Decrement downed players
        PlayerDeathCount = Mathf.Max(PlayerDeathCount - 1, 0);
    }
    //Getter for remaining extra lives
    public int GetExtraLives()
    {
        return CurrentExtraLife;
    }
    //Setter for extra lives
    public void SetExtraLives(int lives)
    {
        ExtraLife = lives;
        ResetCurrentExtraLife();
    }
    //Setter for the last door that the players used
    public void SetDoOver(ModularDoor door)
    {
        DoOverDoor = door;
    }
    //Getter for whether the game is using extra lives
    public bool GetDoExtraLives()
    {
        return DoExtraLives;
    }
    //Increments gold
    public void IncreaseGold(int g)
    {
        gold += g;
    }
    //Getter for current gold amount.
    public int GetGold()
    {
        return gold;
    }
    //setter for immortal boolean
    public void setImmortal(bool foo)
    {
        IMMORTAL = foo;
    }
    //getter for immortal boolean
    public bool isImmortal()
    {
        return IMMORTAL;
    }
    //setter for life UI object
    public void setLivesCountImage(Image image)
    {
        LivesCountImage = image;
        if (LivesCountImage)
        {
            LivesCountImage.sprite = livesSprites[CurrentExtraLife];
        }
    }
    //getter for life UI object
    public Image getLivesCountImage()
    {
        if (LivesCountImage)
        {
            return LivesCountImage;
        }
        else
        {
            return null;
        }
    }
    //function called when pressing retry from the lose screen
    public void doRetry()
    {
        CanTeleport = true;
    }

    public GameObject GetPlayerWeaponChoice(int index)
    {
        return PlayerWeapons[index];
    }

    public void SetPlayerWeaponChoice(int[] choices)
    {
        for(int i = 0; i < choices.Length; i++)
        {
            PlayerWeapons[i] = WeaponList[choices[i]];
        }
        
    }

    public void ResetPlayerWeapons()
    {
        StartCoroutine(ResetWeapons());
    }

    IEnumerator ResetWeapons()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < PlayerList.Count; i++)
        {
            Debug.Log("Player " + i + " Weapon: " + PlayerWeapons[i]);
            PlayerList[i].GetComponent<WeaponSwap>().SetWeapon(PlayerWeapons[i].GetComponent<Weapon>());
            Debug.Log("Weaponns reset");
        }
    }

    public void ResetCurrentExtraLife()
    {
        CurrentExtraLife=ExtraLife;
        if (LivesCountImage)
        {
            LivesCountImage.sprite = livesSprites[CurrentExtraLife];
        }
    }
    public Vector3 GetPlayerCenter()
    {
        Vector3 center = new Vector3(0, 0, 0);
        foreach(GameObject player in PlayerList)
        {
            center += player.transform.position;
        }
        if (PlayerList.Count > 0)
        {
            return center / PlayerList.Count;
        }
        else
        {
            return center;
        }
    }
    public List<GameObject> GetGobs()
    {
        List<GameObject> gobs = new List<GameObject>();
        if (CurrentGobStopper == null)
        {
            return null;
        }
        for(int i=0; i<CurrentGobStopper.transform.childCount; i++)
        {
            gobs.Add(CurrentGobStopper.transform.GetChild(i).gameObject);
        }
        return gobs;
    }
    public void setCamera(Cam c)
    {
        cam = c;
    }
    public void shake(float t, float s)
    {
        cam.ShakeCam(t, s);
    }
    public void SetPauseScript(PauseScript p)
    {
        if (p != null)
        {
            ps = p;
        }
    }
    public bool GetPaused()
    {
        if (ps != null)
        {
            return ps.isPaused;
        }
        return false;
    }
    public void StopRumbling()
    {
        foreach(GameObject boi in PlayerList)
        {
            PlayerCharacter p = boi.GetComponent<PlayerCharacter>();
            p.PleaseRumble(0.0f, 0.0f, 0.0f);
        }
    }
}
