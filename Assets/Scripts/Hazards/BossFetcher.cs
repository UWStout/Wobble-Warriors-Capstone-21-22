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
    private void Start()
    {
        BossCharacter = FindObjectOfType<TestBossAI>().gameObject;
    }

    public void RunRubbleSpawner()
    {
        GetComponentInChildren<AudioSource>().enabled = true;
        GetComponentInChildren<AudioSource>().Play();
        rubblespawner.spawnRubble();
    }

    public bool getStunned()
    {
        return IsStunned;
    }

    public void setStunned(bool Stunned)
    {
        IsStunned = Stunned;
    }
}
