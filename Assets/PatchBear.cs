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
    private int AngerLevel;//怒りレベル
    [SerializeField]
    private bool ChaseFlag;//追跡フラグ
    [SerializeField]
    private float SearchLength;//索敵範囲

    private Vector2 BaseScale;

    [SerializeField]
    private float ActionTimer;//行動時間

    private bool IsChase = false;

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
    }
    void Start()
    {
        //取得
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        SearchPlayer();

        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);

        SetAnim();
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
            MoveWay = 0.0f;
            actionStatus = ActionStatus.Idol;
        }
        else
        {
            //移動
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

            actionStatus = ActionStatus.Walk;
        }

        //時間セット
        ActionTimer = Random.Range(1.0f, 3.0f);
    }
    private void SetAnim()
    {
    }
}
