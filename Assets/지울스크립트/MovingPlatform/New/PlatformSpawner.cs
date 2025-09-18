using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("������ ���� ����")]
    [Tooltip("������ ������ ������(Prefab)�� �����ϼ���.")]
    public GameObject platformPrefab;

    [Header("�̵� ���� �� �ӵ� ����")]
    [Tooltip("������ ������ ��ġ���� �̵� ������(A)�Դϴ�.")]
    public Transform spawnPointA;

    [Tooltip("������ ������ �̵��� ��ǥ ����(B)�Դϴ�.")]
    public Transform targetPointB;

    [Tooltip("������ ������ �̵� �ӵ��Դϴ�.")]
    public float platformSpeed = 3.0f;

    [Tooltip("�÷��̾�� �ν��� ���̾ �������ּ���.")]
    public LayerMask playerLayer;

    [Header("���� �ֱ� ����")]
    [Tooltip("������ �����Ǵ� �ð� ����(��)�Դϴ�.")]
    public float spawnInterval = 5.0f;

    void Start()
    {
        // ������ �ֱ⿡ ���� ������ �����ϴ� �ڷ�ƾ ����
        StartCoroutine(SpawnPlatformRoutine());
    }

    IEnumerator SpawnPlatformRoutine()
    {
        // ���� �ݺ�
        while (true)
        {
            // 1. ������ �ð���ŭ ��ٸ�
            yield return new WaitForSeconds(spawnInterval);

            // 2. ���� �������� �ְ�, ����/��ǥ ������ ��� �����Ǿ��ٸ�
            if (platformPrefab != null && spawnPointA != null && targetPointB != null)
            {
                // 3. A ������ ���ο� ������ ����
                GameObject newPlatform = Instantiate(platformPrefab, spawnPointA.position, Quaternion.identity);

                // 4. ������ ������ DisposablePlatform ��ũ��Ʈ�� ������
                DisposablePlatform platformScript = newPlatform.GetComponent<DisposablePlatform>();
                if (platformScript != null)
                {
                    // 5. ������ ���ǿ��� ��ǥ ����(B)�� �ӵ�, �÷��̾� ���̾� ������ �˷���
                    platformScript.targetB = this.targetPointB;
                    platformScript.speed = this.platformSpeed;
                    platformScript.playerLayer = this.playerLayer;
                }
            }
        }
    }
}
