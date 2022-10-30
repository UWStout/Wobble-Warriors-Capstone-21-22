using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPillar : MonoBehaviour
{
    //[SerializeField] private GameObject boss = null;
    //[SerializeField] private float stunTime = 5f;
    [SerializeField] private float PillarKnockback = 5f;
    private float knockbackScale = 1000000F;
    [SerializeField] private float Eradius = 10f;

    [SerializeField] private GameObject[] PillarPieces;

    [SerializeField] private float EForceUp = 2f;

    [SerializeField] private AudioSource PillarSound = null;


    private static bool CanStun = true;

    [SerializeField] public BossFetcher BossCharacter = null;

    public float getPillarKnockback()
    {
        return PillarKnockback;
    }

    public AudioSource getSound()
    {
        return PillarSound;
    }

    public float getPillarExplosionRadius()
    {
        return Eradius;
    }

    public float getExplosionForceUp()
    {
        return EForceUp;
    }


    public float getKnockbackScale()
    {
        return knockbackScale;
    }

    public bool getCanStun()
    {
        return CanStun;
    }

    



    // Start is called before the first frame update
    void Start()
    {
        if (BossCharacter.BossCharacter == null)
        {
            Debug.Log("Boss pillar is missing the conection to boss fetcher");
        }




        foreach(GameObject Pillar in PillarPieces)
        {
            if(Pillar == null)
            {
                Debug.Log("The PillarPieces array has a null value stored. This array needs to have each pillar piece in the Boss Pillar stored in this array");
            }
        }
    }


    public void DoStunEffects()
    {
        


        //GetComponent<Rigidbody>().AddForce(direction * (PillarKnockback * knockbackScale), ForceMode.Impulse);//sets velocity instead of adding force.
        foreach (GameObject Pillar in PillarPieces)
        {//each pillar piece stores, in the actual pillar prefab, the all the pillar pieces. When the boss hits the pillar, each pillar piece gets effected by the explosion at the contact point between the pillar and the boss
            Pillar.GetComponent<Rigidbody>().AddExplosionForce((PillarKnockback * knockbackScale), BossCharacter.BossCharacter.transform.position, Eradius, EForceUp, ForceMode.Force);
        }

        BossCharacter.RunRubbleSpawner();
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ConfigurableJoint>())
        {
            //check to see if object is the boss
            if (collision.transform.GetComponentInParent<ActiveRagdoll>().getRoot().tag == BossCharacter.BossCharacter.tag || collision.transform.GetComponentInParent<ActiveRagdoll>().getRoot().tag == BossCharacter.BossTag)
            {
                if (!BossCharacter.getStunned())
                {
                    if (collision.gameObject.GetComponent<ConfigurableJoint>())//checks to see if the collision object has a configurable joint (all players and enemies have this)
                    {

                        Transform characterCenter = collision.transform.GetComponentInParent<ActiveRagdoll>().getRoot();//find the root of the rig that hit the enviormental hazard




                        if (characterCenter)
                        {
                            Vector3 direction = (collision.GetContact(0).point - characterCenter.position).normalized;//finds the direction of the collision

                            //characterCenter.GetComponent<Rigidbody>().AddForce(direction * BumperForce, ForceMode.Impulse);//adds bumperforce in the direction of between the trap contact point and center of the envirmental hazard
                            if (direction.y < 0f)
                            {
                                direction.y = 0f;
                            }



                            if (PillarSound)
                            {
                                PillarSound.Play();
                            }


                            //GetComponent<Rigidbody>().AddForce(direction * (PillarKnockback * knockbackScale), ForceMode.Impulse);//sets velocity instead of adding force.
                            foreach (GameObject Pillar in PillarPieces)
                            {//each pillar piece stores, in the actual pillar prefab, the all the pillar pieces. When the boss hits the pillar, each pillar piece gets effected by the explosion at the contact point between the pillar and the boss
                                Pillar.GetComponent<Rigidbody>().AddExplosionForce((PillarKnockback * knockbackScale), collision.GetContact(0).point, Eradius, EForceUp, ForceMode.Force);
                            }

                            StartCoroutine(StunCooldown());
                            BossCharacter.RunRubbleSpawner();
                        }
                    }

                }
            }
        }
    }

    */

/*
This IEnumerator runs after a collision with the boss. It sets stun to true, waits a set time determined in the editor, and turns stunned back to false afterward. 
*/
    private IEnumerator StunCooldown()
    {
        BossCharacter.setStunned(true);

        yield return new WaitForSeconds(BossCharacter.StunTimer);
        BossCharacter.setStunned(false);
    }

}
