using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    // シングルトンのインスタンス
    private static HitStopManager _instance;

    // インスタンスの取得
    public static HitStopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内にHitStopManagerオブジェクトがない場合、新しく作成する
                _instance = FindObjectOfType<HitStopManager>();

                if (_instance == null)
                {
                    GameObject hitStopObject = new GameObject("HitStopManager");
                    _instance = hitStopObject.AddComponent<HitStopManager>();
                }
            }
            return _instance;
        }
    }

    // ヒットストップの状態
    public static bool IsHitStopActive { get; private set; }
    private static float hitStopTime;
    private float timeLeft;

    void Update()
    {
        if (IsHitStopActive)
        {
            timeLeft -= Time.unscaledDeltaTime;  // ゲームの時間に関係なくカウントダウン
            if (timeLeft <= 0)
            {
                //Debug.Log("時間を元に戻す");
                IsHitStopActive = false;
                Time.timeScale = 1f;  // 時間を元に戻す
            }
        }
    }

    // ヒットストップを開始する静的メソッド
    public static void StartHitStop(float duration)
    {
        //Debug.Log("ヒットストップを開始");

        if (IsHitStopActive) return;  // すでにヒットストップ中は開始しない

        IsHitStopActive = true;
        hitStopTime = duration;
        Instance.timeLeft = duration;
        Time.timeScale = 0.1f;  // 時間のスケールを下げてスローに
    }
}
