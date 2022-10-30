using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float EHealth = 4.0f;//health of the hazard
    
    [SerializeField] private GameObject self = null;//stores the gameobject that this script is asociated with

    [SerializeField] private float IPhase = 2f;//amount of time where the trap can't be damaged before it can be damaged again

    [SerializeField] private float HPLost = 1f;

    [SerializeField] private float TimeToDelete = 5f;

    [SerializeField] private AudioSource HazHitSound = null;

    [SerializeField] private AudioSource HazDieSound = null;




    private bool NotImune = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getHealth()
    {
        return EHealth;
    }

    public float getImunePhase()
    {
        return IPhase;
    }

    public float getHPLost()
    {
        return HPLost;
    }

    public float getTimeToDelete()
    {
        return TimeToDelete;
    }

    public bool getNotImune()
    {
        return NotImune;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Weapon>())//checks to see if the collision object have a weapon script        {
            if (NotImune)
                {
                NotImune = false;//sets imune to false to prevent multiple hits
                EHealth -= HPLost;
                HazHitSound.Play();

                if (EHealth <= 0)//if health is less then 0 destroy the object
                {
                    if (HazDieSound)
                    {
                        HazDieSound.Play();
                    }
                    Destroy(self);
                }
                else
                {
                    StartCoroutine(ImunityPhase(IPhase));
                }
            }
       }
    

    private IEnumerator ImunityPhase(float time)
    {
        
        yield return new WaitForSeconds(time);
        NotImune = true;
    }

    private IEnumerator HitReact(float TimeReacting)
    {
    yield return new WaitForSeconds(TimeReacting);
    }

    private IEnumerator Dieing(float DeathTime)
    {
    yield return new WaitForSeconds(DeathTime);
    }
}
