using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCartAnimatorControler : MonoBehaviour
{
    [SerializeField] private Animator myGate = null;//animator for the minecard
    [SerializeField] private Animator myCart = null;//animator for the gate along the railway track

    [SerializeField] private string closeGate = "CloseGate";//name for the close gate animation
    [SerializeField] private string mineCartRun = "MineCartRunning";//name for minecart animation
    [SerializeField] private string openGate = "OpenGate";//name for the open gate animation

    [SerializeField] private Spawner spawner = null;

    [SerializeField] private GameObject MineCart = null;

    //boolean logic for the animation to run in the order of gates lowering, minecart runing through, and the gate rasing up
    bool gateShut = true;
    bool gateDown = false;
    bool MineCartDone = false;

    [SerializeField] public HazardKnockback HKnockback = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(MinecartRunTime());//spins minecart wheels

    }

    private void OnDisable() //disables audio if script get's disabled
    {
        if (!GetComponent<AudioSource>().enabled)
        {
            GetComponent<AudioSource>().Stop();
        }

    }
    public bool getSGate()
    {
        return gateShut;
    }

    public bool getDGate()
    {
        return gateDown;
    }

    public bool getMineCartDone()
    {
        return MineCartDone;
    }

    private IEnumerator PlaySound()
    {
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        GetComponent<AudioSource>().enabled = false;
    }



    private IEnumerator MinecartRunTime()
    {
        
        while (enabled)//this loop iterates through the gate opening, closing, and the mine cart runing though the level
        {
            yield return new WaitForSeconds(3);//waits 3 seconds before the gate lowers into the floor
            if (gateShut)
            {



                myGate.Play(openGate, 0, 0.0f);

                StartCoroutine(PlaySound());
                gateShut = false;
                MineCartDone = true;
            }
            yield return new WaitForSeconds(5);//wait 5 seconds before the minecart runs through the level
            if (MineCartDone)
            {

                spawner.SpawnObject(MineCart);
                MineCartDone = false;
                gateDown = true;
            }
            yield return new WaitForSeconds(5);//wait 5 seconds before the gate rises up again
            if (gateDown)
            {
                myGate.Play(closeGate, 0, 0.0f);
                gateDown = false;
                gateShut = true;

                StartCoroutine(PlaySound());


            }
            yield return new WaitForSeconds(2);//wait 2 seconds before the loop starts again
        }
        if(!enabled)
        {
            StopCoroutine(MinecartRunTime());
        }
        
        
        
    }

}
