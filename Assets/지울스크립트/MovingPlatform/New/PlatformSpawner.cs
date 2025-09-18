using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("생성할 발판 설정")]
    [Tooltip("생성할 발판의 프리팹(Prefab)을 연결하세요.")]
    public GameObject platformPrefab;

    [Header("이동 지점 및 속도 설정")]
    [Tooltip("발판이 생성될 위치이자 이동 시작점(A)입니다.")]
    public Transform spawnPointA;

    [Tooltip("생성된 발판이 이동할 목표 지점(B)입니다.")]
    public Transform targetPointB;

    [Tooltip("생성될 발판의 이동 속도입니다.")]
    public float platformSpeed = 3.0f;

    [Tooltip("플레이어로 인식할 레이어를 선택해주세요.")]
    public LayerMask playerLayer;

    [Header("생성 주기 설정")]
    [Tooltip("발판이 생성되는 시간 간격(초)입니다.")]
    public float spawnInterval = 5.0f;

    void Start()
    {
        // 설정된 주기에 맞춰 발판을 생성하는 코루틴 시작
        StartCoroutine(SpawnPlatformRoutine());
    }

    IEnumerator SpawnPlatformRoutine()
    {
        // 무한 반복
        while (true)
        {
            // 1. 설정된 시간만큼 기다림
            yield return new WaitForSeconds(spawnInterval);

            // 2. 발판 프리팹이 있고, 시작/목표 지점이 모두 설정되었다면
            if (platformPrefab != null && spawnPointA != null && targetPointB != null)
            {
                // 3. A 지점에 새로운 발판을 생성
                GameObject newPlatform = Instantiate(platformPrefab, spawnPointA.position, Quaternion.identity);

                // 4. 생성된 발판의 DisposablePlatform 스크립트를 가져옴
                DisposablePlatform platformScript = newPlatform.GetComponent<DisposablePlatform>();
                if (platformScript != null)
                {
                    // 5. 생성된 발판에게 목표 지점(B)과 속도, 플레이어 레이어 정보를 알려줌
                    platformScript.targetB = this.targetPointB;
                    platformScript.speed = this.platformSpeed;
                    platformScript.playerLayer = this.playerLayer;
                }
            }
        }
    }
}
