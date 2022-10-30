using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWackerController : MonoBehaviour
{
    [SerializeField] private Animator myWacker = null;//where the animator from the editor pertaining to the hazard is stored

    private bool openTrigger = true;//if true trap is either at rest or activating
    private bool closeTrigger = false;//if true, trap is closing

    [SerializeField] private string ActivateTurn = "ActivateTurn";//the exact animation name for the trap activating
    [SerializeField] private string ResetTurn = "ResetTurn";//the exact animation name for the trap reseting
    [SerializeField] public HazardKnockback HKnockback = null;

    private bool done = true;//checks to see if the trap is reset



    [SerializeField] private bool TriggeredTrap = false;

    //runs the smoothAnimation Ienumerator if the other tag is a player or enemy
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<ConfigurableJoint>() && openTrigger)//checks to see if the gameobject has a configerable joint, a key component in the character prefabs
        {
            if (enabled == true)
            {
                if (done)//if the variable done is true, then the trap is at rest and therefore can be activated without trouble
                {
                    StartCoroutine(smoothAnimation());
                }
            }
        }
        
    }


    private IEnumerator smoothAnimation()
    {
        if (openTrigger)//if object is at rest plays turn animation
        {
            done = false;
            if (TriggeredTrap && done)
            {
                //KnockbackCollider.GetComponent<Collider>().enabled = true;
            }
            


            myWacker.Play(ActivateTurn, 0, 0.0f);
            openTrigger = false;//sets to false so object won't play Activate turn
            closeTrigger = true;
        }

        StartCoroutine(PlaySound(0.5f));
        //enables the audiosource, then plays the sound for 1/2 a second before disabling the source
        

        yield return new WaitForSeconds(2f);

        if (closeTrigger)//after 2 seconds resets the trap
        {
            if (TriggeredTrap)
            {
                //KnockbackCollider.GetComponent<Collider>().enabled = false;
            }
            myWacker.Play(ResetTurn, 0, 0.0f);
            openTrigger = true;//trap is reset so it can be activated again
            closeTrigger = false;
        }
        yield return new WaitForSeconds(1);
        done = true;//primes the trap so that it can activate after it resets

    }
    //getters for unit testing
    public bool getCTrigger()
    {
        return closeTrigger;
    }

    public bool getOTrigger()
    {
        return openTrigger;
    }

    public Animator getMyWacker()
    {
        return myWacker;
    }

    public string getActivateTurn()
    {
        return ActivateTurn;
    }

    public string getResetTurn()
    {
        return ResetTurn;
    }

    public HazardKnockback getHKnockback()
    {
        return HKnockback;
    }



    public bool GetTriggeredTrap()
    {
        return TriggeredTrap;
    }

    public bool getIsReset()
    {
        return done;
    }

    /*this script is for sound effects that only need to play for a set ammount of time */
    private IEnumerator PlaySound(float timer)
    {
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timer);
        GetComponent<AudioSource>().enabled = false;
    }
}
