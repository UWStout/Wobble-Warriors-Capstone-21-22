using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AUTHOR: Brian Bauch
public class AICharacter : CharacterMovement
{
    private GameObject[] targetsArray; //Array containing all of the present players in the game

    [SerializeField] string targetString = "Target";

    public GameObject chosenTarget; //Game Object variable storing the currently targeted player
    private bool targetChosen = false; //bool that tells whether or not this entity has a current target

    [Tooltip("The rate at which the enemy attacks (lower = faster)")]
    [SerializeField] float attackRate;
    [SerializeField] float attackDistance = 3;

    private bool ableToAttk = true; //bool that states whether the entity is able to attack

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
        targetsArray = GameObject.FindGameObjectsWithTag(targetString);
        reloadHealth();
    }


    // Update is called once per frame
    void Update()
    {
        if (!knockedOut)
        {
            //if there is a target not currently chosen, then choose a target. or else face and move towards the chosen target
            if (!targetChosen)
            {
                ChooseTarget();
            }
            else
            {
                MoveTowardTarget();
            }
        }
    }

    //for choosing a target
    void ChooseTarget()
    {
        //generate a random number from 0 to 3
        //if the alive bool of the chosen index is true, then choose that index as your target. or else set target chosen to false
        int randnum = Random.Range(0, targetsArray.Length);
        if (!targetsArray[randnum].GetComponent<CharacterMovement>().knockedOut)
        {
            chosenTarget = targetsArray[randnum];
            targetChosen = true;
        }
        else
        {
            targetChosen = false;
        }
    }

    //for moving towards a target
    void MoveTowardTarget()
    {
        //determine the x and z axis distance from the chosen target
        float xDist = chosenTarget.transform.position.x - transform.position.x;
        float zDist = chosenTarget.transform.position.z - transform.position.z;
        //if either the x or z distance is greater than attackDistance, move towards the target. or else attack.
        if (Mathf.Abs(xDist) > attackDistance || Mathf.Abs(zDist) > attackDistance)
        {
            MoveTowards(chosenTarget.transform.position);
            LookAt(chosenTarget.transform.position);
        }
        else
        {
            MoveTowards(chosenTarget.transform.position);
            LookAt(chosenTarget.transform.position);
            if (ableToAttk)
            {
                StartCoroutine(AttackTarget());
            }
        }
    }

    void PlayRandomSound()
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
        Attack(chosenTarget.transform);
        //if the chosen target is no longer alive after attacking, set target chosen to false
        if (chosenTarget.GetComponent<CharacterMovement>().knockedOut)
        {
            targetChosen = false;
        }
        //go into an attack cooldown state, waiting the attack rate variable to be able to attack again
        ableToAttk = false;
        yield return new WaitForSeconds(attackRate);
        ableToAttk = true;
    }
}
