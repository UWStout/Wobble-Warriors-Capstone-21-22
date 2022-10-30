using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BossAITests
{
    GameObject testBoss;
    TestBossAI bossScript;

    [OneTimeSetUp]
    public void Setup()
    {
        testBoss = new GameObject();
        testBoss.AddComponent<TestBossAI>();
        bossScript = testBoss.GetComponent<TestBossAI>();
    }

    [Test]
    public void MakeSureAllValuesAreDefault()
    {
        Assert.AreEqual(bossScript.playerInSight, false);
    }

    /*
    // A Test behaves as an ordinary method
    [Test]
    public void BossAITestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BossAITestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    */
}
