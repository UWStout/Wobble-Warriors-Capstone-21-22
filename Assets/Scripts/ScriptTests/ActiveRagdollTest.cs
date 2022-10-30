using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

public class ActiveRagdollTest
{
    private GameObject playerPrefab;

    [OneTimeSetUp]
    public void LoadAssets() //loading a prefab
    {
        string[] searchResults = AssetDatabase.FindAssets("Player_Character_1");
        string prefabPath = AssetDatabase.GUIDToAssetPath(searchResults[0]);
        Debug.Log(prefabPath);

        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void ActiveRagdollTestSimplePasses()
    {
        Assert.IsNotNull(playerPrefab);
        ActiveRagdoll ragdoll = playerPrefab.GetComponent<ActiveRagdoll>();
        Assert.IsNotNull(ragdoll);
        //if missing, would not work at all
        Assert.IsNotNull(ragdoll.getRoot());
        Assert.IsNotNull(ragdoll.getAnimatedRoot());
        Assert.IsNotNull(ragdoll.getCharacter());

        //if missing, would feel odd
        Assert.NotZero(ragdoll.getSpringForce());
    }

    [Test]
    public void RigConstructTest()
    {
        ActiveRagdoll ragdoll = playerPrefab.GetComponent<ActiveRagdoll>();
        //Find and construct the rig
        ragdoll.ConstructSkeleton();
        //ensure it works
        Assert.NotZero(ragdoll.getRigSize());
    }

    [Test]
    public void SettingSpringForce()
    {
        ActiveRagdoll ragdoll = playerPrefab.GetComponent<ActiveRagdoll>();
        ragdoll.SetSpringForce(1, 1);
        Assert.AreNotEqual(ragdoll.GetJoint(0).angularXDrive.positionSpring, 0);
        ragdoll.SetSpringForce(0, 0);
        Assert.AreEqual(ragdoll.GetJoint(0).angularXDrive.positionSpring, 0);

    }

}
