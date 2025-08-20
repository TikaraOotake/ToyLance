using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private float MoveValue = 1.0f;//移動量
    [SerializeField]
    private float JumpValue = 1.0f;//ジャンプ量

    [SerializeField]
    private Rigidbody2D _rb;//物理コンポーネント
    [SerializeField]
    private Animator _anim;//アニメーターコンポーネント
    [SerializeField]
    private SpriteRenderer _sr;//SpriteRendererコンポーネント


    [SerializeField]
    private Collider2D LandingCheckCollider;//着地チェックコライダー
   

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("リジットボディの取得に失敗");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

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

        if (MoveWay != 0.0f)
        {
            if (_anim != null) _anim.SetBool("isWalk", true);
        }
        else
        {
            if (_anim != null) _anim.SetBool("isWalk", false);
        }

        //Spriteの反転
        if(_sr)
        {
            if(MoveWay>0.0f)//右向き
            {
                _sr.flipX = false;
            }
            else if(MoveWay<0.0f)//左向き
            {
                _sr.flipX = true;
            }
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //移動量取得
                Vector2 MoveVelocity = _rb.velocity;
                MoveVelocity.y = JumpValue;
                _rb.velocity = MoveVelocity;//代入
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
}
