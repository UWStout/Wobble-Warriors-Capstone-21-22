using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    private LevelList lList;
    [SerializeField]private GameObject[] roomList;
    [SerializeField] private GameObject startingRoom;
    [SerializeField] private GameObject PortalRoom;
    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < 5; j++) {
            for (int i = 0; i < 5; i++)
            {
                GameObject room = Instantiate(startingRoom);
                room.transform.position = new Vector3(j * 30, 0, i * 30);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
