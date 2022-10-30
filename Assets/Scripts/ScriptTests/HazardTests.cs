using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;


public class HazardTests
{
    private GameObject slime;
    private GameObject WallWacker;
    private GameObject EjectionSeat;
    private GameObject MovingWall;
    private GameObject MineCartPath;
    private GameObject Conveyor;
    private GameObject capstan;
    private GameObject bossPillar;
    private GameObject player;
    private GameObject MineCart;
    private GameObject puddle;
    private Vector3 parentTransform;

    //grabbing the hazard scripts
    [OneTimeSetUp]
    public void LoadAssets()
    {
        string[] results;
        string prefabPath;
        //boss pillar fetch
        results = AssetDatabase.FindAssets("destructible_pillar_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        bossPillar = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(0, 0, 0);
        Debug.Log(prefabPath);
        bossPillar = MonoBehaviour.Instantiate(bossPillar);
        bossPillar.transform.position = parentTransform;

        //puddle fetch
        results = AssetDatabase.FindAssets("puddle_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        puddle = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(200, 0, 0);
        Debug.Log(prefabPath);
        puddle = MonoBehaviour.Instantiate(puddle);
        puddle.transform.position = parentTransform;

        //slime fetch
        results = AssetDatabase.FindAssets("slime_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        slime = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(20, 0, 0);
        slime = MonoBehaviour.Instantiate(slime);
        slime.transform.position = parentTransform;


        //wall wacker fetch
        results = AssetDatabase.FindAssets("wall_wacker_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        WallWacker = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(40, 0, 0);
        WallWacker = MonoBehaviour.Instantiate(WallWacker);
        WallWacker.transform.position = parentTransform;


        //Ejection seat fetch
        results = AssetDatabase.FindAssets("ejection_seat_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        EjectionSeat = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(60, 0, 0);
        EjectionSeat = MonoBehaviour.Instantiate(EjectionSeat);
        EjectionSeat.transform.position = parentTransform;


        //Moving wall fetch
        results = AssetDatabase.FindAssets("moving_wall_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        MovingWall = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(80, 0, 0);
        MovingWall = MonoBehaviour.Instantiate(MovingWall);
        MovingWall.transform.position = parentTransform;


        //MineCartPath fetch
        results = AssetDatabase.FindAssets("minecart_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        MineCartPath = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(100, 0, 0);
        MineCartPath = MonoBehaviour.Instantiate(MineCartPath);
        MineCartPath.transform.position = parentTransform;


        //MineCart fetch
        results = AssetDatabase.FindAssets("minecart_no_rails_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        MineCart = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(120, 0, 0);
        MineCart = MonoBehaviour.Instantiate(MineCart);
        MineCart.transform.position = parentTransform;


        //conveyor fetch
        results = AssetDatabase.FindAssets("conveyorbelt_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        Conveyor = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(140, 0, 0);
        Conveyor = MonoBehaviour.Instantiate(Conveyor);
        Conveyor.transform.position = parentTransform;


        //capstan fetch
        results = AssetDatabase.FindAssets("capstan_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        capstan = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        parentTransform = new Vector3(160, 0, 0);
        capstan = MonoBehaviour.Instantiate(capstan);
        capstan.transform.position = parentTransform;

        results = AssetDatabase.FindAssets("Player_Character_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        player = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        player = MonoBehaviour.Instantiate(player);




    }

    [Test]
    public void SimpleNullTests()
    {
        Assert.IsNotNull(slime);

        Assert.IsNotNull(bossPillar);

        Assert.IsNotNull(WallWacker);

        Assert.IsNotNull(EjectionSeat);

        Assert.IsNotNull(MovingWall);

        Assert.IsNotNull(MineCartPath);

        Assert.IsNotNull(MineCart);

        Assert.IsNotNull(Conveyor);

        Assert.IsNotNull(capstan);

    }


    [Test]
    public void BossPillarTest()//pillar tests
    {

        BossFetcher BFScript = bossPillar.GetComponent<BossFetcher>();
        Debug.Log(BFScript);

        //boss fetcher tests
        Assert.IsFalse(BFScript.getStunned());

        Assert.AreEqual(BFScript.StunTimer, 5);
        
        Assert.AreEqual(BFScript.BossTag, "Boss");

        BossPillar BPScript = bossPillar.GetComponentInChildren<BossPillar>();
        Debug.Log(BPScript);

        //pillar piece tests
        Assert.AreEqual(BPScript.getPillarKnockback(), 1);

        Assert.AreEqual(BPScript.getCanStun(), true);

        Assert.AreEqual(BPScript.getExplosionForceUp(), 5);

        Assert.AreEqual(BPScript.getKnockbackScale(), 1000000F);

        Assert.AreEqual(BPScript.getPillarExplosionRadius(), 10);

        Spawner SpawnScript = bossPillar.GetComponentInChildren<Spawner>();
        Debug.Log(SpawnScript);

        //spawner test
        Assert.AreEqual(SpawnScript.getSpawnIndex(), 0);

        Assert.AreEqual(SpawnScript.getSpawnTimer(), 0.5f);

        Assert.IsFalse(SpawnScript.getSpawnLooping());

        Assert.NotNull(SpawnScript.getSpawnedUnder());


        RubbleSpawn RSScript = bossPillar.GetComponentInChildren<RubbleSpawn>();
        Debug.Log(RSScript);

        //RubbleSpawnTest
        Assert.NotNull(RSScript.getRubble());

        Assert.AreEqual(RSScript.getCordRange(), new float[] { 1, 10} );

        Assert.AreEqual(RSScript.getNumberOfRubbleSpawned(), new float[] { 4, 8 });

        Assert.AreEqual(RSScript.GetSpawnDelay(), 0.5f);

        Assert.Less(RSScript.getCordRange()[0], RSScript.getCordRange()[1]);

        Assert.AreEqual(RSScript.getCordRange().Length, 2);

        Assert.AreEqual(RSScript.getNumberOfRubbleSpawned().Length, 2);


        Assert.Less(RSScript.getNumberOfRubbleSpawned()[0], RSScript.getNumberOfRubbleSpawned()[1]);

        
    }

    [Test]
    public void PuddleTest()
    {
        changeFriction CFScript = puddle.GetComponent<changeFriction>();

        Assert.AreEqual(1f, CFScript.getDfriction());

        Assert.AreEqual(1f, CFScript.getSfriction());


        changeAlpha CAScript = puddle.GetComponentInChildren<changeAlpha>();

        Assert.AreEqual(20f, CAScript.GetNOpacityReduced());

        Assert.AreEqual(4.0f, CAScript.getDeletionTime());

        Assert.AreEqual(30f, CAScript.getFadeInScalar());

        Assert.AreEqual(2f, CAScript.getTimeBeforeFade());

        Assert.IsFalse(CAScript.getStartFading());

        Assert.AreEqual(0.8f, CAScript.getBaseOpacity());

    }

    //[Test]
    //public void DynamicPuddleTest()
    //{


    //    changeAlpha CAScript = puddle.GetComponentInChildren<changeAlpha>();

    //    Assert.NotNull(puddle.GetComponentInChildren<Material>());

    //    float puddleBaseAlpha = puddle.GetComponentInChildren<changeAlpha>().getOpacity();

    //    CAScript.ChangeOpac(0.1f);

    //    Assert.AreEqual(0.1f, CAScript.getOpacity());
    //}


    [Test]
    public void CapstanTest()
    {
        

        //capstan tests
        Capstan CapScript = capstan.GetComponentInChildren<Capstan>();

        //Debug.Log(CapScript);
        Assert.AreEqual(CapScript.GetComponent<Capstan>().getCapstanAnimator(), "CapRotation");


        Assert.AreEqual(CapScript.getMinSpeed(), 0.5);

        Assert.AreEqual(CapScript.getMaxSpeed(), 1.5);

        Assert.AreEqual(CapScript.getMinSpeedInterval(), 10);


        Assert.AreEqual(CapScript.getMaxSpeedInterval(), 20);

        HazardKnockback HazScript = capstan.GetComponentInChildren<HazardKnockback>();

        Debug.Log(HazScript);
        //Hazard Knockback tests
        Assert.AreEqual(HazScript.getKB(), 0.25);

        Assert.AreEqual(HazScript.getKBscale(), 10000);

        Assert.NotNull(HazScript.getRotationSpeed());

        Assert.IsFalse(HazScript.getISpeed());

        Assert.IsTrue(HazScript.GetComponent<AudioSource>().enabled);

        //disable hazard script
        DisableHazards DScript = capstan.GetComponent<DisableHazards>();

        Assert.AreEqual(2, DScript.getDisabledObjects().ToArray().Length);


    }

    /*[UnityTest]
    public IEnumerator DynamicCapstanTest()
    {
        float rotationSpeed = capstan.GetComponentInChildren<Capstan>().RotationSpeed;
        Debug.Log("test");
        Quaternion startRotation = capstan.transform.rotation;
        Debug.Log("test1");

        player.transform.position = capstan.transform.position;
        Debug.Log("test2");

        Vector3 playerStartLocation = player.transform.position;
        Debug.Log("test3");

        yield return null;

        Debug.Log("test4");

        Assert.AreNotEqual(rotationSpeed, capstan.GetComponentInChildren<Capstan>().RotationSpeed, "BaseSpeed: " + rotationSpeed + " ActualSpeed: " + capstan.GetComponentInChildren<Capstan>().RotationSpeed);
        Debug.Log("test5");

        Assert.AreNotEqual(startRotation, capstan.transform.rotation, "Base Rotation: " + startRotation + " Actual Rotation: " + capstan.transform.rotation);
        Debug.Log("test6");

        Assert.AreNotEqual(playerStartLocation, player.transform.position, "Start location: " + playerStartLocation + "Current location after knockback: " + player.transform.position);

    }*/

    [Test]
    public void ConveyorTest()
    {
        conveyor CScript = Conveyor.GetComponentInChildren<conveyor>();
        Debug.Log(CScript);

        //conveyor test
        Assert.AreEqual(CScript.getSpeed(), 3.0f);

        Assert.AreEqual(CScript.getSpeedScalar(), 100);


        //disable hazard script
        DisableHazards DScript = Conveyor.GetComponent<DisableHazards>();
        Assert.AreEqual(4, DScript.getDisabledObjects().ToArray().Length);



        //animate texture script
        AnimateTexture ATScript = Conveyor.GetComponentInChildren<AnimateTexture>();

        Assert.AreEqual(new Vector2(2,0), ATScript.getASpeed());
    }

    /*[UnityTest]
    public IEnumerator DynamicConveyortest()
    {
        player.transform.position = Conveyor.transform.position;

        Vector3 PlayerStartLocation = player.transform.position;

        yield return null;

        Assert.AreNotEqual(PlayerStartLocation, player.transform.position);


        yield return null;
    }*/

    [Test]
    public void MineCartPathTest()
    {
        MineCartAnimatorControler MACScript = MineCartPath.GetComponentInChildren<MineCartAnimatorControler>();
        Debug.Log(MACScript);

        //minecart animator test
        Assert.IsTrue(MACScript.getSGate());

        Assert.IsFalse(MACScript.getDGate());

        Assert.IsFalse(MACScript.getMineCartDone());

        Assert.IsFalse(MACScript.GetComponent<AudioSource>().enabled);


        Spawner SScript = MineCartPath.GetComponentInChildren<Spawner>();

        Debug.Log(SScript);

        //spawner test
        Assert.AreEqual(SScript.getSpawnIndex(), 0);

        Assert.AreEqual(SScript.getSpawnTimer(), 0.5);


        Assert.IsFalse(SScript.getSpawnLooping());

        Assert.NotNull(SScript.getSpawnedUnder());


        //disable hazard script
        DisableHazards DScript = MineCartPath.GetComponent<DisableHazards>();

        Assert.AreEqual(2, DScript.getDisabledObjects().ToArray().Length);

        MineCartPath.GetComponent<DisableHazards>().DisableHazard();

        Assert.IsFalse(MineCartPath.GetComponentInChildren<AudioSource>().enabled);
    }

   

    [Test]
    public void MineCartTest()
    {

        MoveRoute MRScript = MineCart.GetComponentInChildren<MoveRoute>();
        Debug.Log(MRScript);

        //moveroute test
        Assert.AreEqual(MRScript.getSpeed(), 20);

        Assert.AreEqual(MRScript.getRotationSpeed(), 0.5);

        Assert.AreEqual(MRScript.getErrorMarginToPoint(), .5);

        Assert.IsTrue(MineCart.GetComponent<AudioSource>().enabled);

        Assert.IsTrue(MRScript.getRouteDelete());

        Assert.IsFalse(MRScript.getRotateRoute());

        Assert.NotNull(MRScript.getSelf());

        HazardKnockback HazScript = MineCart.GetComponentInChildren<HazardKnockback>();
        Debug.Log(HazScript);

        //hazard knockback test
        Assert.AreEqual(HazScript.getKB(), 0.25);

        Assert.AreEqual(HazScript.getKBscale(), 10000);

        Assert.IsTrue(HazScript.getISpeed());

        Assert.IsTrue(HazScript.GetComponent<AudioSource>().enabled);
    }

    [Test]
    public void SlimeTest()
    {

        HazardKnockback HazScript = slime.GetComponentInChildren<HazardKnockback>();
        Debug.Log(HazScript);

        //hazard knockback test
        Assert.AreEqual(HazScript.getKB(), 0.25);

        Assert.AreEqual(HazScript.getKBscale(), 10000);

        Assert.IsTrue(HazScript.getISpeed());

        Assert.IsTrue(HazScript.GetComponent<AudioSource>().enabled);


        MoveRoute MRScript = slime.GetComponentInChildren<MoveRoute>();
        Debug.Log(MRScript);

        //moveroute test
        Assert.AreEqual(MRScript.getSpeed(), 3);

        Assert.AreEqual(MRScript.getRotationSpeed(), 1);


        Assert.AreEqual(MRScript.getErrorMarginToPoint(), .5);

        Assert.IsFalse(MRScript.getRouteDelete());

        Assert.IsTrue(MRScript.getRotateRoute());

        Assert.NotNull(MRScript.getSelf());


        Spawner SScript = slime.GetComponentInChildren<Spawner>();
        Debug.Log(SScript);


        //spawner test
        Assert.AreEqual(SScript.getSpawnIndex(), 0);

        Assert.AreEqual(SScript.getSpawnTimer(), 0.5f);

        Assert.NotNull(SScript.getSpawned());

        Assert.IsTrue(SScript.getSpawnLooping());

        Assert.NotNull(SScript.getSpawnedUnder());


        //disable hazard script
        DisableHazards DScript = slime.GetComponent<DisableHazards>();

        Assert.AreEqual(3, DScript.getDisabledObjects().ToArray().Length);

        slime.GetComponent<DisableHazards>().DisableHazard();

        Assert.IsFalse(slime.GetComponentInChildren<AudioSource>().enabled);
    }

    [Test]
    public void WallWackerTest()
    {
        //TriggerWackerTests
        TriggerWackerController TWCScript = WallWacker.GetComponentInChildren<TriggerWackerController>();
        Debug.Log(TWCScript);

        //TriggerWackerTests
        Assert.IsFalse(TWCScript.getCTrigger());

        Assert.IsTrue(TWCScript.getOTrigger());

        Assert.NotNull(TWCScript.getMyWacker());

        Assert.NotNull(TWCScript.getHKnockback());

        Assert.IsFalse(TWCScript.GetComponent<AudioSource>().enabled);


        Assert.AreEqual(TWCScript.getActivateTurn(), "ActivateTurn");

        Assert.AreEqual(TWCScript.getResetTurn(), "ResetTurn");

        Assert.IsTrue(TWCScript.GetTriggeredTrap());

        Assert.IsTrue(TWCScript.getIsReset());

        HazardKnockback HazScript = WallWacker.GetComponentInChildren<HazardKnockback>();
        Debug.Log(HazScript);

        //hazard knockback test
        Assert.AreEqual(HazScript.getKB(), 0.2f);

        Assert.AreEqual(HazScript.getKBscale(), 10000);

        Assert.IsTrue(HazScript.getISpeed());

        Assert.IsTrue(HazScript.GetComponent<AudioSource>().enabled);


        //disable hazard script
        DisableHazards DScript = WallWacker.GetComponent<DisableHazards>();

        Assert.AreEqual(2, DScript.getDisabledObjects().ToArray().Length);

        WallWacker.GetComponent<DisableHazards>().DisableHazard();

        Assert.IsFalse(WallWacker.GetComponentInChildren<AudioSource>().enabled);
    }

    [Test]
    public void EjectionSeatTest()
    {
        TriggerWackerController TWCScript = EjectionSeat.GetComponentInChildren<TriggerWackerController>();
        Debug.Log(TWCScript);

        //TriggerWackerTests
        Assert.IsFalse(TWCScript.getCTrigger());

        Assert.IsTrue(TWCScript.getOTrigger());

        Assert.NotNull(TWCScript.getMyWacker());

        Assert.NotNull(TWCScript.getHKnockback());


        Assert.AreEqual(TWCScript.getActivateTurn(), "EjectionActivate");

        Assert.AreEqual(TWCScript.getResetTurn(), "EjectionReset");

        Assert.IsTrue(TWCScript.GetTriggeredTrap());

        Assert.IsTrue(TWCScript.getIsReset());

        Assert.IsFalse(TWCScript.GetComponent<AudioSource>().enabled);

        HazardKnockback HazScript = EjectionSeat.GetComponentInChildren<HazardKnockback>();
        Debug.Log(HazScript);

        //hazard knockback test
        Assert.AreEqual(HazScript.getKB(), 0.2f);

        Assert.AreEqual(HazScript.getKBscale(), 10000);

        Assert.IsTrue(HazScript.getISpeed());

        Assert.IsTrue(HazScript.GetComponent<AudioSource>().enabled);

        //disable hazard script
        DisableHazards DScript = EjectionSeat.GetComponent<DisableHazards>();

        Assert.AreEqual(2, DScript.getDisabledObjects().ToArray().Length);

        EjectionSeat.GetComponent<DisableHazards>().DisableHazard();

        Assert.IsFalse(EjectionSeat.GetComponentInChildren<AudioSource>().enabled);
    }

    [Test]
    public void MovingWallTest()
    {
        MoveRoute MRScript = MovingWall.GetComponentInChildren<MoveRoute>();
        Debug.Log(MRScript);


        //moveroute test
        Assert.AreEqual(MRScript.getSpeed(), 2);

        Assert.AreEqual(MRScript.getRotationSpeed(), 1);


        Assert.AreEqual(MRScript.getErrorMarginToPoint(), .5);

        Assert.IsFalse(MRScript.getRouteDelete());

        Assert.IsFalse(MRScript.getRotateRoute());

        Assert.NotNull(MRScript.getSelf());

        //disable hazard script
        DisableHazards DScript = MovingWall.GetComponent<DisableHazards>();

        Assert.AreEqual(1, DScript.getDisabledObjects().ToArray().Length);


    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.


}
