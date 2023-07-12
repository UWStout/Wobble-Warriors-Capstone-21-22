using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Find the director
        Director dir = GameObject.Find("Director").GetComponent<Director>();
        //Tell the director that this room is the starting room
        dir.SetCurrentRoom(this.gameObject);
        //Tell the director to Retry at the start of the scene as a failsafe
        dir.doRetry();
        dir.ResetCurrentExtraLife();
        dir.ResetPlayerWeapons();
        dir.ActivateRoom();
        GameObject gobstopper = this.GetComponent<RoomInfo>().GetGobStopper();
        foreach(GameObject boi in dir.PlayerList)
        {
            boi.transform.position = gobstopper.transform.position + new Vector3(dir.PlayerList.IndexOf(boi)*2, 0, 0);
        }
    }
}
