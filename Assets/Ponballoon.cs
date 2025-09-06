using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Ponballoon : MonoBehaviour
{
    [SerializeField] private GameObject HangLift;//吊り下げる足場
    [SerializeField] private float AnchorRange;//吊り下げ距離

    [SerializeField] private int BalloonNam;//風船個数
    [SerializeField] private int BalloonNam_Max = 1;//最大風船個数
    private int BalloonNam_old;


    [SerializeField] private float RevivalTime;//復活時間(0の場合は復活なし)
    [SerializeField] private float RevivalTimer;//復活タイマー
    private float RevivalTimer_old = 0.0f;

    [SerializeField] private float MoveSpeed = 1.0f;//移動速度

    [SerializeField] private float TargetHeight;//目標座標高度

    [SerializeField] private float[] SequenceHeight;//段階高度(原則0番目には0)

    [SerializeField] private float ChaseRate;//追跡率

    [SerializeField] float BasePos;//基本座標

    Rigidbody2D _rb;

    private void Awake()
    {
        TargetHeight = transform.position.y;//自身の座標にセット
        BasePos = transform.position.y;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();//物理コンポ取得

        if (_rb)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;//重力無効化
        }

        if (MoveSpeed <= 0) Debug.Log("速度が0以下です　正常な動作ができません");

        BalloonNam = BalloonNam_Max;

        if (HangLift != null)
        {
            // 親子関係を解除（worldPositionStays = true でワールド座標を保持）
            HangLift.transform.SetParent(null, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //段階高度に目標高度を移動
        int Index = BalloonNam - 1;
        if (Index >= 0 && Index < SequenceHeight.Length)//配列外チェック
        {
            if (!(MoveSpeed <= 0))//移動速度より大きい
            {
                if (TargetHeight != SequenceHeight[Index])//段階高度と目標高度が違う
                {
                    //移動速度以内であれば位置を合わせて終了
                    if (SequenceHeight[Index] - MoveSpeed > TargetHeight ||
                        SequenceHeight[Index] + MoveSpeed < TargetHeight)
                    {
                        TargetHeight = SequenceHeight[Index];
                    }

                    else if (SequenceHeight[Index] > TargetHeight)//段階高度より小さければ
                    {
                        TargetHeight += MoveSpeed * Time.deltaTime;//加算(上昇)
                    }
                    else
                    {
                        TargetHeight -= MoveSpeed * Time.deltaTime;//減算(下降)
                    }
                }
            }
        }

        if (BalloonNam <= 0)
        {
            //風船が無い場合
            if (_rb)
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;//重力有効化
            }
        }
        else
        {
            //目標高度に移動
            Vector2 pos = transform.position;
            pos.y -= BasePos;
            float rate = 1.0f - Mathf.Pow(1.0f - ChaseRate, Time.deltaTime * 60f); // 秒間ChaseRateになるように
            pos.y += (TargetHeight - pos.y) * rate;

            //Nanチェック
            if (!(pos.y >= 0.0f && pos.y <= 0.0f))
            {
                transform.position = pos + new Vector2(0.0f, BasePos);
            }

            if (_rb)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;//重力無効化
                _rb.velocity = Vector2.zero;//速度初期化
            }
        }



        //タイマーが0になった瞬間
        if (RevivalTimer <= 0.0f && RevivalTimer != RevivalTimer_old)
        {
            //風船を復活
            BalloonNam_old = BalloonNam;
            BalloonNam = Mathf.Min(BalloonNam_Max, BalloonNam + 1);

            //0個から復帰したら
            if (BalloonNam != BalloonNam_old && BalloonNam > 0)
            {
                //基準座標を下回っていたら目標高度を現在座標にセット
                if (BasePos > transform.position.y)
                {
                    TargetHeight = BasePos - transform.position.y;
                }
            }
        }

        //タイマーセット
        if (RevivalTimer == 0.0f && BalloonNam < BalloonNam_Max)//風船が最大数に満たしていないとき
        {
            RevivalTimer = RevivalTime;
        }

        //タイマー更新
        if (RevivalTime > 0)//0の場合はタイマー更新なし
        {
            RevivalTimer_old = RevivalTimer;
            RevivalTimer = Mathf.Max(0.0f, RevivalTimer - Time.deltaTime);
        }

        //吊り下げている足場を引き寄せる
        if (HangLift != null)
        {
            Vector2 LiftPos = HangLift.transform.position;
            Vector2 BalloonPos = transform.position;
            Vector2 Vec = BalloonPos - LiftPos;
            float Range = Vec.magnitude;//距離を算出

            if (Range > AnchorRange &&//吊り下げ距離よりも遠かったら
                Range != 0.0f)//0以外の時
            {
                //足場を近づける
                Vec.Normalize();//正規化
                HangLift.transform.position = BalloonPos - Vec * AnchorRange;

                //速度を初期化
                Rigidbody2D _rb = HangLift.GetComponent<Rigidbody2D>();
                if (_rb) _rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            //風船を一個減らす
            BalloonNam_old = BalloonNam;
            BalloonNam = Mathf.Max(0, BalloonNam - 1);

            //復活タイマー設定
            RevivalTimer = RevivalTime;
        }
    }
}
