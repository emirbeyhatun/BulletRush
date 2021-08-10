using System.Collections;
using System.Collections.Generic;
using BulletRushGame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditorTests
{

    [Test]
    public void PlayerTakesDamageEndDies()
    {

        GameObject gameObj = new GameObject();
        Player pl = gameObj.AddComponent<Player>();
        PlayerStats stats = new PlayerStats();
        stats.hp = 100;
        pl.SetStats(stats);

        bool isDead = false;
        pl.TakeDamage(25, out isDead);

        Assert.IsTrue(stats.hp == 75);

        pl.TakeDamage(75, out isDead);

        Assert.IsTrue(isDead == true);
    }

    [Test]
    public void PlayerJoystickMoves4Directions()
    {
        GameObject playerObj = new GameObject();
        playerObj.transform.position = Vector3.zero;

        Player pl = playerObj.AddComponent<Player>();
        Rigidbody rb = playerObj.AddComponent<Rigidbody>();
        rb.useGravity = false;

        GameObject cam = new GameObject();
        Camera camComp = cam.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 0, -10);
        cam.transform.forward = (playerObj.transform.position - cam.transform.position).normalized;

        PlayerInput pInput = new PlayerInput(rb, camComp);

        pl.PlayerInput = pInput;
        pl.PlayerInput.MovementInputs(10, new Vector2(0, 1));//Forward
        Assert.AreEqual(rb.velocity.z, 10);

        rb.velocity = Vector3.zero;
        pl.PlayerInput.MovementInputs(10, new Vector2(0, -1));//Backward
        Assert.AreEqual(rb.velocity.z, -10);

        rb.velocity = Vector3.zero;
        pl.PlayerInput.MovementInputs(10, new Vector2(1, 0));//Right

        Assert.AreEqual(rb.velocity.x, 10);

        rb.velocity = Vector3.zero;
        pl.PlayerInput.MovementInputs(10, new Vector2(-1, 0));//Left

        Assert.AreEqual(rb.velocity.x, -10);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EditorTestsWithEnumeratorPasses()
    {
       



        yield return null;

    }
}
