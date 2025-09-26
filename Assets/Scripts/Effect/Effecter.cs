using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] private GameObject EffectPrefab;

    [SerializeField] GameObject SpawnPosObj;

    [SerializeField]
    private float SpawnInterval = 0.1f;//生成間隔 ※0は無効とする
    private float SpawnTimer;

    [SerializeField]
    private int SpawnRange_Max;
    [SerializeField]
    private int SpawnRange_Min;

    [SerializeField]
    private bool IsStartSpawnFlag;

    void Start()
    {
        if (EffectPrefab != null)
        {
            if (IsStartSpawnFlag == true)
            {
                GenerateEffect();
            }
        }
        else
        { 
            Debug.Log("Effectのプレハブがありません");
        }

    }

    void Update()
    {
        if (SpawnInterval == 0.0f) return;

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

    public void GenerateEffect()
    {
        if (EffectPrefab != null)
        {
            int Rand = Random.Range(SpawnRange_Min, SpawnRange_Max); 

            Vector2 SpawnPos = transform.position;
            if (SpawnPosObj != null)
            {
                SpawnPos = SpawnPosObj.transform.position;
            }
            for (int i = 0; i < Rand; ++i)
            {
                Instantiate(EffectPrefab, SpawnPos, transform.rotation);
            }
        }
    }
}
