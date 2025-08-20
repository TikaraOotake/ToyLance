using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    [Header("스폰 설정")]
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f; // 스크린샷을 보니 1초로 설정되어 있습니다.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("중복된 ObjectSpawner가 발견되어 새로 생긴 것을 파괴합니다.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (objectToSpawn == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("ObjectSpawner 스크립트에 필요한 오브젝트나 스폰 위치가 지정되지 않았습니다.");
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

    // ★★★ 이 부분이 수정된 함수입니다 ★★★
    void SpawnObject()
    {
        // 'spawnPoints' 목록에 있는 모든 지점을 순회합니다.
        foreach (Transform point in spawnPoints)
        {
            // 각 지점(point)의 위치에 오브젝트를 생성합니다.
            if (point != null)
            {
                Instantiate(objectToSpawn, point.position, point.rotation);
            }
        }
    }
}
