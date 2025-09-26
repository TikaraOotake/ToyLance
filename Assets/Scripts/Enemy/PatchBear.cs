using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PatchBear : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject FlipObj;//移動に合わせて反転させるオブジェクト

    [SerializeField] private GameObject CottonPrefab;       //コットンのPrefab
    [SerializeField] private float CottonSpownRate = 1.0f;  //コットンの生成頻度
    private bool CottonSpownFlag = false;                   //コットン生成フラグ
    private Vector2 Position_old;                           //前にいた座標
    private float TotalMoveValue;                           //累計移動距離

    [SerializeField]
    private float MoveValue;//移動速度
    [SerializeField]
    private int AngerLevel = 0;//怒りレベル
    [SerializeField]
    private bool IsChase;//追跡フラグ
    [SerializeField]
    private float SearchLength;//索敵範囲

    private Vector2 BaseScale;
    private float BaseSpeed;

    [SerializeField] private float ActionTimer;//行動時間
    [SerializeField] private float MoveFlipCoolTime = 0.5f;//振り向きのクールタイム
    [SerializeField] private float MoveFlipCoolTimer;//振り向きのクールタイマー

    [SerializeField]
    private float MoveWay = 0.0f;
    [SerializeField]
    private float SearchDistance;//Playerを探す距離

    [SerializeField] Collider2D CliffCheckColl;//崖端を確認するコライダー
    [SerializeField] Collider2D WallCheckColl;//壁を確認するコライダー

    [SerializeField] private bool IsWalk;

    [SerializeField]
    private Animator _anim;//アニメーター
    [SerializeField]
    private Rigidbody2D _rb;//物理コンポ
    [SerializeField]
    private SpriteRenderer _sr;//スプライトレンダラー

    [SerializeField]
    private EnemyHealth _eh;//敵体力
    private int HP_old;

    public struct AngerStatus
    {
        public float MoveValue;
        public float Scale;
    }
    [SerializeField] private AngerStatus[] PhaseStatusList = new AngerStatus[3]; 

    enum ActionStatus
    {
        Idol,//待機
        Walk,//歩行
        Dead,//死亡
    }
    [SerializeField]
    private ActionStatus actionStatus;

    private void Awake()
    {
        BaseScale = transform.localScale;//BaseScale設定
        BaseSpeed = MoveValue;
    }

    private void OnEnable()
    {
        CottonSpownFlag = false;//初期化
    }
    void Start()
    {
        //取得
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _eh = GetComponent<EnemyHealth>();

        if (_eh) HP_old = _eh.GetHP();//体力を記録

        if (0 < PhaseStatusList.Length)
        {
            PhaseStatusList[0].MoveValue = 1.0f;
            PhaseStatusList[0].Scale = 1.0f;
        }
        if (1 < PhaseStatusList.Length)
        {
            PhaseStatusList[1].MoveValue = 3.0f;
            PhaseStatusList[1].Scale = 1.1f;
        }
        if (2 < PhaseStatusList.Length)
        {
            PhaseStatusList[2].MoveValue = 5.0f;
            PhaseStatusList[2].Scale = 1.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Statusから速度とサイズを適用
        if (AngerLevel >= 0 && AngerLevel < PhaseStatusList.Length)//要素チェック
        {
            float Scale = PhaseStatusList[AngerLevel].Scale;
            transform.localScale = BaseScale * Scale;
            MoveValue = PhaseStatusList[AngerLevel].MoveValue * BaseSpeed;
        }

        if(actionStatus == ActionStatus.Idol)
        {
            if (_rb)
            {
                _rb.velocity = new Vector2(0.0f, _rb.velocity.y);//静止する
            }
        }
        else if(actionStatus == ActionStatus.Walk)
        {
            Walk();
        }
        else if (actionStatus == ActionStatus.Dead)
        {
            if (_eh != null)
            {
                int hp = _eh.GetHP();//HPを取得
                if (hp > 0)
                {
                    AngerLevel = 0;
                    actionStatus = ActionStatus.Idol;//待機状態に戻す
                }
            }
        }

        SearchPlayer();
        SpownCotton();
        
        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        //タイマー更新
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
        MoveFlipCoolTimer = Mathf.Max(0.0f, MoveFlipCoolTimer - Time.deltaTime);

        SetAnim();

        if (_eh != null)
        {
            int hp = _eh.GetHP();//HPを取得
            
            if (hp != HP_old && hp < HP_old)//ダメージを受けた時
            {
                AngerLevel = Mathf.Min(3, AngerLevel + 1);//怒りレベルを上げる(最大値:3)
                IsChase = true;                           //追跡状態に
                CottonSpownFlag = true;                   //コットンの生成開始
            }

            if(hp<=0.0f)//体力が0以下の場合
            {
                actionStatus = ActionStatus.Dead;//死亡状態に
            }

            HP_old = hp;//体力を記録
        }
    }
    private void Walk()
    {
        if (CliffCheckColl != null && MoveFlipCoolTimer <= 0.0f)
        {
            if (!Collision_Manager.GetTouchingObjectWithLayer(CliffCheckColl, "Platform"))
            {
                MoveFlipCoolTimer = MoveFlipCoolTime;//タイマーセット
                MoveWay *= -1;//移動方向反転
            }
        }

        if (FlipObj != null)
        {
            if (MoveWay > 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
            else if (MoveWay < 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        if (_sr != null)
        {
            if (MoveWay > 0)
            {
                _sr.flipX = true;
            }
            else if (MoveWay < 0)
            {
                _sr.flipX = false;
            }
        }


        if (_rb)
        {
            _rb.velocity = new Vector2(MoveWay * MoveValue, _rb.velocity.y);
        }
    }
    private void SearchPlayer()
    {
        Player = GameManager_01.GetPlayer();
        if (Player != null)
        {
            float Length = Vector2.Distance(Player.transform.position, transform.position);
            if (SearchLength > Length)
            {
                //索敵範囲内に入れば追跡モードに
                IsChase = true;
            }

            if (IsChase && MoveFlipCoolTimer <= 0.0f)
            {
                //プレイヤーの方向に向きを合わせる
                float Direction = Player.transform.position.x - transform.position.x;
                if (Direction >= 1.0f)
                {
                    MoveWay = +1.0f;
                }
                else if (Direction <= -1.0f)
                {
                    MoveWay = -1.0f;
                }
            }
        }
    }
    private void SpownCotton()
    {
        if (CottonSpownFlag)
        {
            //移動距離を計算
            Vector2 Pos = transform.position;
            Vector2 LengthVec = Pos - Position_old;
            TotalMoveValue += LengthVec.magnitude;//距離を加算

            //座標を更新
            Position_old = transform.position;

            //一定距離を超えたら
            if (TotalMoveValue >= CottonSpownRate)
            {
                //コットンを生成
                if (CottonPrefab != null)
                {
                    GameObject Cotton = Instantiate(CottonPrefab, transform.position + new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity);
                    Rigidbody2D _rb = Cotton.GetComponent<Rigidbody2D>();
                    if (_rb != null)
                    {
                        _rb.velocity = new Vector2(_rb.velocity.x, 1.0f);//上に跳ねる
                    }
                }

                TotalMoveValue = 0.0f;//累計移動距離リセット
            }
        }
    }
    private void SetAction()
    {
        //一旦リセット
        IsWalk = false;

        if (0.5f >= Random.Range(0.0f, 1.0f))
        {
            //待機
            actionStatus = ActionStatus.Idol;
            MoveWay = 0.0f;
        }
        else
        {
            //移動
            actionStatus = ActionStatus.Walk;
            if (0.5f >= Random.Range(0.0f, 1.0f))
            {
                //右移動
                MoveWay = +1.0f;
            }
            else
            {
                //左移動
                MoveWay = -1.0f;
            }
        }

        //時間セット
        ActionTimer = Random.Range(1.0f, 3.0f);
    }
    private void SetAnim()
    {
        if(_anim)
        {
            int Enemy_HP = 0;
            if (_eh)
            {
                Enemy_HP = _eh.GetHP();//体力を記録
            } 

            //一旦falseに
            _anim.SetBool("IsIdle", false);
            _anim.SetBool("IsWalk", false);
            _anim.SetBool("IsDead", false);

            if (Enemy_HP <= 0)
            {
                _anim.SetBool("IsDead", true);
            }
            else if (actionStatus == ActionStatus.Walk)
            {
                _anim.SetBool("IsWalk", true);
            }
            else
            {
                _anim.SetBool("IsIdle", true);
            }

            //怒りレベルを更新
            _anim.SetInteger("AngerLevel", AngerLevel);
        }
    }
}
