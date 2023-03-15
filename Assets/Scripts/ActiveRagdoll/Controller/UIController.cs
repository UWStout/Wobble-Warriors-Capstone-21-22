using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private int playerNumber;
    private ReadyUpLives lives;
    public static UnityEvent<int> playerReady = new UnityEvent<int>();
    public static UnityEvent<int, int> changeColor = new UnityEvent<int, int>();
    public static UnityEvent<int, int> changeWeapon = new UnityEvent<int, int>();

    // Start is called before the first frame update
    void Start()
    {
        playerNumber = GetComponent<PlayerInput>().playerIndex + 1;
        lives = FindObjectOfType<ReadyUpLives>();
    }

    void Update()
    {
        
    }

    public void OnSelect(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        playerReady.Invoke(playerNumber); //calls the event that allows the PlayerJoin script to register that a player is ready
    }

    public void OnLeft(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnLeft Called!");
        changeWeapon.Invoke(-1, playerNumber);
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnRight Called!");
        changeWeapon.Invoke(1, playerNumber);
    }

    public void OnUp(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnUp Called!");
        changeColor.Invoke(1, playerNumber);
    }

    public void OnDown(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnDown Called!");
        changeColor.Invoke(-1, playerNumber);
    }

    public void OnBack(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        if (SceneManager.GetActiveScene().name == "ReadyUpMenu") { return; } // TODO: FIND BETTTER SOLUTION
        for (int c = 0; c < 4; c++)
        {
            ReadyUpPlayer.colors[c] = -1;
            ReadyUpPlayer.weapons[c] = 0;
        }
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void OnDecrease(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnDecrease Called!");
        lives.DecreaseLives();
    }

    public void OnIncrease(InputAction.CallbackContext value)
    {
        if (!value.started) { return; }
        Debug.Log("OnIncrease Called!");
        lives.IncreaseLives();
    }
}
