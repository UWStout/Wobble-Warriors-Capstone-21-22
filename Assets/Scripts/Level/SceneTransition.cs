using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    public List<PlayerCharacter> players = new List<PlayerCharacter>();
    public Color[] colors;

    public static UnityEvent numPlayersChanged = new UnityEvent();
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); //lets the gameobject transition between levels
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        numPlayersChanged.Invoke();

        Debug.Log("PLAYER JOINED");
        players.Add(playerInput.gameObject.GetComponent<PlayerCharacter>()); //get script and add to player list
        players.ToArray()[players.Count - 1].SetColor(colors[players.Count - 1]);
        playerInput.transform.root.parent = transform; //find the root and make it a child of the player input manager
        GameObject.Find("Director").GetComponent<Director>().Reload(); //reload director

        for(int i = 0; i < players.Count; i++)
        {
            //refresh player health
            players[i].reloadHealth();
        }
    }

    void Update()
    {
        
    }
}
