using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

//public class HazardDynamicTests
//{

//    private GameObject slime;
//    private GameObject WallWacker;
//    private GameObject EjectionSeat;
//    private GameObject MineCartPath;
//    private GameObject Conveyor;
//    private GameObject capstan;
//    private GameObject bossPillar;
//    private GameObject player;

//    private List<GameObject> rootObjects;
//    private Scene scene;

//    //grabbing the hazard scripts
//    [OneTimeSetUp]
//    public void LoadAssets()
//    {

//        SceneManager.LoadScene("Trap Tester");

//        scene = SceneManager.GetActiveScene();

//        scene.GetRootGameObjects(rootObjects);

//        foreach(GameObject trap in rootObjects)
//        {
//            if(trap.tag == "BossPrefab")
//            {
//                bossPillar = trap;
//            }
//            else if(trap.tag == "Minecart")
//            {
//                MineCartPath = trap;

//            }
//            else if (trap.tag == "slime")
//            {
//                slime = trap;
//            }
//            else if (trap.tag == "ejection seat")
//            {
//                EjectionSeat = trap;
//            }
//            else if (trap.tag == "wall wacker")
//            {
//                WallWacker = trap;
//            }
//            else if (trap.tag == "Conveyor")
//            {
//                Conveyor = trap;
//            }
//            else if (trap.tag == "Capstan")
//            {
//                capstan = trap;
//            }

//        }

//        //grab traps

//        Debug.Log(capstan);

//    }

//    [Test]
//    public void SimpleNullTests()
//    {
        



//        Assert.IsNotNull(slime);

//        Assert.IsNotNull(bossPillar);

//        Assert.IsNotNull(WallWacker);

//        Assert.IsNotNull(EjectionSeat);


//        Assert.IsNotNull(MineCartPath);

//        Assert.IsNotNull(Conveyor);

//        Assert.IsNotNull(capstan);

//    }




//    // A Test behaves as an ordinary method
//    [Test]
//    public void HazardDynamicTestsSimplePasses()
//    {
//        // Use the Assert class to test conditions
//    }

//    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
//    // `yield return null;` to skip a frame.
//    [UnityTest]
//    public IEnumerator HazardDynamicTestsWithEnumeratorPasses()
//    {
//        // Use the Assert class to test conditions.
//        // Use yield to skip a frame.
//        yield return null;
//    }
//}
