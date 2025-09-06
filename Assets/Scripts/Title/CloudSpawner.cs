using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs; 

    public float spawnInterval = 3f; 

    public float upperCloudY = 3f;     
    public float upperCloudSpeed = 2f; 

    public float lowerCloudY = 1f;     
    public float lowerCloudSpeed = 1.5f; 
    public float lowerCloudScale = 0.7f; 

    private float spawnX = 12f; 

    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            SpawnSingleCloud(upperCloudY, upperCloudSpeed, 1f); 

            SpawnSingleCloud(lowerCloudY, lowerCloudSpeed, lowerCloudScale);
        }
    }

    void SpawnSingleCloud(float yPos, float speed, float scale)
    {
        int randomIndex = Random.Range(0, cloudPrefabs.Length);
        GameObject selectedPrefab = cloudPrefabs[randomIndex];

        Vector2 spawnPosition = new Vector2(spawnX, yPos);
        GameObject newCloud = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        newCloud.transform.localScale = new Vector3(scale, scale, 1f);

        CloudMover mover = newCloud.GetComponent<CloudMover>();
        if (mover != null)
        {
            mover.moveSpeed = speed;
        }
    }
}
