using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public bool playerInSight;
    [SerializeField] GameObject followTarget;
    [SerializeField] GameObject chargeTarget;
    GameObject targetedPlayer;
    [SerializeField] float moveSpeed;
    Vector3 chargeDirection;
    Quaternion chargeAngle;


    bool canCharge = true;
    bool prepping = false;
    bool charging = false;
    bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInSight || prepping || charging || stunned)
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
                Quaternion targetRotation = Quaternion.LookRotation(chargeTarget.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            }
            else if (charging)
            {
                transform.position += chargeDirection * Time.deltaTime * moveSpeed * 6;
            }
            else if (stunned)
            {

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, Time.deltaTime * moveSpeed);
                Quaternion targetRotation = Quaternion.LookRotation(followTarget.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, Time.deltaTime * moveSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(followTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        }


    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (charging)
            {
                StartCoroutine(Stun(2.0f));
            }
        }
    }

    IEnumerator PrepCharge()
    {
        targetedPlayer = GetComponent<BossSensor>().Objects[0];
        chargeTarget.transform.position = GetComponent<BossSensor>().Objects[0].transform.position;
        yield return new WaitForSeconds(3.0f);
        prepping = false;
        chargeDirection = (chargeTarget.transform.position - this.transform.position).normalized;
        charging = true;
    }

    IEnumerator TargetCharge()
    {
        //chargeTarget.transform.position = targetedPlayer.transform.position;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Stun (float stunTime)
    {
        Debug.Log("Stun");
        stunned = true;
        charging = false;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        canCharge = true;
    }

}
