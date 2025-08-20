using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private GameObject LancePrefab;//投げ槍のプレハブ

    [SerializeField]
    private float MoveValue = 1.0f;//移動量
    [SerializeField]
    private float JumpValue = 1.0f;//ジャンプ量

    [SerializeField]
    private float LanceSpeed;//槍速度

    [SerializeField]
    private Rigidbody2D _rb;//物理コンポーネント
    [SerializeField]
    private Animator _anim;//アニメーターコンポーネント
    [SerializeField]
    private SpriteRenderer _sr;//SpriteRendererコンポーネント

    private bool FlipX;//プレイヤーの向きを記録

    [SerializeField] private bool IsJump = false;
    [SerializeField] private bool IsMove = false;
    [SerializeField] private bool IsThrustAtk = false;
    [SerializeField] private bool IsThrowAtk = false;

    [SerializeField]
    private Collider2D LandingCheckCollider;//着地チェックコライダー

    [SerializeField]
    private float KnockBackValue = 1.0f;//ノックバック量
    [SerializeField]
    private float KonokBackTime = 0.5f;//ノックバック時間
    private float KnockBackTimer;//ノックバックタイマー
   
    enum PlayerStatus
    {
        Fine,
        HitDamage,
        Dead,
    }
    PlayerStatus playerStatus;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("リジットボディの取得に失敗");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");

        KnockBackTimer = KonokBackTime;//タイマー初期化
    }

    void Start()
    {
        playerStatus = PlayerStatus.Fine;//ステータスを通常に
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus == PlayerStatus.Fine)
        {
            Move();
            Jump();
            Throw();
        }
        else
        {
            //...
        }



        SetAnim();

        //タイマー更新
        KnockBackTimer = Mathf.Max(0.0f, KnockBackTimer - Time.deltaTime);
        if (KnockBackTimer <= 0.0f) playerStatus = PlayerStatus.Fine;//簡易実装
    }

    private void Move()
    {
        float MoveWay = 0.0f;
        if (Input.GetKey(KeyCode.D))
        {
            MoveWay = +1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveWay = -1.0f;
        }

        //アニメーションの変更
        if (MoveWay != 0.0f)
        {
            IsMove = true;
        }
        else
        {
            IsMove = false;
        }

        //Spriteの反転
        if (_sr)
        {
            if (MoveWay > 0.0f)//右向き
            {
                FlipX = false;
            }
            else if (MoveWay < 0.0f)//左向き
            {
                FlipX = true;
            }
            _sr.flipX = FlipX;
        }

        //移動量の代入
        if (_rb != null)
        {
            //移動量取得
            Vector2 MoveVelocity = _rb.velocity;
            MoveVelocity.x = MoveWay * MoveValue;
            _rb.velocity = MoveVelocity;//代入
        }
    }
    private void Jump()
    {
        if (GetTouchingObjectWithLayer(LandingCheckCollider,"Platform"))
        {
            if (_rb != null)
            {
                if (_rb.velocity.y <= 0.0f)
                {
                    IsJump = false;//降下中かつ地上ならジャンプをオフ
                }
            }

            //入力
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //移動量取得
                Vector2 MoveVelocity = _rb.velocity;
                MoveVelocity.y = JumpValue;
                _rb.velocity = MoveVelocity;//代入

                IsJump = true;//アニメーションをジャンプに
            }
        }
    }

    public void Throw()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (LancePrefab != null)
            {
                Quaternion Rot = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                if (!FlipX)
                {
                    Rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                GameObject Lance = Instantiate(LancePrefab, transform.position, Rot);

                if (Lance.TryGetComponent(out SpearProjectile stick))
                {
                    stick.Init(1, LanceSpeed);
                }
            }
        }
    }

    /// <summary>
    /// 指定したコライダーに接触している、指定レイヤー名のオブジェクトを取得します。
    /// </summary>
    /// <param name="collider">対象のCollider2D</param>
    /// <param name="layerName">探すレイヤー名</param>
    /// <returns>該当レイヤーのGameObject（なければnull）</returns>
    public static GameObject GetTouchingObjectWithLayer(Collider2D collider, string layerName)
    {
        int targetLayer = LayerMask.NameToLayer(layerName);
        if (targetLayer == -1)
        {
            Debug.LogWarning($"指定されたレイヤー名「{layerName}」は存在しません。");
            return null;
        }

        // 一時的な配列（最大数を適当に確保）
        Collider2D[] results = new Collider2D[10];
        int count = collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);

        for (int i = 0; i < count; i++)
        {
            if (results[i] != null && results[i].gameObject.layer == targetLayer)
            {
                return results[i].gameObject;
            }
        }

        return null;
    }

    private void SetAnim()
    {
        if (_anim == null) return;//Animが設定されていなかったら終了

        //全て一旦リセット
        _anim.SetBool("isWalk", false);
        _anim.SetBool("isJumping", false);
        _anim.SetBool("DoMeleeAttack", false);

        //優先な物ほど上にならべる
        if (IsThrowAtk)//投げ
        {

        }
        else if(IsThrustAtk)//突き
        {
            _anim.SetBool("DoMeleeAttack", true);
        }
        else if(IsJump)//ジャンプ
        {
            _anim.SetBool("isJumping", true);
        }
        else if(IsMove)//歩く
        {
            _anim.SetBool("isWalk", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //どちらの方向からぶつかったか調べる
            float Way = collision.transform.position.x - transform.position.x;

            //方向にあわせて吹っ飛ばす
            if (_rb)
            {
                if (Way > 0)//左に
                {
                    _rb.velocity += new Vector2(-KnockBackValue, 0.0f);
                }
                else if (Way < 0)//右に
                {
                    _rb.velocity += new Vector2(KnockBackValue, 0.0f);
                }
            }

            KnockBackTimer = KonokBackTime;//タイマーセット

            playerStatus = PlayerStatus.HitDamage;//ステータスセット

            //マネージャーにカメラ振動依頼
            CameraManager.SetShakeCamera();
        }
    }
}
