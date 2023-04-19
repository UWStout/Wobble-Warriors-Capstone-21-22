using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AUTHOR: Brian Bauch
//rewritten by jaden schneider lol
public class RangedAI : AICharacter
{

    [SerializeField] Rigidbody projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform projectileOrigin;

    //AI behaviour. can be overwritten for different AI
    public override void AIBehaviour()
    {
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        //move away from target


        LookAt(currentTarget.transform.position);

        if (distance < maxDistance)
        {
            SetMoveDirection((transform.position - currentTarget.transform.position).normalized);
        }
        else
        {
            SetMoveDirection(Vector3.zero);
        }
        

        //ranged goblins always attack lol
        if (ableToAttk)
        {
            StartCoroutine(RangedAttack());
        }
    }

    IEnumerator RangedAttack()
    {
        PlayRandomSound();

        //throw something
        Rigidbody clone = Instantiate(projectile, projectileOrigin.position, Quaternion.identity);
        clone.velocity = (currentTarget.transform.position - clone.position).normalized * projectileSpeed;

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
