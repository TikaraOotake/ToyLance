using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatchBear : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject FlipObj;//移動に合わせて反転させるオブジェクト

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

    [SerializeField]
    private float ActionTimer;//行動時間

    [SerializeField]
    private float MoveWay = 0.0f;
    [SerializeField]
    private float SearchDistance;//Playerを探す距離

    [SerializeField] Collider2D CliffCheckColl;//崖端を確認するコライダー
    [SerializeField] Collider2D WallCheckColl;//壁を確認するコライダー

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
    }
    [SerializeField]
    private ActionStatus actionStatus;

    private void Awake()
    {
        BaseScale = transform.localScale;//BaseScale設定
        BaseSpeed = MoveValue;
    }
    void Start()
    {
        //取得
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

        Walk();
        SearchPlayer();

        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);

        SetAnim();

        if (_eh != null)
        {
            int hp = _eh.GetHP();
            
            if (hp != HP_old && hp < HP_old)//ダメージを受けた時
            {
                AngerLevel = Mathf.Min(3, AngerLevel + 1);//怒りレベルを上げる(最大値:3)
            }
            HP_old = hp;//体力を記録
        }
    }
    private void Walk()
    {
        if (CliffCheckColl != null) 
        {
            if (!Collision_Manager.GetTouchingObjectWithLayer(CliffCheckColl, "Platform"))
                MoveWay *= -1;//移動方向反転
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
            if (SearchLength <= Length)
            {
                //索敵範囲内に入れば追跡モードに
                IsChase = true;
            }

            if (IsChase)
            {
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
    private void SetAction()
    {
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
    }
}
