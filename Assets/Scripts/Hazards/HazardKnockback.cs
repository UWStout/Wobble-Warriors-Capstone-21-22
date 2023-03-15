using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardKnockback : MonoBehaviour
{
 /*   [SerializeField] private float Knockback = 0.25f;//scalar that modifies the base knockback force

    [SerializeField] private bool IgnoreSpeed = true;//this variable is for the modification of knockback though speed. If false, RotationSpped needs to be grabbed from another script (this script is stored in rotation speed)

    [SerializeField] private Capstan rotationSpeed = null;//variable to fetch the rotationSpeed script on the capstan


    [SerializeField] private GameObject HitParticleFX = null;

    private float KnockBackScale = 10000;//knockback that is applied to the player or enemy upon colliding with a hazard


    private void OnEnable()
    {
        if (!IgnoreSpeed)
        {
            if (rotationSpeed == null)
            {
                Debug.Log("IgnoreSpeed is false yet there is no script attached to rotation speed. Either Set Ignore Speed to (true) or link a script with (float rotationSpeed) to rotation speed");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled == true) {
            
            if (collision.gameObject.GetComponent<ConfigurableJoint>())//checks to see if the collision object has a configurable joint (all players and enemies have this)
            {
                Transform characterCenter = collision.transform.GetComponentInParent<ActiveRagdoll>().getRoot();//find the root of the rig that hit the enviormental hazard

                if (characterCenter)
                {
                     Vector3 direction = (characterCenter.position - collision.GetContact(0).point).normalized;//finds the direction of the collision

                    //characterCenter.GetComponent<Rigidbody>().AddForce(direction * BumperForce, ForceMode.Impulse);//adds bumperforce in the direction of between the trap contact point and center of the envirmental hazard
                    if (direction.y < 0f)
                    {
                        direction.y = 0f;
                    }

                    GetComponent<AudioSource>().Play();

                    PlayerCharacter char1 = characterCenter.GetComponent<PlayerCharacter>();
                    if (char1)
                    {
                        char1.EnvHit();
                    }

                    if (!IgnoreSpeed & rotationSpeed != null)
                    {//runs this knockback if the hazard is a capstan modifies knockback by the speed of the capstan's rotation
                        characterCenter.GetComponent<Rigidbody>().AddForce(direction * (Knockback * KnockBackScale * rotationSpeed.RotationSpeed), ForceMode.Impulse);//sets velocity instead of adding force.
                    }

                    else
                    {
                        characterCenter.GetComponent<Rigidbody>().AddForce(direction * (Knockback * KnockBackScale), ForceMode.Impulse);//sets velocity instead of adding force.
                    }
                    if (HitParticleFX)
                    {
                        GameObject particles = Instantiate(HitParticleFX, collision.GetContact(0).point, Quaternion.identity, gameObject.transform);
                        Destroy(particles, 2);//particle effect for hit particle effects
                    }
                }
            }
        }
    }*/
}
