using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFetcher : MonoBehaviour
{
    [SerializeField] public GameObject BossCharacter;

    [SerializeField] public float StunTimer = 5f;

    [SerializeField] bool IsStunned = false;

    [SerializeField] public string BossTag = "Boss";

    [SerializeField] private RubbleSpawn rubblespawner;

    private bool StunDuration = false;
    private void start()
    {
        if(BossCharacter == null)
        {
            Debug.Log("Boss Character shouldn't be null");
        }

        if (IsStunned)
        {
            Debug.Log("IsStunned should start out false");
        }
    }



    public void RunRubbleSpawner()
    {
        GetComponentInChildren<AudioSource>().enabled = true;
        GetComponentInChildren<AudioSource>().Play();
        rubblespawner.spawnRubble();
    }//on boss collision rubble spawner activates



    public bool getStunned()
    {
        return IsStunned;
    }

    public void setStunned(bool Stunned)
    {
        IsStunned = Stunned;
    }
}
