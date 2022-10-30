using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnList;//place prefabs you want spawned here

    [SerializeField] private float SpawnTimer = 0.5f;//time between a object spawning

    [SerializeField] private Transform SpawnedUnder = null;

    private int SpawnIndex = 0;//iterator that moves through the array of prefabs set to spawn for the spawner script

    [SerializeField] private bool spawnloop = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (spawnloop)
        {
            StartCoroutine("spawnObject");//starts the loop that spawns prefabs from spawn list
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //spawns a prefab in the spawnList array every x seconds. When a object is spawned will move to the next prefab in the array
    private IEnumerator spawnObject()
    {
        
        while (enabled)
        {
            Instantiate(spawnList[SpawnIndex], transform.position, Quaternion.identity, SpawnedUnder);//spawns a object stored in a gameOject array


            yield return new WaitForSeconds(SpawnTimer);//waits for spawnTimer seconds 

            SpawnIndex++;

            if(SpawnIndex == spawnList.Length)//if count gets to the length of the spawnList array resets count to prevent a error
            {
                SpawnIndex = 0;
            }
        }
        if(!enabled)
        {
            StopCoroutine(spawnObject());
        }
    }

    public int getSpawnIndex()
    {
        return SpawnIndex;
    }

    public float getSpawnTimer()
    {
        return SpawnTimer;
    }

    public GameObject getSpawned()
    {
        return spawnList[0];
    }

    public Transform getSpawnedUnder()
    {
        return SpawnedUnder;
    }

    public bool getSpawnLooping()
    {
        return spawnloop;
    }

    public void SpawnObject(GameObject spawned)
    {
        Instantiate(spawned, transform.position, transform.rotation, SpawnedUnder);//spawns a object stored in a gameOject array
    }

    public void SpawnObject(GameObject spawned, Vector3 position, Quaternion rotation)//overloaded version of SpawnObject that takes a position and rotation also
    {
        Vector3 pos = transform.position;
        pos += position;
        Instantiate(spawned, pos, rotation, SpawnedUnder);
        Debug.Log(pos);
    }
}
