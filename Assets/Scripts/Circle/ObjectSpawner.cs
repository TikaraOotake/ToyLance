using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    [Header("½ºÆE¼³Á¤")]
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f; // ½ºÅ©¸°¼¦À» º¸´Ï 1ÃÊ·Î ¼³Á¤µÇ¾EÀÖ½À´Ï´Ù.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Áßº¹µÈ ObjectSpawner°¡ ¹ß°ßµÇ¾E»õ·Î »ı±E°ÍÀ» ÆÄ±«ÇÕ´Ï´Ù.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (objectToSpawn == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("ObjectSpawner ½ºÅ©¸³Æ®¿¡ ÇÊ¿äÇÑ ¿ÀºE§Æ®³ª ½ºÆEÀ§Ä¡°¡ ÁöÁ¤µÇÁE¾Ê¾Ò½À´Ï´Ù.");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObject();
        }
    }

    // ¡Ú¡Ú¡Ú ÀÌ ºÎºĞÀÌ ¼öÁ¤µÈ ÇÔ¼öÀÔ´Ï´Ù ¡Ú¡Ú¡Ú
    void SpawnObject()
    {
        // 'spawnPoints' ¸ñ·Ï¿¡ ÀÖ´Â ¸ğµEÁöÁ¡À» ¼øÈ¸ÇÕ´Ï´Ù.
        foreach (Transform point in spawnPoints)
        {
            // °¢ ÁöÁ¡(point)ÀÇ À§Ä¡¿¡ ¿ÀºE§Æ®¸¦ »ı¼ºÇÕ´Ï´Ù.
            if (point != null)
            {
                Instantiate(objectToSpawn, point.position, point.rotation);
            }
        }
    }
}
