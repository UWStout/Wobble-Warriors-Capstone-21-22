using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    Director Director;
    private void Awake()
    {
        Director = GameObject.Find("Director").GetComponent<Director>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Director.IncreaseGold(1);
            Destroy(gameObject);
        }
    }
}
