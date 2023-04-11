using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 2.0f;
    [SerializeField] float speed = 2.0f;
    [SerializeField] float knockback = 2.0f;
    [SerializeField] bool isKey;
    public GameObject weaponAppearance;
    private const float DMGMod = 10.0f;
    private const float SPDMod = 1.0f;
    private const float KBMod = 200000.0f;

    [SerializeField] Transform holder;

    private bool canHit = false;
    [SerializeField] AudioClip hitSound;
    [SerializeField] GameObject hitVFX;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float KnockBack
    {
        get { return knockback; }
        set { knockback = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public bool IsKey
    {
        get { return isKey; }
        set { isKey = value; }
    }

    private void Start()
    {
        canHit = false;
        if (GetComponentInParent<ActiveRagdoll>() != null)
        {
            holder = GetComponentInParent<ActiveRagdoll>().getRoot();
        }
    }

    public void SetCanHit(bool setCanHit)
    {
        canHit = setCanHit;
    }

    [SerializeField] float LowPitch = .75f;
    [SerializeField] float HighPitch = 1.25f;
    public void SphereAttack(Vector3 position, float radius, float damageMult, bool canRevive)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius); //make this not hardcoded, loser
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<CharacterMovement>() && hitCollider.gameObject != holder.gameObject)
            {
                Vector3 hitVector = hitCollider.transform.position - transform.position;
                hitVector = new Vector3(hitVector.x, 0, hitVector.z);

                PlayerCharacter player = hitCollider.gameObject.GetComponent<PlayerCharacter>();
                if (player)
                {
                    if (holder.GetComponent<PlayerCharacter>())
                    {
                        damageMult = 0;
                    }
                    if (player.knockedOut && canRevive)
                    {
                        player.Revive(true);
                    }
                }

                if (hitCollider.gameObject.CompareTag("Boss"))
                {
                    if (hitCollider.gameObject.GetComponent<TestBossAI>().stunned)
                    {
                        hitCollider.gameObject.GetComponent<CharacterMovement>().health -= (int)(damage * DMGMod);
                    }
                }
                else
                {
                    hitCollider.GetComponent<Rigidbody>().AddForce(hitVector.normalized * knockback * KBMod);
                    if (hitCollider.GetComponent<AICharacter>())
                    {
                        hitCollider.GetComponent<AICharacter>().currentTarget = player;
                        Debug.Log(holder);

                    }
                }
                hitCollider.GetComponent<CharacterMovement>().health -= damage * DMGMod * damageMult;
                canHit = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canHit)
        {
            Lock lockthingy = collision.gameObject.GetComponent<Lock>();
            if (lockthingy != null&&IsKey)
            {
                lockthingy.bonk(1.0f);
            }
            //if the component is part of a rig and the weapon is enabled

            ActiveRagdoll ragdoll = collision.transform.GetComponentInParent<ActiveRagdoll>();
            if (ragdoll != null)
            {
                //get the root of the target being hit
                Transform characterRoot = ragdoll.getRoot();
                //make sure character root isnt null and that the character isnt hitting itself
                if (characterRoot && holder != characterRoot)
                {
                    if (hitSound) //play a hit sound if there is one
                    {
                        GetComponent<AudioSource>().pitch = Random.Range(LowPitch, HighPitch);
                        GetComponent<AudioSource>().PlayOneShot(hitSound);
                    }
                    if (hitVFX) //Create a particle effect if assigned
                    {
                        //Debug.Log("Boop");
                        Destroy(Instantiate(hitVFX, collision.transform.position, collision.transform.rotation), 2);
                    }
                    if (characterRoot.GetComponent<AICharacter>() && holder.gameObject.GetComponent<PlayerCharacter>())
                    {
                        characterRoot.GetComponent<AICharacter>().currentTarget = holder.gameObject.GetComponent<PlayerCharacter>();
                        Debug.Log(holder);

                    }

                    //calculate vector of knockback
                    Vector3 hitVector = characterRoot.position - transform.position;
                    hitVector = new Vector3(hitVector.x, 0, hitVector.z);
                    //damage character, and set some regen variables 
                    if (characterRoot.gameObject.CompareTag("Boss") && characterRoot.gameObject.GetComponent<TestBossAI>().stunned)
                    {
                        characterRoot.GetComponent<CharacterMovement>().health -= (int)(damage * DMGMod);
                    }
                    else 
                    {
                        if (characterRoot.gameObject.GetComponent<AICharacter>() || !holder.GetComponent<PlayerCharacter>())
                        {
                            characterRoot.GetComponent<CharacterMovement>().health -= (int)(damage * DMGMod);
                        }
                        characterRoot.GetComponent<Rigidbody>().AddForce(hitVector.normalized * knockback * KBMod);
                    }

                    if (characterRoot.GetComponent<PlayerCharacter>())
                    {
                        characterRoot.GetComponent<PlayerCharacter>().SetTimeSinceDamage(1);
                        characterRoot.GetComponent<PlayerCharacter>().WeaponHit();
                    }
                    //end swing
                    canHit = false;

                    if (holder.GetComponent<ControllerRumble>())
                    {
                        holder.GetComponent<ControllerRumble>().Rumble(.25f, 1, 1);
                    }
                }
            }
        }
    }
}
