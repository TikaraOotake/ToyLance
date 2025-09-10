using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public float ScatterAngle;       // 飛び散る角度の基準（度）
    public float RandScatterAngle;   // 角度のブレ幅（±度）

    public float BaseSpeed;          // 速度の基準
    public float RandSpeed;          // 速度のブレ幅（±）
    public float GravityScale;       // 重力加速度スケール

    public float DeleteTime = 1.0f;//削除時間
    private float DeleteTimer;

    private Vector2 SpeedVec;         // 現在の速度ベクトル
    private void Start()
    {
        // 角度をランダムに決定（ブレ幅付き）
        float angle = ScatterAngle + Random.Range(-RandScatterAngle * 0.5f, RandScatterAngle * 0.5f);
        float angleRad = angle * Mathf.Deg2Rad;

        // 速度をランダムに決定（ブレ幅付き）
        float speed = BaseSpeed + Random.Range(-RandSpeed * 0.5f, RandSpeed * 0.5f);

        // 初期速度ベクトルを計算
        SpeedVec = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;

        //タイマー設定
        DeleteTimer = DeleteTime;
    }

    private void Update()
    {
        // 重力加速度を適用（y方向に加速）
        SpeedVec += Vector2.down * GravityScale * Time.deltaTime;

        // 現在の速度で位置を更新
        //transform.position += (Vector3)(SpeedVec * Time.deltaTime); オブジェクトの向きを考慮した飛び方をしたいため変更
        transform.Translate(SpeedVec * Time.deltaTime, Space.World);

        //タイマー更新
        DeleteTimer = Mathf.Max(0.0f, DeleteTimer - Time.deltaTime);

        //時間経過で自身を削除
        if (DeleteTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
