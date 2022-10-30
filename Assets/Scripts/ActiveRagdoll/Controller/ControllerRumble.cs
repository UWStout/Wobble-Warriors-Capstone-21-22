using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{
    private PlayerInput playerInput;
    private Gamepad currentGamepad;
    [SerializeField] Vector2 motorSpeeds;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        currentGamepad = GetGamepad();
    }

    public void Rumble(float duration, float lowFrequency, float highFrequency)
    {
        if (currentGamepad != null)
        {
            motorSpeeds = new Vector2(lowFrequency, highFrequency);
            StopAllCoroutines();
            StartCoroutine(RumbleForSeconds(duration));
        }
    }

    IEnumerator RumbleForSeconds(float duration)
    {
        currentGamepad.SetMotorSpeeds(motorSpeeds.x, motorSpeeds.y);
        yield return new WaitForSeconds(duration);
        currentGamepad.SetMotorSpeeds(0, 0);
    }

    private Gamepad GetGamepad()
    {
        Gamepad gamepad = null;

        foreach(var g in Gamepad.all) //search through all gamepads
        {
            foreach (var d in playerInput.devices) //for all devices assigned to a player, find one in the gamepad array
            {
                if(d.deviceId == g.deviceId)
                {
                    gamepad = g;
                    break;
                }
            }
            if(gamepad != null)
            {
                break;
            }
        }
        return gamepad;
    }
}
