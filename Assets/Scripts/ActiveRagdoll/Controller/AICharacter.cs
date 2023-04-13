using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AUTHOR: Brian Bauch
//rewritten by jaden schneider lol
public class AICharacter : CharacterMovement
{
    public PlayerCharacter currentTarget; //Game Object variable storing the currently targeted player

    [Tooltip("The rate at which the enemy attacks (lower = faster)")]
    public float attackRate;
    public float attackDistance = 3;

    public bool ableToAttk = true; //bool that states whether the entity is able to attack

    [SerializeField] AudioClip[] growlSounds;
    [SerializeField] float soundChance = 40;

    [SerializeField] float LowPitch = .75f;
    [SerializeField] float HighPitch = 1.25f;

    public HealthBar healthbar;

    public void reloadHealth()
    {
        healthbar.setMaxValue((int)health);
    }

    // Start is called before the first frame update
    void Awake()
    {
        //establish the targets array by finding all targets in the room with the set tag
        ableToAttk = true;
        reloadHealth();
    }


    // Update is called once per frame
    void Update()
    {
        if (!knockedOut)
        {
            //if there is a target not currently chosen, then choose a target. or else face and move towards the chosen target
            if (currentTarget == null)
            {
                currentTarget = ChooseTarget();
            }
            else
            {
                AIBehaviour();
            }
        }
    }

    //for choosing a target
    PlayerCharacter ChooseTarget()
    {
        //find all player characters in the scene
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();
        List<PlayerCharacter> validTargets = new List<PlayerCharacter>();

        //filter valid targets for an active player
        for(int i = 0; i < players.Length; i++)
        {
            if (!players[i].knockedOut)
            {
                validTargets.Add(players[i]);
            }
        }

        //return no target if none are found
        if(validTargets.Count == 0) 
        {
            return null;
        }

        //select random target if one or more is found
        return validTargets[Random.Range(0, validTargets.Count)];
    }

    //AI behaviour. can be overwritten for different AI
    public virtual void AIBehaviour()
    {
        //find distance between transform and target
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        MoveTowards(currentTarget.transform.position);
        LookAt(currentTarget.transform.position);

        //move toward 
        if (distance <= attackDistance && ableToAttk)
        {
            StartCoroutine(AttackTarget());
        }
    }

    public void PlayRandomSound()
    {
        //if the randomly generated number is less than the sound chance variable
        if (Random.Range(0, 100) < soundChance)
        {
            //play one of the random sounds by taking one from a variable of sounds
            Debug.Log("Playing sound");
            int index = Random.Range(0, growlSounds.Length);
            GetComponent<AudioSource>().pitch = Random.Range(LowPitch, HighPitch);
            GetComponent<AudioSource>().PlayOneShot(growlSounds[index]);
        }
    }

    // for attacking a target
    IEnumerator AttackTarget()
    {
        //log that an attack has been made, and damage the target
        if (growlSounds.Length > 0)
        {
            PlayRandomSound();
        }
        Debug.Log("Attacking");
        Attack(currentTarget.transform);
        //if the chosen target is no longer alive after attacking, set target chosen to false
        if (currentTarget.knockedOut)
        {
            currentTarget = null;
        }
        //go into an attack cooldown state, waiting the attack rate variable to be able to attack again
        ableToAttk = false;
        yield return new WaitForSeconds(attackRate);
        ableToAttk = true;
    }
}
