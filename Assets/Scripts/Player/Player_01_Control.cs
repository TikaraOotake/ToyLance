using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;
using UnityEngine.SceneManagement;
using PlayerStatusEnum;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private GameObject LancePrefab;//投げ槍のプレハブ
    [SerializeField]
    private List<GameObject> LanceList = new List<GameObject>();//投げた槍をリストで記録
    [SerializeField]
    private GameObject AttackPrefab;//近距離：落下攻撃　用のコライダープレハブ
    [SerializeField]
    private GameObject AttackObj;//

    [SerializeField] private WarpEntrance Door;//ドアを格納

    [SerializeField] private GameObject FlipObj;//Playerの向きに合わせて反転する子オブジェクト

    [SerializeField]
    private bool HaveLance = true;
    [SerializeField]
    private float MoveValue = 1.0f;//移動量
    [SerializeField]
    private float JumpValue = 1.0f;//ジャンプ量

    [SerializeField] private float HP;//体力
    [SerializeField] private float HP_Max = 5.0f;//最大体力

    [SerializeField]
    private float LanceSpeed;//槍速度

    [SerializeField]
    private Rigidbody2D _rb;//物理コンポーネント
    [SerializeField]
    private Animator _anim;//アニメーターコンポーネント
    [SerializeField]
    private SpriteRenderer _sr;//SpriteRendererコンポーネント

    private bool FlipX;//プレイヤーの向きを記録

    [SerializeField] private bool IsPause;//ポーズフラグ

    //アニメーション変数
    [SerializeField] private bool IsJump = false;
    [SerializeField] private bool IsMove = false;
    [SerializeField] private bool IsThrustAtk = false;
    [SerializeField] private bool IsThrowAtk = false;
    [SerializeField] private bool IsFallAtk = false;
    [SerializeField] private bool IsDoorEnter = false;

    [SerializeField] private bool IsLanding = false;//着地中か判定する
    [SerializeField] private bool IsLanding_old = false;

    [SerializeField] private Collider2D LandingCheckCollider;//着地チェックコライダー
    [SerializeField] private GameObject FallAttackPos;//落下攻撃コライダーの座標
    [SerializeField] private GameObject TrustAttackPos;//突き攻撃コライダーの座標

    [SerializeField]
    private float KnockBackValue = 1.0f;//ノックバック量
    [SerializeField]
    private float KonokBackTime = 0.5f;//ノックバック時間
    private float KnockBackTimer;//ノックバックタイマー
    private float KnockBackTimer_old;
    [SerializeField]
    private float InvincibleTime = 1.0f;//無敵時間
    private float InvincibleTimer = 0.0f;//無敵タイマー
    private float BlinkingTimer = 0.0f;//点滅タイマー

    [SerializeField]
    private float ThrowCooltime = 1.0f;//投槍のクールタイム
    private float ThrowCooltimer;

    private float AtkTimer;//攻撃タイマー
    private float TrustAtkTimer = 0.7f;//突き攻撃時間

    private float KeyboardInputTimer;
    private bool IsInputDown = false;//下入力情報を記録する
    private bool IsInputDown_old = false;

    private float AttackMoveValue_TrustBase = 10.0f;//攻撃中の基礎移動量
    private float AttackMoveValue;//攻撃中の移動量
    private float AttackMoveValueDecayRate = 30.0f;//秒間の減衰量

    [SerializeField] private int TrustAttackValue;//刺突攻撃力
    [SerializeField] private int ThrowAttackValue;//投擲攻撃力
    [SerializeField] private int FallAttackValue;//落下攻撃力

    [SerializeField] private float TestRot;

    PlayerStatus playerStatus;

    enum AtkStatus
    {
        None,//無し
        Thrust,//突き
        Throw,//投げ
        Falling,//落下
    }
    AtkStatus atkStatus = AtkStatus.None;
    private void Awake()
    {
        //マネージャーに自身を登録
        GameManager_01.SetPlayer(this.gameObject);

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("リジットボディの取得に失敗");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("アニメーターの取得に失敗");

        HP = HP_Max;
    }

    void Start()
    {
        playerStatus = PlayerStatus.Fine;//ステータスを通常に
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPause == true) return;//ポーズは何もせず終了
        if (Time.timeScale == 0.0f) return;//時間停止中は何もしない

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            KeyboardInputTimer = 1.0f;
        }

        if (playerStatus == PlayerStatus.Fine)
        {

            if (atkStatus == AtkStatus.None)
            {
                //基本動作
                Move();
                Jump();
                Throw();
                DoorEnter();
            }
            else
            {
                FallingAtk();
                TrustAtk();
            }

            InputAtkSetting();
        }
        else if (playerStatus == PlayerStatus.Dead)//死亡状態
        {
            if (_rb != null)
            {
                _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            }
            //GameManager_01.RespawnPlayer();//ゲームオーバー画面ができるまではここで処理する
        }

        //落下速度を制限
        if (_rb)
        {
            if (_rb.velocity.y < -JumpValue * 2.0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -JumpValue * 2.0f);
            }
        }

        //被弾判定
        GameObject Enemy = GetTouchingObjectWithLayer(GetComponent<Collider2D>(), "Enemy");
        if (Enemy != null)
        {
            SetDamage(Enemy);
        }

        SetAnim();

        CheckLanding_Update();

        GameManager_01.SetHP_UI((int)HP);

        Timer_Update();//タイマー更新

        //入力を検知する
        IsInputDown_old = IsInputDown;
        IsInputDown = Input.GetAxis("Vertical") < -0.1f;

        if (KnockBackTimer <= 0.0f && KnockBackTimer != KnockBackTimer_old)//タイマーが0になった瞬間だけ
        {
            //簡易実装
            playerStatus = PlayerStatus.Fine;//ステータスを通常に
            InvincibleTimer = InvincibleTime;//タイマーセット
            BlinkingTimer = InvincibleTimer;//点滅タイマーセット

            //死亡チェック
            if (HP <= 0.0f)
            {
                playerStatus = PlayerStatus.Dead;
                GameManager_01.CollGameOver();
            }
        }
    }

    private void Timer_Update()
    {
        //タイマー更新
        AtkTimer = Mathf.Max(0.0f, AtkTimer - Time.deltaTime);
        InvincibleTimer = Mathf.Max(0.0f, InvincibleTimer - Time.deltaTime);
        BlinkingTimer = Mathf.Max(0.0f, BlinkingTimer - Time.deltaTime);
        KnockBackTimer_old = KnockBackTimer;
        KnockBackTimer = Mathf.Max(0.0f, KnockBackTimer - Time.deltaTime);
        ThrowCooltimer = Mathf.Max(0.0f, ThrowCooltimer - Time.deltaTime);
        KeyboardInputTimer = Mathf.Max(0.0f, KeyboardInputTimer - Time.deltaTime);
    }
    private void InputAtkSetting()
    {
        if (!HaveLance) return;//槍を持っていない為終了
        if (AtkTimer > 0.0f) return;//攻撃中は処理をしない

        if (Input.GetKeyDown(KeyCode.S) || (IsInputDown != IsInputDown_old && IsInputDown))
        {
            if (GetTouchingObjectWithLayer(LandingCheckCollider, "Platform") ||
                GetTouchingObjectWithLayer(LandingCheckCollider, "SpearPlatform"))
                return;//着地中であれば除外

            //落下攻撃
            atkStatus = AtkStatus.Falling;
            AtkTimer = 1.0f;
            if (_rb) _rb.velocity = new Vector2(0.0f, JumpValue * 0.5f);
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("Melee")) 
        {
            //突き攻撃
            atkStatus = AtkStatus.Thrust;
            AtkTimer = TrustAtkTimer;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                KeyboardInputTimer = 1.0f;
            }

            if (Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.A) ||
                (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && KeyboardInputTimer <= 0.0f))
            {
                AttackMoveValue = AttackMoveValue_TrustBase;//移動量セット
            }
            else
            {
                AttackMoveValue = 0.0f;//移動無し
            }

            return;
        }
    }
    private void Move()
    {
        float MoveWay = 0.0f;
        if (Input.GetKey(KeyCode.D) || (Input.GetAxis("Horizontal") > 0.1f && KeyboardInputTimer <= 0.0f))
        {
            MoveWay = +1.0f;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetAxis("Horizontal") < -0.1f && KeyboardInputTimer <= 0.0f))
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
                FlipX = true;
            }
            else if (MoveWay < 0.0f)//左向き
            {
                FlipX = false;
            }
            _sr.flipX = FlipX;
        }
        if (FlipObj != null)
        {
            if (MoveWay > 0.0f)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else if (MoveWay < 0.0f)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
        }


        //移動量の代入
        if (_rb != null)
        {
            //移動量取得
            Vector2 MoveVelocity = _rb.velocity;
            MoveVelocity.x = MoveWay * MoveValue;
            _rb.velocity = MoveVelocity;//代入

            transform.Translate(new Vector2(MoveWay * MoveValue * 0.01f, 0.0f) * Time.deltaTime);//突っかかり防止のため少しめり込み
        }
    }
    private void Jump()
    {
        if (IsLanding)//接地中か見る
        {
            if (_rb != null)
            {
                if (_rb.velocity.y <= 0.0f)
                {
                    IsJump = false;//降下中かつ地上ならジャンプをオフ
                }
                if (_rb.velocity.y >= JumpValue * 0.1f)//上昇速度が一定以上ある場合ジャンプしない
                {
                    return;
                }
            }

            //入力
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) 
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
        if (!HaveLance) return;//槍を持っていない為終了

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Ranged")) 
        {
            if (ThrowCooltimer > 0.0f) return;//クールタイム中であるなら終了

            if (LancePrefab != null)
            {
                Vector3 Rot = new Vector3(0.0f, 0.0f, 0.0f);//向きを定義
                Vector2 ThrowVec = new Vector2(1.0f, 0.0f);//投げるベクトルを定義

                if (Input.GetKey(KeyCode.W))//上入力中であれば斜めに
                {
                    ThrowVec += new Vector2(0.0f, 1.0f);
                    ThrowVec.Normalize();//正規化
                    Rot.z += 45.0f;//角度を曲げる
                }

                ThrowVec *= LanceSpeed;//速度を適用

                if (!FlipX)
                {
                    Rot.y += 180.0f;
                    ThrowVec.x *= -1;//速度の向きを反転
                }
                GameObject Lance = Instantiate(LancePrefab, transform.position, Quaternion.identity);//槍を生成
                Lance.transform.eulerAngles = Rot;

                Rigidbody2D _rb = Lance.GetComponent<Rigidbody2D>();//物理コンポ取得
                if (_rb != null) _rb.velocity = ThrowVec;//速度代入

                ThrowCooltimer = ThrowCooltime;//クールタイマーセット

                return;
            }
        }
    }
    private void FallingAtk()
    {
        if (atkStatus == AtkStatus.Falling)
        {
            //終了処理
            if (AtkTimer <= 0.0f)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//攻撃判定の破棄

                IsFallAtk = false;
                return;
            }

            if (AtkTimer <= 0.8f)
            {
                if(!IsLanding)
                {
                    if (_rb) _rb.velocity = new Vector2(0.0f, -JumpValue * 2.0f);//空中時のみ急降下
                }
                
                InvincibleTimer = 0.1f;//無敵時間セット(一瞬)

                //攻撃判定の生成
                if (AttackObj == null && AttackPrefab != null)
                {
                    AttackObj = Instantiate(AttackPrefab);

                    //攻撃タイプの設定
                    SpearAttack spearAttack = AttackObj.GetComponent<SpearAttack>();
                    if (spearAttack != null)
                    {
                        spearAttack.SetAttackType(AttackType.Fall);
                        spearAttack.SetAttackValue(FallAttackValue);
                    }
                }
                //攻撃判定の設定更新
                if (AttackObj != null && FallAttackPos != null)
                {
                    AttackObj.transform.localScale = FallAttackPos.transform.lossyScale;//スケール
                    AttackObj.transform.position = FallAttackPos.transform.position;//座標
                    AttackObj.transform.eulerAngles = FallAttackPos.transform.eulerAngles;//角度
                }

                IsFallAtk = true;//落下攻撃Anim
            }

            //下を押し続けていたら時間延長
            if (Input.GetKey(KeyCode.S))
            {
                if (AtkTimer <= 0.0f)//0以下
                {
                    AtkTimer = Time.deltaTime * 2.0f;
                }
            }

            //着地判定が敵と接触したらキャンセル処理
            if (GetTouchingObjectWithLayer(LandingCheckCollider, "Enemy"))
            {
                if (_rb) _rb.velocity = new Vector2(0.0f, JumpValue * 1.5f);//通常ジャンプより少し高く跳ねる

                //マネージャーにカメラ振動依頼
                CameraManager.SetShakeCamera();

                AtkTimer = 0.0f;//タイマーを0に
            }

            if (IsLanding != IsLanding_old && IsLanding == true)//着地した瞬間だけ
            {
                CameraManager.SetShakeCamera();//カメラを揺らす
                if (_rb) _rb.velocity = new Vector2(0.0f, 0.0f);//移動しない
            }
        }
    }
    private void TrustAtk()
    {
        if (atkStatus == AtkStatus.Thrust)
        {
            //移動方向の取得
            float Way = -1.0f;
            if (_sr)
            {
                if (_sr.flipX) Way *= -1.0f;
            }

            if (_rb) _rb.velocity = new Vector2(AttackMoveValue * Way, _rb.velocity.y);//移動

            //移動量を減衰させる
            AttackMoveValue = Mathf.Max(0.0f, AttackMoveValue - AttackMoveValueDecayRate * Time.deltaTime);

            if (AtkTimer <= TrustAtkTimer)
            {
                IsThrustAtk = true;//刺突攻撃Anim
            }

            if (AtkTimer <= 0.9f)
            {
                //攻撃判定の生成
                if (AttackObj == null && AttackPrefab != null)
                {
                    AttackObj = Instantiate(AttackPrefab);

                    //攻撃タイプの設定
                    SpearAttack spearAttack = AttackObj.GetComponent<SpearAttack>();
                    if (spearAttack != null)
                    {
                        spearAttack.SetAttackType(AttackType.Trust);
                        spearAttack.SetAttackValue(TrustAttackValue);
                    }
                }
                //攻撃判定の設定更新
                if (AttackObj != null && TrustAttackPos != null)
                {
                    AttackObj.transform.localScale = TrustAttackPos.transform.lossyScale;//スケール
                    AttackObj.transform.position = TrustAttackPos.transform.position;//座標
                    AttackObj.transform.eulerAngles = TrustAttackPos.transform.eulerAngles;//角度
                }
            }

            //終了処理
            if (AtkTimer <= 0.0f)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//攻撃判定の破棄

                IsThrustAtk = false;
            }
        }
    }
    private void DoorEnter()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Vertical") > 0.1f)
        {
            if (Door != null)
            {
                bool result = false;//成功したかを記録
                result = Door.TeleportSetting(this.gameObject);
                if (result == true)
                {
                    IsDoorEnter = true;//ドア進入状態
                }
            }
        }
    }
    public void DoorExit()
    {
        IsDoorEnter = false;//ドアから出た
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

    private void CheckLanding_Update()
    {
        IsLanding_old = IsLanding;//変更前の状態を記録
        IsLanding = true;//初期化

        //足場があるかチェックし、あれば関数をそのまま終了
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "Platform") != null) return;
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "SpearPlatform") != null) return;
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "PlayerPlatform") != null) return;

        //空中(非着地)として記録
        IsLanding = false;
    }
    private void SetAnim()
    {
        if (_anim == null) return;//Animが設定されていなかったら終了

        //全て一旦リセット
        _anim.SetBool("isWalk", false);
        _anim.SetBool("isJumping", false);
        _anim.SetBool("isTrustAttack", false);
        _anim.SetBool("isDownAttacking", false);
        _anim.SetBool("doDamaged", false);
        _anim.SetBool("IsDoorEnter", false);
        _anim.SetBool("IsDead", false);

        //優先な物ほど上にならべる
        if (playerStatus == PlayerStatus.Dead)
        {
            _anim.SetBool("IsDead", true);
        }
        else if (IsDoorEnter)
        {
            _anim.SetBool("IsDoorEnter", true);
        }
        else if (KnockBackTimer > 0.0f)//被弾状態
        {
            _anim.SetBool("doDamaged", true);
        }
        else if (IsThrowAtk)//投げ
        {

        }
        else if (IsThrustAtk)//突き
        {
            _anim.SetBool("isTrustAttack", true);
        }
        else if (IsFallAtk)//落下攻撃
        {
            _anim.SetBool("isDownAttacking", true);
        }
        else if (IsJump)//ジャンプ
        {
            _anim.SetBool("isJumping", true);
        }
        else if (IsMove)//歩く
        {
            _anim.SetBool("isWalk", true);
        }

        //色の変更
        if (_sr != null)
        {
            //被弾中は赤色にさせる
            if (KnockBackTimer > 0.0f)
            {
                _sr.color = Color.red;//赤色にセット
            }
            else
            {
                _sr.color = Color.white;//白色にセット
            }


            //点滅時間中は点滅させる
            Color tempColor = _sr.color;
            if (BlinkingTimer > 0.0f)
            {
                if ((int)(BlinkingTimer * 10.0f) % 2 == 0)
                {
                    tempColor.a = 0.5f;
                }
                else
                {
                    tempColor.a = 1.0f;
                }
            }
            else
            {
                tempColor.a = 1.0f;
            }
            _sr.color = tempColor;
        }
    }

    public void RespawnPlayer(Vector2 _pos)
    {
        //リスポーン
        transform.position = _pos;//座標設定
        HP = HP_Max;//体力初期化
        playerStatus = PlayerStatus.Fine;
        if (AttackObj != null) Destroy(AttackObj);//攻撃判定の破棄

        if (_rb)
        {
            _rb.velocity = Vector2.zero;//速度初期化
        }
        if(_sr)
        {
            _sr.flipY = false;
        }
    }

    private void SetDamage(GameObject _Enemy)
    {
        if (InvincibleTimer > 0.0f)
        {
            return;//無敵時間中は処理を行わず終了
        }

        if (playerStatus == PlayerStatus.Fine)
        {
            HP -= 1.0f;//体力を減らす
            UIManager.Instance.SetHP_UI((int)HP);

            //どちらの方向からぶつかったか調べる
            float Way = _Enemy.transform.position.x - transform.position.x;

            //方向にあわせて吹っ飛ばす
            if (_rb)
            {
                Vector2 KnockBackVelocity = Vector2.zero;
                if (Way > 0)//左に
                {
                    Debug.Log("右からぶつかりました");
                    KnockBackVelocity += new Vector2(-KnockBackValue, 0.0f);
                }
                else if (Way < 0)//右に
                {
                    Debug.Log("左からぶつかりました");
                    KnockBackVelocity += new Vector2(KnockBackValue, 0.0f);
                }
                KnockBackVelocity += new Vector2(0.0f, JumpValue);//上方向の力を代入

                _rb.velocity = KnockBackVelocity;
            }

            //タイマーセット
            KnockBackTimer = KonokBackTime;//ノックバック時間
            InvincibleTimer = KnockBackTimer;//無敵時間

            playerStatus = PlayerStatus.HitDamage;//ステータスセット
            atkStatus = AtkStatus.None;//攻撃リセット
            if (AttackObj != null) Destroy(AttackObj);//攻撃判定の破棄

            //アニメーションフラグリセット
            IsFallAtk = false;
            IsThrowAtk = false;
            IsThrustAtk = false;

            //マネージャーにカメラ振動依頼
            CameraManager.SetShakeCamera();
        }
    }

    public void SetLance(bool _flag)
    {
        HaveLance = _flag;
    }
    public void SetIsPause(bool _flag)
    {
        IsPause = _flag;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spear")) return;

        if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }

        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance != null)
        {
            Door = warpEntrance;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance == Door)
        {
            Door = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance != null)
        {
            Door = warpEntrance;
        }
        if (collision.gameObject.tag == "Default")
        {
            Debug.Log(collision.gameObject.name);
        }

            if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance == Door)
        {
            Door = null;
        }
    }
}
