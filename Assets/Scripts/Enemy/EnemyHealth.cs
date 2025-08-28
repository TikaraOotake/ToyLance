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
    }

    /// <summary> ﾇﾃｷｹﾀﾌｾ錞｡ｰﾔ ｸﾂｾﾒﾀｻ ｶｧ ﾈ｣ﾃ・</summary>
    public virtual void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP -= dmg;
        StartCoroutine(HitFlash());            // ｸﾂﾀｻ ｶｧ ｻ｡ｰ｣ｻ・ﾀｯﾁ・
        /* ｦ｡ ｳﾋｹ・ｿｩｺﾎｸｦ ﾅｴ/ｱﾙﾁ｢ｿ｡ ｵ郞・ｼｱﾅﾃ ｦ｡ */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // ｼ・ｳﾋｹ・

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        if (currentHP <= 0) Die();

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
        // TODO: ｻ邵ﾁ ｾﾖｴﾏ｡､ﾆﾄﾆｼﾅｬ｡､ｽｺﾄﾚｾ・
        Destroy(gameObject);      // ﾇﾊｿ萇ﾏｸ・ｾﾖｴﾏ ﾈﾄ Destroy(gameObject,0.3f);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHP_old = currentHP;
    }
}
