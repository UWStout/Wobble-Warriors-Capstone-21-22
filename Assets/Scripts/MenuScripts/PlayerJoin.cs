using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerJoin : MonoBehaviour
{
    PlayerInputManager manager;
    // Graphics Variables
    public Image notReady; //uncolored player
    public Image join; //join text below player icon
    public Image ready; //a player has ready'd up, requires player to have selected a weapon
    public Image duplicateWarning; //displays issue with ready up
    //-----------------------
    //Warning handling variables
    private bool warningOn;
    public float duration = 1.0f;
    private Color color1 = new Color(1, 1, 1, 0.0f);
    public Color color2 = new Color(1, 1, 1, .9f);
    private float timer = 0.0f;
    //arrow variables
    public GameObject colorArrows;
    public GameObject weaponArrows;
    //------colored players, requires player to join game
    private bool colorLock = false;
    private int color;
    
    public Image[] colorPartials;
    public Image[] colorFulls;
    //----------------------
    //-------player weapons
    private int weapon = 0;
    public Image[] weaponPartials;
    public Image[] weaponFulls;
    //---------------------------
    // Event Variables
    [SerializeField]
    public int playerNumber; // what the given player's number is
    // Control Variables
    bool isAlreadyReady = false;
    ReadyUpPlayer rup;

    // Start is called before the first frame update
    void Start()
    {
        //set whether certain gui elements are active or not on startup
        notReady.gameObject.SetActive(true);
        for(int c = 0; c < colorPartials.Length; c++)
        {
            colorPartials[c].gameObject.SetActive(false);
            colorFulls[c].gameObject.SetActive(false);
        }
        for (int w = 0; w < weaponPartials.Length; w++)
        {
            weaponPartials[w].gameObject.SetActive(false);
            weaponFulls[w].gameObject.SetActive(false);
        }
        colorArrows.gameObject.SetActive(false);
        weaponArrows.gameObject.SetActive(false);
        join.gameObject.SetActive(true);
        ready.gameObject.SetActive(false);

        ReadyUpPlayer.newInput.AddListener(playersChanged);
        UIController.playerReady.AddListener(playerCouldBeReady); //have bool variable that indicated whether or not the player is already readyd up
        UIController.changeColor.AddListener(changeColor);
        UIController.changeWeapon.AddListener(changeWeapon);
        rup = GameObject.Find("Canvas").GetComponent<ReadyUpPlayer>();
    }

    void playersChanged()
    {
        int numPlayers = ReadyUpPlayer.playersIn;
        if(numPlayers >= playerNumber)
        {
            if (colorLock == false)
            {
                color = DetermineColor(playerNumber - 1);
                colorPartials[color].gameObject.SetActive(true);
                ReadyUpPlayer.colors[playerNumber - 1] = color;
                colorLock = true;
            }
            if(numPlayers == playerNumber)
            {
                weaponPartials[0].gameObject.SetActive(true);
                colorArrows.gameObject.SetActive(true);
                weaponArrows.gameObject.SetActive(true);
            }
            join.gameObject.SetActive(false);
            notReady.gameObject.SetActive(false);
        }
        else
        {
            for (int c = 0; c < colorPartials.Length; c++)
            {
                colorPartials[c].gameObject.SetActive(false);
                colorFulls[c].gameObject.SetActive(false);
            }
            for (int w = 0; w < weaponPartials.Length; w++)
            {
                weaponPartials[w].gameObject.SetActive(false);
                weaponFulls[w].gameObject.SetActive(false);
            }
            join.gameObject.SetActive(true);
            notReady.gameObject.SetActive(true);
            colorArrows.gameObject.SetActive(false);
            weaponArrows.gameObject.SetActive(false);
            colorLock = false;
        }
    }

    private int DetermineColor(int index)
    {
        int[] colors = ReadyUpPlayer.colors;
        List<int> potentialColors =  new List<int>{ 0, 1, 2, 3 };
        for(int i = 0; i < colors.Length; i++)
        {
            if(colors[i] != -1)
            {
                potentialColors.Remove(colors[i]);
            }
        }
        return potentialColors[0];
    }

    void playerCouldBeReady(int passedNum)
    {
        if(playerNumber == passedNum)
        {
            if (isAlreadyReady == false)
            {
                playerReadyUp();
            }
            else if (isAlreadyReady == true)
            {
                playerNotReady();
            }
        }
    }

    void playerReadyUp()
    {
        bool dupeColor = false;
        if(ReadyUpPlayer.lockedIn == false)
        {
            for(int i = 0; i < 4; i++)
            {
                if(i != playerNumber - 1 && ReadyUpPlayer.colors[i] == color)
                {
                    dupeColor = true;
                    //flashes dup warning
                    warningOn = true;
                    break;
                }
            }

            if (dupeColor == false)
            {
                //turn dup warning off
                warningOn = false;
                duplicateWarning.color = color1;
                timer = 0.0f;

                isAlreadyReady = true;
                ready.gameObject.SetActive(true);
                for (int c = 0; c < colorPartials.Length; c++)
                {
                    colorPartials[c].gameObject.SetActive(false);
                    colorFulls[c].gameObject.SetActive(false);
                }
                for (int w = 0; w < weaponPartials.Length; w++)
                {
                    weaponPartials[w].gameObject.SetActive(false);
                    weaponFulls[w].gameObject.SetActive(false);
                }
                colorArrows.gameObject.SetActive(false);
                weaponArrows.gameObject.SetActive(false);
                colorFulls[color].gameObject.SetActive(true);
                weaponFulls[weapon].gameObject.SetActive(true);

                ReadyUpPlayer.playersReady++;
                Debug.Log("I am player " + playerNumber + " and I am " + color + " and I wield a " + weapon);
            }
        }
        if (ReadyUpPlayer.vidIsPlaying)
        {
            rup.PrepLevel();
        }
    }

    void playerNotReady()
    {
        if (ReadyUpPlayer.lockedIn == false)
        {
            isAlreadyReady = false;
            ready.gameObject.SetActive(false);
            colorPartials[color].gameObject.SetActive(true);
            weaponPartials[weapon].gameObject.SetActive(true);
            colorArrows.gameObject.SetActive(true);
            weaponArrows.gameObject.SetActive(true);
            colorFulls[color].gameObject.SetActive(false);
            weaponFulls[weapon].gameObject.SetActive(false);
            ReadyUpPlayer.playersReady--;
        }
    }

    private void Update()
    {
        //triggers the warning of duplicate colors
        if (warningOn)
        {
            float t = Mathf.PingPong(Time.time, duration) / duration;
            duplicateWarning.color = Color.Lerp(color1, color2, t);
            timer += Time.deltaTime;

            //timer for how long the warning is up
            if (timer > 5.0f)
            {
                timer = 0.0f;
                warningOn = false;
                duplicateWarning.color = color1;
            }
        }
    }

    void changeColor(int change, int playerNum)
    {
        if (playerNum == playerNumber && isAlreadyReady == false)
        {
            color += change;
            if(color > 3) { color = 0; }
            if(color < 0) { color = 3; }
            for (int c = 0; c < colorPartials.Length; c++)
            {
                colorPartials[c].gameObject.SetActive(false);
            }
            colorPartials[color].gameObject.SetActive(true);
            ReadyUpPlayer.colors[playerNumber - 1] = color;
            Debug.Log("Color: " + color);
        }
        if (ReadyUpPlayer.vidIsPlaying)
        {
            rup.PrepLevel();
        }
    }

    void changeWeapon(int change, int playerNum)
    {
        if (playerNum == playerNumber && isAlreadyReady == false)
        {
            weapon += change;
            if (weapon > 3) { weapon = 0; }
            if (weapon < 0) { weapon = 3; }
            for (int w = 0; w < weaponPartials.Length; w++)
            {
                weaponPartials[w].gameObject.SetActive(false);
            }
            weaponPartials[weapon].gameObject.SetActive(true);
            ReadyUpPlayer.weapons[playerNumber - 1] = weapon;
            Debug.Log("Weapon: " + weapon);
        }
        if (ReadyUpPlayer.vidIsPlaying)
        {
            rup.PrepLevel();
        }
    }
}
