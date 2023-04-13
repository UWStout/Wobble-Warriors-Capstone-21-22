using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 5;
    public float lifespan = 5;

    private void Awake()
    {
        StartCoroutine(Life());
    }

    IEnumerator Life() 
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled == true)
        {
            //checks to see if the collision object is a player
            ActiveRagdoll characterHit = collision.transform.GetComponentInParent<ActiveRagdoll>();
            if(characterHit != null)
            {
                PlayerCharacter player = characterHit.getCharacter().GetComponent<PlayerCharacter>(); //ew

                //if a player was hit
                if (player != null)
                {
                    player.health -= damage;

                    //play sounds
                    if (player)
                    {
                        player.EnvHit();
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
