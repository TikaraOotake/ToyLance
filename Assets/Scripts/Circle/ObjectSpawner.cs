using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    [Header("��ƁE����")]
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f; // ��ũ������ ���� 1�ʷ� �����Ǿ�E�ֽ��ϴ�.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("�ߺ��� ObjectSpawner�� �߰ߵǾ�E���� ����E���� �ı��մϴ�.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (objectToSpawn == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("ObjectSpawner ��ũ��Ʈ�� �ʿ��� ����E�Ʈ�� ��ƁE��ġ�� ��������E�ʾҽ��ϴ�.");
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

    // �ڡڡ� �� �κ��� ������ �Լ��Դϴ� �ڡڡ�
    void SpawnObject()
    {
        // 'spawnPoints' ��Ͽ� �ִ� ��E������ ��ȸ�մϴ�.
        foreach (Transform point in spawnPoints)
        {
            // �� ����(point)�� ��ġ�� ����E�Ʈ�� �����մϴ�.
            if (point != null)
            {
                Instantiate(objectToSpawn, point.position, point.rotation);
            }
        }
    }
}
