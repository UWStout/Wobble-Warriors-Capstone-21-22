using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RubbleSpawn : MonoBehaviour
{


    [SerializeField] private GameObject[] Rubble;//stores the prefabs for the rubble that is spawned

    [SerializeField] private float[] CordRange = { 0, 5 };// rubble spawned coordinates will be randomized between these two points. First number must be 0 with second number

    [SerializeField] private float[] NumberOfRubbleSpawned = { 4, 8 };//rubble spawned will be randomized between these two points. First number in Array must be lower then the second number

    [SerializeField] private float SpawnDelay = 0.5f;//determines the rate at which rubble spawns;

    //these arrays store the list of x and z coordinates that will be used to spawn the rubble
    private float[] XCordinates;
    private float[] ZCordinates;

    //getters for unit testing
    public GameObject[] getRubble()
    {
        return Rubble;
    }

    public float[] getCordRange()
    {
        return CordRange;
    }

    public float[] getNumberOfRubbleSpawned()
    {
        return NumberOfRubbleSpawned;
    }

    public float GetSpawnDelay()
    {
        return SpawnDelay;
    }






    // Start is called before the first frame update
    void Start()
    {
        XCordinates = new float[(int)NumberOfRubbleSpawned[1]];//modifies the array size so it is equal to the ammount of rubble that is spawned
        ZCordinates = new float[(int)NumberOfRubbleSpawned[1]];

        //these if statements are to catch when the cord range and number of rubble spawned minimum is higher then the maximum and terminate the playmode before randomization is done
        if (CordRange[0] > CordRange[1])
        {
            Debug.Log("CordRange[0]: " + CordRange[0] + " is higher then CordRange[1]: " + CordRange[1] + "CordRange[0] must be lower then CordRange[1]");
            //EditorApplication.ExitPlaymode();
        }



        if (NumberOfRubbleSpawned[0] > NumberOfRubbleSpawned[1])
        {
            Debug.Log("NumberOfRubbleSpawned[0]: " + NumberOfRubbleSpawned[0] + " is higher then NumberOfRubbleSpawned[1]: " + NumberOfRubbleSpawned[1] + "NumberOfRubbleSpawned[0] must be lower then NumberOfRubbleSpawned[1]");
            //EditorApplication.ExitPlaymode();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable() //disables audio if script get's disabled
    {
        if (!GetComponent<AudioSource>().enabled)
        {
            GetComponent<AudioSource>().Stop();
        }

    }

    public void spawnRubble()//this code is called to spawn a randomized amount of rubble around the gameobject this script is attached to
    {
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
        }
        //randomizes the number of rubble spawned this iteration
        int NumberLooped = (int)Random.Range(NumberOfRubbleSpawned[0], NumberOfRubbleSpawned[1]);


        //loop to add the x and z cordinates for each rubble spawned
        for(int i = 0; i < NumberOfRubbleSpawned[1]; i++)
        {
            //random float between 0 and 10 and then a if to seperate any number under 5 to -1 and above 5 to 1 This will determine what grid the rubble will spawn under. I.E. -1,-1 or -1,1 or 1,-1 or 1,1
            float XGrid = Random.Range(0, 10);
            
            float ZGrid = Random.Range(0, 10);



            if (XGrid <5)
            {
                XGrid = -1;
            }
            else
            {
                XGrid = 1;
            }
            if(ZGrid < 5)
            {
                ZGrid = -1;
            }
            else
            {
                ZGrid = 1;
            }

            //randomizes the coordinates after determining what grid it will be
            RandomizeCordGridCord(XGrid,ZGrid , i);
        }
        StartCoroutine(RubbleSpawnDelay(SpawnDelay));

        
        
    }

    //randomize a cordinates between ZCordRange and XCordRange and set it on a grid while storing it in the cordinates array
    private void RandomizeCordGridCord(float XGrid, float ZGrid, int iterator)
    {
        float XCord = Random.Range(CordRange[0], CordRange[1]);
        XCord *= XGrid;

        XCordinates[iterator]= XCord;

        float ZCord = Random.Range(CordRange[0], CordRange[1]);
        ZCord *= ZGrid;

        ZCordinates[iterator] = ZCord;

        


    }

    //spawns a rubble piece every (spawn delay) seconds while randomizing it's rotation using the spawner script
    private IEnumerator RubbleSpawnDelay(float t)
    {
        for (int i = 0; i < NumberOfRubbleSpawned[1]; i++)
        {
            Vector3 pos = transform.position;
            pos.x = XCordinates[i];
            pos.y = 0;
            pos.z = ZCordinates[i];
            Quaternion rotation = new Quaternion();
            rotation[0] = Random.Range(0, 1);
            rotation[1] = Random.Range(0, 1);
            rotation[2] = Random.Range(0, 1);

            GetComponent<Spawner>().SpawnObject(Rubble[i % Rubble.Length], pos, rotation);

            yield return new WaitForSeconds(t);
        }
        GetComponent<AudioSource>().enabled = false;
    }

    
}
