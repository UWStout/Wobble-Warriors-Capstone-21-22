using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleSpawner : MonoBehaviour
{
    [SerializeField] private Transform RootPosition;
    private Transform actualPosition;

    [SerializeField] private GameObject Rubble;

    [SerializeField] private float[] ZCord;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnRubble()
    {
        //float ZCord = Random.Range();
        GetComponent<Spawner>().SpawnObject(Rubble);
    }
}
