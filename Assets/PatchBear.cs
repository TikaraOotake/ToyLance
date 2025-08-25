using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchBear : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

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

    [SerializeField]
    private float MoveWay = 0.0f;

    [SerializeField]
    private Rigidbody2D _rb;//物理コンポ

    enum ActionStatus
    {
        Idol,//待機
        Walk,歩行
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
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
    }
    private void Walk()
    {
        if (_rb)
        {
            _rb.velocity = new Vector2(MoveWay * MoveValue, _rb.velocity.y);
        }
    }
    private void SetAction()
    {
        float Rand = Random.Range(0.0f, 1.0f);
        if (Rand <= 0.33f)//右移動
        {
            MoveWay = 1.0f;
        }
        else if (Rand <= 0.66f)//左移動
        {
            MoveWay = -1.0f;
        }
        else//待機
        {
            MoveWay = 0.0f;
        }

        //時間セット
        ActionTimer = Random.Range(3.0f, 6.0f);
    }
}
