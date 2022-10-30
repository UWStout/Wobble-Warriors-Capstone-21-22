using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

public class CharacterMovementTest
{

    private GameObject playerPrefab;
    private GameObject aiPrefab;

    [OneTimeSetUp]
    public void LoadAssets() //loading a prefab
    {
        string[] searchResults = AssetDatabase.FindAssets("Player_Character_1");
        string prefabPath = AssetDatabase.GUIDToAssetPath(searchResults[0]);
        Debug.Log(prefabPath);
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        searchResults = AssetDatabase.FindAssets("EnemyCharacter_1");
        prefabPath = AssetDatabase.GUIDToAssetPath(searchResults[0]);
        Debug.Log(prefabPath);
        aiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    }


    // A Test behaves as an ordinary method
    [Test]
    public void CharacterMovementTestSimplePasses()
    {
        //add necessary scripts

        Assert.IsNotNull(playerPrefab);
        Assert.IsNotNull(playerPrefab.GetComponent<ActiveRagdoll>());
        CharacterMovement movement = playerPrefab.GetComponent<ActiveRagdoll>().getRoot().GetComponent<CharacterMovement>();

        //make sure the character movement script has a reference to the rigidbody
        movement.getRB();

        //test default values for logic errors
        Assert.That(movement.knockedOut == false);
        Assert.That(movement.MaxHealth > 0);
        Assert.That(movement.moveVector == Vector3.zero);

        if (aiPrefab.GetComponent<ActiveRagdoll>().getRoot().GetComponent<AICharacter>())
        {
            Assert.IsNotNull(aiPrefab.GetComponent<ActiveRagdoll>().getRoot().GetComponent<AICharacter>().healthbar);
        }

    }

    [Test]
    public void MovementTest()
    {
        CharacterMovement movement = playerPrefab.GetComponent<ActiveRagdoll>().getRoot().GetComponent<CharacterMovement>();
        //test movement
        movement.SetMoveDirection(Vector3.forward);
        Assert.That(movement.moveVector != Vector3.zero);

        //test rotation
        Quaternion initialRotation = Quaternion.identity;
        movement.TargetRotation = initialRotation;
        Assert.That(movement.TargetRotation == Quaternion.identity);

        movement.LookAt(Vector3.right);

        Assert.That(movement.TargetRotation != Quaternion.identity);
    }
}
