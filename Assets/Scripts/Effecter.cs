using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] private GameObject EffectPrefab;

    [SerializeField] GameObject SpawnPosObj;

    [SerializeField]
    private float SpawnInterval = 0.1f;//生成間隔
    private float SpawnTimer;

    void Start()
    {
        if (EffectPrefab == null) Debug.Log("Effectのプレハブがありません");
    }

    void Update()
    {
        //タイマー設定
        if (SpawnTimer <= 0.0f)
        {
            if (EffectPrefab != null)
            {
                Vector2 SpawnPos = transform.position;
                if (SpawnPosObj != null)
                {
                    SpawnPos = SpawnPosObj.transform.position;
                }
                Instantiate(EffectPrefab, SpawnPos, transform.rotation);
            }
            SpawnTimer = SpawnInterval;
        }

        //タイマー更新
        SpawnTimer = Mathf.Max(0.0f, SpawnTimer - Time.deltaTime);
    }

}
