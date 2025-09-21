using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHP = 4;
    [SerializeField] protected float knockback = 10f;
    [SerializeField] private float iTime = 0.1f;   // ｹｫﾀ・ｽﾃｰ｣
    [SerializeField]
    protected int currentHP;
    protected int currentHP_old;
    protected Rigidbody2D rb;
    protected bool invincible;

    [SerializeField]
    private float DeadTime = 1.0f;//死亡時間
    private float DeadTimer;//死亡タイマー

    [SerializeField]
    private bool IsRespawn = true;//復帰可能かのフラグ
    private Vector2 RestartPos;

    private int Layer;

    /* ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡
   ｡ﾚﾃﾟｰ｡ : ﾇﾇｰﾝ ｻ・ｺｯｰ豼・SpriteRenderer
    ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ */
    private SpriteRenderer sr;                                   // ｡ﾚﾃﾟｰ｡

    void Awake()
    {
        currentHP = maxHP;
        currentHP_old = currentHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();                     // ｡ﾚﾃﾟｰ｡

        RestartPos = transform.position;//初期値をリスポーン座標として記録
    }

    /// <summary> ﾇﾃｷｹﾀﾌｾ錞｡ｰﾔ ｸﾂｾﾒﾀｻ ｶｧ ﾈ｣ﾃ・</summary>
    public virtual void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (currentHP <= 0) return;
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP = Mathf.Max(0, currentHP - dmg);

        StartCoroutine(HitFlash());            // ｸﾂﾀｻ ｶｧ ｻ｡ｰ｣ｻ・ﾀｯﾁ・
        /* ｦ｡ ｳﾋｹ・ｿｩｺﾎｸｦ ﾅｴ/ｱﾙﾁ｢ｿ｡ ｵ郞・ｼｱﾅﾃ ｦ｡ */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // ｼ・ｳﾋｹ・

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        //ダメージ音
        SEManager.instance.PlaySE("damage", transform.position);
    }

    public virtual void TakeDamage(int dmg, Vector2 attackerPos, Collider2D _coll, bool doKnockback = true)
    {
        TakeDamage(dmg, attackerPos, doKnockback);
    }

    public float GetDeadTimer()
    {
        return DeadTimer;
    }

    /* ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡
   ｡ﾚﾃﾟｰ｡ : ｽｺﾇﾁｶﾌﾆｮｸｦ ﾀ盂・ｻ｡ｰ｣ｻ貅ｹｱﾍ
    ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ｦ｡ */
    protected IEnumerator HitFlash()                                        // ｡ﾚﾃﾟｰ｡
    {
        Color original = sr.color;        // ｿ｡ ｻ・ﾀ惕・
        sr.color = Color.red;             // ｻ｡ｰ｣ｻｸｷﾎ ｺｯｰ・
        yield return new WaitForSeconds(0.1f);
        sr.color = original;              // ｿ｡ ｻ・ｺｹｱﾍ
    }

    protected IEnumerator IFrame()
    {
        invincible = true;
        yield return new WaitForSeconds(iTime);
        invincible = false;
    }

    protected void Die()
    {
        //死亡音
        SEManager.instance.PlaySE("dead", transform.position);

        if (IsRespawn == false)//復活しない設定のため完全に削除する
        {
            Destroy(this.gameObject);
            return;
        }

        
            
        //死亡状態としてマネージャーに登録
        Enemy_Manager.Instance.SetDeadEnemy(this.gameObject);

        // TODO: ｻ邵ﾁ ｾﾖｴﾏ｡､ﾆﾄﾆｼﾅｬ｡､ｽｺﾄﾚｾ・
        //Destroy(gameObject);      // ﾇﾊｿ萇ﾏｸ・ｾﾖｴﾏ ﾈﾄ Destroy(gameObject,0.3f);
        this.gameObject.SetActive(false);//オブジェクトを非有効に
    }

    public void SetHP(int _HP)
    {
        //上限下限
        _HP = Mathf.Max(_HP, 0);
        _HP = Mathf.Min(_HP, maxHP);
        
        currentHP = _HP;
    }
    public int GetHP()
    {
        return currentHP;
    }
    public int GetHP_old()
    {
        return currentHP_old;
    }

    public bool CheckRespawn()//復帰可能かチェック
    {
        bool result = false;

        //カメラ範囲の当たり判定取得
        Collider2D CameraCollider = Camera.main.GetComponent<Collider2D>();

        result = !(Collision_Manager.IsPointInsideCollider(CameraCollider, RestartPos));
        if (result && CameraCollider == true)//カメラ外の時かつカメラ範囲が有効である場合
        {
            Debug.Log("復帰を開始");

            //復帰処理
            transform.position = RestartPos;//座標を再設定
            currentHP = maxHP;//HPを回復
            this.gameObject.SetActive(true);//オブジェクトを有効に
            //レイヤー変更
            this.gameObject.layer = Layer;
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Health_Update();
    }

    protected void Health_Update()
    {
        //HPが0になった瞬間
        if (currentHP <= 0 && currentHP != currentHP_old)
        {
            //タイマー設定
            DeadTimer = DeadTime;

            //レイヤー変更
            Layer = this.gameObject.layer;
            this.gameObject.layer = LayerMask.NameToLayer("EnemySensor");
        }

        //HPが0以下のときタイマー終了で完全な死亡として処理
        if (currentHP <= 0 && DeadTimer <= 0.0f) Die();

        //タイマー更新
        DeadTimer = Mathf.Max(0.0f, DeadTimer - Time.deltaTime);

        //前の状態を記録
        currentHP_old = currentHP;
    }
}
