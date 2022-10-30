using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeSight : MonoBehaviour
{
    [SerializeField] GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Target") && !boss.GetComponent<TestBossAI>().playerInSight)
        {
            Debug.Log("yes");
            boss.GetComponent<TestBossAI>().playerInSight = true;
            boss.GetComponent<TestBossAI>().targetedPlayer = other.gameObject;
        }
    }
}
