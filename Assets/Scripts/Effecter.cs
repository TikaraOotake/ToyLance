using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] GameObject EffectPrefab;

    [SerializeField]
    private float SpownInterval = 0.1f;//生成間隔
    private float SpownTimer;

    void Start()
    {
        if (EffectPrefab == null) Debug.Log("Effectのプレハブがありません");
    }

    void Update()
    {


        //タイマー設定
        if (SpownTimer <= 0.0f)
        {
            if (EffectPrefab != null)
            {
                Instantiate(EffectPrefab, transform.position, transform.rotation);
            }
            SpownTimer = SpownInterval;
        }

        //タイマー更新
        SpownTimer = Mathf.Max(0.0f, SpownTimer - Time.deltaTime);
    }
}
