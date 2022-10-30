using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossAI : CharacterMovement
{
    public bool playerInSight;
    public bool longRangeSight = false;
    [SerializeField] GameObject followTarget;
    [SerializeField] GameObject chargeTarget;
    public GameObject targetedPlayer;
    [SerializeField] float moveSpeed;
    Vector3 chargeDirection;
    Quaternion chargeAngle;
    [SerializeField] GameObject StunVFX;


    bool canCharge = true;
    bool prepping = false;
    public bool charging = false;
    public bool stunned = false;

    public HealthBar healthbar;

    public void reloadHealth()
    {
        healthbar.setMaxValue((int)health);
    }


    // Update is called once per frame
    void Update()
    {
        if (animator)
        {
            animator.SetBool("Charging",charging);
            animator.SetBool("Prepping", prepping);
            animator.SetBool("Stunned", stunned);
            animator.SetBool("Walking", !prepping && !charging && !stunned);
        }

        if (playerInSight || prepping || charging || stunned || longRangeSight)
        {

            if (canCharge)
            {
                StartCoroutine(PrepCharge());
                canCharge = false;
                prepping = true;
            }
            else if (prepping)
            {
                StartCoroutine(TargetCharge());
                LookAt(chargeTarget.transform.position);
            }
            else if (charging)
            {
                moveVector = chargeDirection * moveSpeed * 6;
                LookAt(chargeTarget.transform.position);
            }
            else if (stunned)
            {

            }
            else
            {
                MoveTowards(followTarget.transform.position);
                LookAt(followTarget.transform.position);
            }
        }
        else
        {
            /*
            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, Time.deltaTime * moveSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(followTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            */
            MoveTowards(followTarget.transform.position);
            LookAt(followTarget.transform.position);
        }


    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (charging)
            {
                StartCoroutine(Stop(2.0f));
            }
        }
        if (other.gameObject.CompareTag("Pillar"))
        {
            if (charging)
            {
                //call pillar function
                BossPillar pillar = FindObjectOfType<BossPillar>();
                if (pillar)
                {
                    pillar.DoStunEffects();
                }
                StartCoroutine(Stun(10.0f));
            }
        }
    }

    IEnumerator PrepCharge()
    {
        moveVector = new Vector3(0f, 0f, 0f);
        
        if (longRangeSight)
        {
            targetedPlayer = GetComponent<BossSensor>().Objects[0];
        }
        
        chargeTarget.transform.position = targetedPlayer.transform.position;
        yield return new WaitForSeconds(3.0f);
        prepping = false;
        chargeDirection = (chargeTarget.transform.position - this.transform.position).normalized;
        chargeTarget.transform.position += chargeDirection * 100;
        charging = true;
    }

    IEnumerator TargetCharge()
    {
        //chargeTarget.transform.position = targetedPlayer.transform.position;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Stop(float stopTime)
    {
        Debug.Log("Stopped");
        moveVector = new Vector3(0f, 0f, 0f);
        stunned = true;
        charging = false;
        yield return new WaitForSeconds(stopTime);
        stunned = false;
        StartCoroutine(ChargeCooldown(2f));
    }

    IEnumerator Stun(float stunTime)
    {
        Debug.Log("Stunned");
        Destroy(Instantiate(StunVFX, this.transform), stunTime);
        moveVector = new Vector3(0f, 0f, 0f);
        stunned = true;
        charging = false;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        StartCoroutine(ChargeCooldown(2f));
    }

    IEnumerator ChargeCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canCharge = true;
    }

    /*
    IEnumerator ChargeCooldown(float chargeCooldown)
    {
        yield return new WaitForSeconds(chargeCooldown);
        canCharge = true;
    }
    */

}

