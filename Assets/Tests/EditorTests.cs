using System.Collections;
using System.Collections.Generic;
using BulletRushGame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditorTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void EditorTestsSimplePasses()
    {
        PlayerStats stats = new PlayerStats();

        Assert.IsTrue(stats.hp == 100);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EditorTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
