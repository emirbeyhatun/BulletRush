using System.Collections;
using System.Collections.Generic;
using BulletRushGame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayTests
{
   
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerPositionChangeWithJoystickInput()
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


        yield return new WaitForSeconds(1);

        Assert.Greater(playerObj.transform.position.z, 1);
    }

    [UnityTest]
    public IEnumerator PlayerTargetDetection()
    {
        GameObject playerObj = new GameObject();
        playerObj.transform.position = Vector3.zero;

        Player pl = playerObj.AddComponent<Player>();
        Rigidbody rb = playerObj.AddComponent<Rigidbody>();
        rb.useGravity = false;

        BoxCollider boxCol = playerObj.AddComponent<BoxCollider>();
        boxCol.size = Vector3.one * 5;
        boxCol.isTrigger = true;

        TargetFinder targetFinder = playerObj.AddComponent<TargetFinder>();

        pl.SetTargetFinder(targetFinder);


        GameObject enemyObj = new GameObject();
        enemyObj.transform.position = new Vector3(0, 0, -10);

        Enemy enemy = enemyObj.AddComponent<Enemy>();
        Rigidbody enemRb = enemyObj.AddComponent<Rigidbody>();
        enemRb.useGravity = false;

        BoxCollider enemBox = enemyObj.AddComponent<BoxCollider>();
        enemBox.size = Vector3.one * 2;
        enemBox.isTrigger = true;
        enemRb.velocity = new Vector3(0, 0, 2);

        yield return new WaitForSeconds(1);

        bool didTargetFinderCatchIt = false;

        for (float i = 5; i >= 0; i -= Time.deltaTime)
        {
            if (targetFinder.GetTarget())
            {
                didTargetFinderCatchIt = true;
            }
            yield return null;
        }

        Assert.AreEqual(didTargetFinderCatchIt, true);

    }
}
