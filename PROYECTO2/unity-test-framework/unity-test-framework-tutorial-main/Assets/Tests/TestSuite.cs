using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestSuite
{
    private GameObject skeletonObj;
    private Skeleton skeleton;
    private GameObject playerObj;
    private Player player;

    //private Game game;

    [SetUp]
    public void Setup()
    {
        skeletonObj = Object.Instantiate(Resources.Load<GameObject>("ArmedSkeleton"));
        skeleton = skeletonObj.GetComponent<Skeleton>();

        GameObject playerObj = new GameObject("FakePlayer");
        playerObj.transform.position = skeletonObj.transform.position + Vector3.forward * 5f;
        skeleton.player = playerObj.AddComponent<Player>(); 
        skeleton.player.enabled = false;

        if (skeleton.controller == null)
            skeleton.controller = skeletonObj.AddComponent<CharacterController>();
    }

    [UnityTest]
    public IEnumerator SkeletonMovesTowardsPlayer()
    {
        Vector3 startPos = skeletonObj.transform.position;

        yield return new WaitForSeconds(0.3f);

        Assert.AreNotEqual(startPos, skeletonObj.transform.position, 
            "El esqueleto no se movió hacia el jugador.");
    }
    //----------------el inicia correctamente--------------
    [UnityTest]
    public IEnumerator GameStartsCorrectly()
    {
        SceneManager.LoadScene("Main"); 
        yield return new WaitForSeconds(0.5f); 
        Assert.AreEqual("Main", SceneManager.GetActiveScene().name, "La escena principal no se cargo correctamente");

        Assert.Greater(Object.FindObjectsOfType<GameObject>().Length, 0, "La escena esta vacia, el juego no inicio correctamente");
    }

    //----------------el esqueleto recibe daño--------------

     [UnityTest]
    public IEnumerator SkeletonReceivesDamage()
    {
        float initialHealth = skeleton.health;
        skeleton.ApplyDamage(20f);
        yield return null;
        Assert.Less(skeleton.health, initialHealth, "El esqueleto no recibio daño correctamente.");
    }
    
    //----------------El esqueleto muere al llegar a 0 de vida--------------
    [UnityTest]
    public IEnumerator SkeletonDiesInZero()
    {
        skeleton.ApplyDamage(999f);
        yield return null;
        Assert.IsTrue(skeleton.health <= 0f, "El esqueleto no murio al quedarse sin salud.");
    }
    

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(skeletonObj);
    }

  
}
