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

    [UnityTest]
    public IEnumerator GameStartsCorrectly()
    {
        // Cargar la escena principal del juego (asegurate de usar el nombre exacto de la escena)
        SceneManager.LoadScene("Main"); 
        yield return new WaitForSeconds(0.5f); // Espera corta para inicialización

        // Verifica que la escena se haya cargado correctamente
        Assert.AreEqual("Main", SceneManager.GetActiveScene().name, "La escena principal no se cargó correctamente.");

        // Verifica que haya al menos un objeto activo en la escena
        Assert.Greater(Object.FindObjectsOfType<GameObject>().Length, 0, "La escena está vacía, el juego no inició correctamente.");
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(skeletonObj);
    }
}
