using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] float knockback = 0.25f;
    [SerializeField] GameObject HitParticleFX = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled == true)
        {
            //checks to see if the collision object is a ragdoll
            ActiveRagdoll characterHit = collision.transform.GetComponentInParent<ActiveRagdoll>();

            //if a character was hit
            if(characterHit != null)
            {
                Transform characterCenter = characterHit.getRoot(); //root of rig

                //direction of collision
                Vector3 direction = -collision.GetContact(0).normal;
                direction.y = 0;

                //set velocity of character
                characterCenter.GetComponent<Rigidbody>().AddForce(direction.normalized * knockback, ForceMode.VelocityChange);

                //play sounds
                PlayerCharacter char1 = characterCenter.GetComponent<PlayerCharacter>();
                if (char1)
                {
                    char1.EnvHit();
                }

                GetComponent<AudioSource>().Play();

                //play particles
                if (HitParticleFX)
                {
                    GameObject particles = Instantiate(HitParticleFX, collision.GetContact(0).point, Quaternion.identity, gameObject.transform);
                    Destroy(particles, 2);//particle effect for hit particle effects
                }
            }
        }
    }
}
