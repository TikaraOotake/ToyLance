using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnLance : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;//回転速度
    [SerializeField] private GameObject SpriteBoard;//スプライトを表示する子オブジェクト

    [SerializeField] private GameObject Player;//槍を投げたPlayer

    [SerializeField] private float MoveSpeed;//移動速度
    [SerializeField] private float AccValue = 1.0f;//加速度量

    [SerializeField] private float StopTimer = 1.0f;//停止時間

    [SerializeField] private int AttackValue;
    void Start()
    {
        Player = GameManager_01.GetPlayer();//仮取得
    }

    // Update is called once per frame
    void Update()
    {
        if (StopTimer <= 0.0f)//停止時間中でないとき
        {
            //速度に加速度を加える
            MoveSpeed += AccValue * Time.deltaTime;

            if (Player != null)
            {
                //移動
                Vector2 TargetVec = Player.transform.position - transform.position;//移動方向計算
                float Length = TargetVec.magnitude;
                TargetVec.Normalize();//正規化
                transform.Translate(TargetVec * MoveSpeed * Time.deltaTime, Space.World);

                //距離を確認
                if (Length < MoveSpeed * Time.deltaTime)
                {
                    Destroy(this.gameObject);//移動速度が距離を超えたら削除
                }
            }


            //スプライトボードを回転
            if (SpriteBoard != null)
            {
                SpriteBoard.transform.eulerAngles += new Vector3(0.0f, 0.0f, RotateSpeed) * Time.deltaTime;
            }
        }
        else
        {
            if (Player != null && SpriteBoard != null)
            {
                //移動
                Vector2 TargetVec = Player.transform.position - transform.position;//移動方向計算
                TargetVec.Normalize();//正規化

                float angleDeg = Mathf.Atan2(TargetVec.y, TargetVec.x) * Mathf.Rad2Deg;    // 度数法に変換

                //SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 0.0f, angleDeg + 90.0f);


                // 現在のZ角度を取得
                float currentAngle = transform.eulerAngles.z;

                // deltaTimeを使ってフレームレートに依存しない回転速度に
                float step = RotateSpeed * 0.2f * Time.deltaTime;

                // 最短経路で目標角度へ近づく（360度の巻き戻りも考慮）
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, angleDeg, step);

                // 回転を適用（Euler角で直接設定）
                transform.eulerAngles = new Vector3(0f, 0f, newAngle);
            }
        }

            //タイマー更新
            StopTimer = Mathf.Max(0.0f, StopTimer - Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BreakableObject _breakable = collision.GetComponent<BreakableObject>();
        if (_breakable != null)
        {
            _breakable.Break();
        }

        EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
        if (_enemyHealth != null)
        {
            _enemyHealth.TakeDamage(AttackValue, transform.position, true);
        }
    }
}
