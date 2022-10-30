using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.VFX;

public class LevelTests
{
    private GameObject doorPrefab;
    private GameObject directorPrefab;
    private GameObject roomPrefab;
    [OneTimeSetUp]
    public void LoadAssets()
    {
        string[] results;
        string prefabPath;
        results = AssetDatabase.FindAssets("door_room_001");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        Debug.Log(prefabPath);
        doorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);


        results = AssetDatabase.FindAssets("Director");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        Debug.Log(prefabPath);
        directorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        results = AssetDatabase.FindAssets("EmptyRoom");
        prefabPath = AssetDatabase.GUIDToAssetPath(results[0]);
        Debug.Log(prefabPath);
        roomPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    }
    // A Test behaves as an ordinary method
    [Test]
    public void SimpleNullTests()
    {
        // Use the Assert class to test conditions
        Assert.IsNotNull(doorPrefab);
        Assert.IsNotNull(doorPrefab.GetComponentInChildren<ModularDoor>());
        Assert.IsNotNull(directorPrefab);
        Assert.IsNotNull(directorPrefab.GetComponent<Director>());
        Assert.IsNotNull(roomPrefab);
        Assert.IsNotNull(roomPrefab.GetComponent<RoomInfo>());
    }

    [Test]
    public void DoorTests()
    {
        Assert.IsNotNull(doorPrefab);
        Assert.IsNotNull(doorPrefab.GetComponentInChildren<ModularDoor>());
        ModularDoor md = doorPrefab.GetComponentInChildren<ModularDoor>();
        Assert.IsNull(md.getCamera());
        Assert.AreEqual(md.getCameraOffset(), new Vector3(-2.76999998f, 10.8999996f, -11.1199999f));
        Assert.IsNull(md.getDirector());
        Assert.IsNotNull(md.getIndicators());
        Assert.AreNotEqual(md.getIndicators(), new List<VisualEffect>());
        Assert.IsNotNull(md.getSoundList());
        Assert.AreNotEqual(md.getSoundList(), new List<AudioClip>());
        Assert.IsNull(md.getMyDoor());
        Assert.IsNull(md.getTargetDoor());
        Assert.IsNull(md.getTargetRoom());
        Assert.AreEqual(md.getPlayerCount(), 0);
        Assert.AreEqual(md.getPlayersOnDoor(), 0);
        Assert.IsFalse(md.isHorizontal());
        Assert.IsFalse(md.isLockedDoor());
    }
    [Test]
    public void DirectorTests()
    {
        Assert.IsNotNull(directorPrefab);
        Assert.IsNotNull(directorPrefab.GetComponent<Director>());
        
        
        Director dir = directorPrefab.GetComponent<Director>();
        //Checking that debug stuff is off
        Assert.IsFalse(dir.isImmortal());
        Assert.IsFalse(dir.GetDoExtraLives());
        //
        dir.setImmortal(true);
        Assert.AreEqual(dir.GobCount, 0);
        //testing that player death tracking is working
        Assert.AreEqual(dir.PlayerDeathCount, 0);
        dir.PlayerDied();
        Assert.AreEqual(dir.PlayerDeathCount, 1);
        dir.PlayerUnDied();
        Assert.AreEqual(dir.PlayerDeathCount, 0);
        //testing that the undied function clamps properly
        dir.PlayerDied();
        dir.PlayerDied();
        dir.PlayerDied();
        dir.PlayerDied();
        Assert.AreEqual(dir.PlayerDeathCount, 4);
        dir.PlayerUnDied();
        dir.PlayerUnDied();
        dir.PlayerUnDied();
        dir.PlayerUnDied();
        dir.PlayerUnDied();
        dir.PlayerUnDied();
        Assert.AreEqual(dir.PlayerDeathCount, 0);
        //testing that enemy ko is working
        dir.GobCount = 2;
        dir.GobDied();
        Assert.AreEqual(dir.GobCount, 1);
        dir.GobCount = 0;
        //Checking that stuff is null/default prior to runtime
        Assert.IsNull(dir.GetCurrentRoom());
        Assert.IsNull(dir.GetCurrentRoomInfo());
        Assert.IsTrue(dir.CanTeleport);
    }
    [Test]
    public void RoomTests()
    {
        Assert.IsNotNull(roomPrefab);
        Assert.IsNotNull(roomPrefab.GetComponent<RoomInfo>());
        RoomInfo room = roomPrefab.GetComponent<RoomInfo>();
        Assert.IsNotNull(room.GetGobStopper());
        Assert.AreEqual(room.getMaxGobs(), 0);
        Assert.IsTrue(room.getRoomCleared());
        Assert.IsTrue(room.getSpawnRadius() > 0);
        //Checking that the hezard thing returns false when there are no hazards,
        //regardless of if you turn it on or off
        Assert.IsFalse(room.setRoomHazards(true));
        Assert.IsFalse(room.setRoomHazards(false));
    }
}
