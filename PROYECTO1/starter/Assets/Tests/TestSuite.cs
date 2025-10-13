using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{
    // 1
    private Game game;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    // 2
    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        // 5
        float initialYPos = asteroid.transform.position.y;
        // 6
        yield return new WaitForSeconds(0.1f);
        // 7
        Assert.Less(asteroid.transform.position.y, initialYPos);
        // 8
        //Object.Destroy(game.gameObject);
    }

    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        //1
        asteroid.transform.position = game.GetShip().transform.position;
        //2
        yield return new WaitForSeconds(0.1f);

        //3
        Assert.True(game.isGameOver);

        //Object.Destroy(game.gameObject);
    }

    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        // 1
        GameObject laser = game.GetShip().SpawnLaser();
        // 2
        float initialYPos = laser.transform.position.y;
        yield return new WaitForSeconds(0.1f);
        // 3
        Assert.Greater(laser.transform.position.y, initialYPos);
    }

    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        // 1
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        // 2
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }

    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        // 1
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        // 2
        Assert.AreEqual(game.score, 1);
    }

    [Test]
    public void NewGameRestartsGame()
    {
        //2
        game.isGameOver = true;
        game.NewGame();
        //3
        Assert.False(game.isGameOver);
    }

    //----------------------------------------------------------
    [UnityTest]
    public IEnumerator ShipNotMoveWASD()
    {
        GameObject shipObject = game.GetShip().gameObject;
        Vector3 initialPosition = shipObject.transform.position;

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(initialPosition, shipObject.transform.position);
    }

    [UnityTest]
    public IEnumerator ShipMovement()
    {
        Ship ship = game.GetShip();
        ship.isDead = false; 

        Vector3 startPos = ship.transform.position;

        ship.MoveRight();
        yield return new WaitForSeconds(0.1f);

        Assert.AreNotEqual(startPos, ship.transform.position, "La nave no se movió al llamar MoveRight.");

        Vector3 afterMoveRight = ship.transform.position;

        ship.MoveLeft();
        yield return new WaitForSeconds(0.1f);

        Assert.AreNotEqual(afterMoveRight, ship.transform.position, "La nave no se movió al llamar MoveLeft.");
    }

    [UnityTest]
    public IEnumerator LimitsShip()
    {
        Ship ship = game.GetShip();
        ship.isDead = false;

        ship.transform.localPosition = new Vector3(100f, 0f, 0f);

        ship.MoveLeft(); 
        yield return null; 

        Assert.AreEqual(40f, ship.transform.localPosition.x, 0.001f,
            "No se aplico el limite (maxLeft = 40).");

        
        ship.transform.localPosition = new Vector3(-100f, 0f, 0f);

        ship.MoveRight(); 
        yield return null;

        Assert.AreEqual(-40f, ship.transform.localPosition.x, 0.001f,
            "No se aplico el limite (maxRight = -40).");
    }
    
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }

}