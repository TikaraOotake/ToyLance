using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject SpawnObjectPrefab; // 生成するオブジェクト
    [SerializeField] private float SpawnCycleTime = 1.0f; // スポーン間隔
    [SerializeField] private float AddRandamCycleTimeScale = 0.0f; // スポーン間隔にランダムな補正値を加える量
    private float SpawnCycleTimer;

    [SerializeField] private Vector2 RandamSpawnPosOffset;//ランダムでスポーン地点をずらす量

    [SerializeField]
    private Vector2 SpawnVel;//生成したオブジェクトに加える(リジットボディがある場合)

    [SerializeField] private bool IsSpawning = true; // 生成開始フラグ
    private bool IsSpawning_old;

    enum SpawnMode
    {
        FullAuto, // 一定周期で生成し続ける
        SemiAuto, // 一回だけ生成
    }
    [SerializeField] SpawnMode mode;

    [SerializeField] private SwitchButton[] SwitchList; // スイッチ

    [Header("制限設定")]
    [SerializeField] private int MaxObjects = 10;   // 最大数 (0以下なら無制限)
    [SerializeField] private float LifeTime = 10.0f;  // 寿命 (秒, 0以下なら無制限)

    // 生成したオブジェクトと生成時間を記録
    private List<(GameObject obj, float spawnTime)> spawnedObjects = new List<(GameObject, float)>();


    void Start()
    {
        SpawnCycleTimer = SpawnCycleTime + Random.Range(0.0f, AddRandamCycleTimeScale);
    }

    void Update()
    {
        // スイッチ状態を確認
        
        if (SwitchList != null && SwitchList.Length > 0)
        {
            IsSpawning = true;
            for (int i = 0; i < SwitchList.Length; ++i)
            {
                if (SwitchList[i] != null && !SwitchList[i].GetSwitchFlag())
                {
                    IsSpawning = false;
                }
            }
        }

        // モードごとの生成処理
        if (mode == SpawnMode.FullAuto)
        {
            if (IsSpawning) Update_Timer(ref SpawnCycleTimer);

            if (SpawnCycleTimer <= 0.0f)
            {
                SpawnObject();
                SpawnCycleTimer = SpawnCycleTime + Random.Range(0.0f, AddRandamCycleTimeScale);
            }
        }
        else if (mode == SpawnMode.SemiAuto)
        {
            if (IsSpawning && IsSpawning != IsSpawning_old)
            {
                SpawnObject();
            }
        }

        // 外部で破壊されたオブジェクトや寿命切れを削除
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            var entry = spawnedObjects[i];

            // すでに破壊されている
            if (entry.obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            // 寿命チェック
            if (LifeTime > 0 && Time.time - entry.spawnTime >= LifeTime)
            {
                Destroy(entry.obj);
                spawnedObjects.RemoveAt(i);
            }
        }
        
        //前フレームのフラグの状態を記録
        IsSpawning_old = IsSpawning;
    }

    private void SpawnObject()
    {
        if (SpawnObjectPrefab == null) return;

        //出現座標を設定
        Vector2 SpawnPos = transform.position;
        SpawnPos.x += Random.Range(-RandamSpawnPosOffset.x, RandamSpawnPosOffset.x);
        SpawnPos.y += Random.Range(-RandamSpawnPosOffset.y, RandamSpawnPosOffset.y);

        //オブジェクト生成
        GameObject newObj = Instantiate(SpawnObjectPrefab, SpawnPos, Quaternion.identity);

        //リジットボディがある場合は速度を与える
        Rigidbody2D _rb = newObj.GetComponent<Rigidbody2D>();
        if (_rb)
        {
            _rb.velocity = SpawnVel;//速度を代入
        }

        //リストに追加
        spawnedObjects.Add((newObj, Time.time));

        // 最大数制限による削除
        if (MaxObjects > 0 && spawnedObjects.Count > MaxObjects)
        {
            var oldest = spawnedObjects[0];
            if (oldest.obj != null) Destroy(oldest.obj);
            spawnedObjects.RemoveAt(0);
        }
    }

    private void Update_Timer(ref float _Timer)
    {
        _Timer = Mathf.Max(0.0f, _Timer - Time.deltaTime);
    }
}
