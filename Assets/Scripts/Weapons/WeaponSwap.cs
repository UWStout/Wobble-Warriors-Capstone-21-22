using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DESCRIPTION: used to swap weapons the player is holding
//AUTHOR: Brian Bauch
public class WeaponSwap : MonoBehaviour
{
    [SerializeField] GameObject pickupWeapon; //the weapon that will be picked up upon switching weapons

    public GameObject currentWeapon; //the weapon currently held by the player
    public GameObject weaponParent; //the parent joint of the weapon
    public GameObject weaponAppearance; //the appearance object of the weapon

    public Weapon cWeaponScript;
    public MeshFilter cWeaponMesh;
    public MeshRenderer cWeaponMats;

    private bool inMenu = false; //will get set from ReadyUpMenu

    bool canSwap = true; //bool that says if this player can swap

    public bool hasKey;

    // Start is called before the first frame update
    void Start()
    {
        //initialize the current weapon and parent joint
        currentWeapon = (gameObject.GetComponentInChildren(typeof(Weapon)) as Weapon).gameObject;
        weaponParent = currentWeapon.transform.parent.gameObject;

        cWeaponScript = currentWeapon.GetComponent<Weapon>();
        cWeaponMesh = weaponAppearance.GetComponent<MeshFilter>();
        cWeaponMats = weaponAppearance.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //swaps the currently held weapon and the weapon to be picked up
    public void SwapWeapons()
    {
        Debug.Log("in menu: " + inMenu);
        //if canSwap is true and the player's interact radius variable reutrns true
        if((canSwap && GetComponentInChildren<InteractRadius>().interactable) || inMenu)
        {
            inMenu = false;
            canSwap = false;
            SwapWeapons(pickupWeapon.GetComponent<Weapon>());
            //run the swap cooldown coroutine
            StartCoroutine(SwapCooldown());

        }

    }

    public void SwapWeapons(Weapon weapon)
    {
        Mesh tempMesh;
        float tempDmg;
        float tempKb;
        float tempSpeed;
        bool tempIsKey;
        Material[] tempMaterials;

        Weapon pWeaponScript = weapon;
        MeshFilter pWeaponMesh = pWeaponScript.weaponAppearance.GetComponent<MeshFilter>();
        MeshRenderer pWeaponMats = pWeaponScript.weaponAppearance.GetComponent<MeshRenderer>();
            
        //set the values for the temp using the held weapon
        tempMesh = cWeaponMesh.mesh;
        tempDmg = cWeaponScript.Damage;
        tempKb = cWeaponScript.KnockBack;
        tempSpeed = cWeaponScript.Speed;
        tempIsKey = cWeaponScript.IsKey;
        tempMaterials = cWeaponMats.materials;


        //set the new value for the held weapon using the pickup weapon
        if (pWeaponScript.IsKey)
        {
            hasKey = true;
        }
        else
        {
            hasKey = false;
        }

        cWeaponMesh.mesh = pWeaponMesh.mesh;
        cWeaponScript.Damage = pWeaponScript.Damage;
        cWeaponScript.KnockBack = pWeaponScript.KnockBack;
        cWeaponScript.Speed = pWeaponScript.Speed;
        cWeaponScript.IsKey = pWeaponScript.IsKey;
        cWeaponMats.materials = pWeaponMats.materials;


        //set the new pickup values with the temp values
        pWeaponMesh.mesh = tempMesh;
        pWeaponScript.Damage = tempDmg;
        pWeaponScript.KnockBack = tempKb;
        pWeaponScript.Speed = tempSpeed;
        pWeaponScript.IsKey = tempIsKey;
        pWeaponMats.materials = tempMaterials;
    }

    public void SetWeapon(Weapon weapon)
    {
        Weapon pWeaponScript = weapon;
        MeshFilter pWeaponMesh = pWeaponScript.weaponAppearance.GetComponent<MeshFilter>();
        MeshRenderer pWeaponMats = pWeaponScript.weaponAppearance.GetComponent<MeshRenderer>();

        //set the new value for the held weapon using the pickup weapon
        if (pWeaponScript.IsKey)
        {
            hasKey = true;
        }
        else
        {
            hasKey = false;
        }

        cWeaponMesh.mesh = pWeaponMesh.mesh;
        cWeaponScript.Damage = pWeaponScript.Damage;
        cWeaponScript.KnockBack = pWeaponScript.KnockBack;
        cWeaponScript.Speed = pWeaponScript.Speed;
        cWeaponScript.IsKey = pWeaponScript.IsKey;
        cWeaponMats.materials = pWeaponMats.materials;
    }



    IEnumerator SwapCooldown()
    {
        //wait 2 seconds and set canSwap back to true
        yield return new WaitForSeconds(2f);
        canSwap = true;
    }

    //set the weapon to be picked up
    public void SetPickup(GameObject pickup)
    {
        pickupWeapon = pickup;
    }

    public void SetInMenu(bool status)
    {
        inMenu = status;
    }
}
