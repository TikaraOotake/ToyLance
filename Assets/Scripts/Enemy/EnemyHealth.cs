using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHP = 4;
    [SerializeField] private float knockback = 10f;
    [SerializeField] private float iTime = 0.1f;   // 무적 시간
    private int currentHP;
    private Rigidbody2D rb;
    private bool invincible;

    /* ─────────────────────────────────────────
   ★추가 : 피격 색상 변경용 SpriteRenderer
    ───────────────────────────────────────── */
    private SpriteRenderer sr;                                   // ★추가

    void Awake()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();                     // ★추가
    }

    /// <summary> 플레이어에게 맞았을 때 호출 </summary>
    public void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP -= dmg;
        StartCoroutine(HitFlash());            // 맞을 때 빨간색 유지

        /* ─ 넉백 여부를 투척/근접에 따라 선택 ─ */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // 수평 넉백

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        if (currentHP <= 0) Die();

    }

    /* ─────────────────────────────────────────
   ★추가 : 스프라이트를 잠깐 빨간색→복귀
    ───────────────────────────────────────── */
    IEnumerator HitFlash()                                        // ★추가
    {
        Color original = sr.color;        // 원래 색상 저장
        sr.color = Color.red;             // 빨간색으로 변경
        yield return new WaitForSeconds(0.1f);
        sr.color = original;              // 원래 색상 복귀
    }

    IEnumerator IFrame()
    {
        invincible = true;
        yield return new WaitForSeconds(iTime);
        invincible = false;
    }

    void Die()
    {
        // TODO: 사망 애니·파티클·스코어
        Destroy(gameObject);      // 필요하면 애니 후 Destroy(gameObject,0.3f);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
