using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField]
    private float durability = 1.0f;
    [SerializeField] ModularDoor door;
    [SerializeField] GameObject closedLockModel;
    [SerializeField] GameObject openLockModel;
    [SerializeField] GameObject chainModel;
    [SerializeField] AudioClip unlockSound;
    [SerializeField] AudioSource soundEffectSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Reduces durability by damage received and destroys the object if durability is at or below 0
    public void bonk(float damage)
    {
        durability -= damage;
        if (durability <= 0){
            Destroy(closedLockModel);
            Destroy(chainModel);
            openLockModel.SetActive(true);
            door.keyHit();
            soundEffectSource.PlayOneShot(unlockSound);
        }
    }
}
