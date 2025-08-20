using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Enemy_c01 몸에 10초 박히고, 플랫폼에 닿으면 파괴
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class StickableSpear : MonoBehaviour
{
    [SerializeField] float flyLife = 3f;
    [SerializeField] float stuckLife = 10f;
    Rigidbody2D rb;
    BoxCollider2D col;
    SpriteRenderer sr;
    bool stuck;

    public void Init(int dir, float speed)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = dir < 0;

        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.right * dir * speed;
        col.isTrigger = true;

        Destroy(gameObject, flyLife);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (stuck) return;

        // ★추가: 날아가는 중에 다른 박힌 창과 부딪혔는지 확인
        int spearPlatformLayer = LayerMask.NameToLayer("SpearPlatform");
        if (other.gameObject.layer == spearPlatformLayer || other.CompareTag("StuckSpear"))
        {
            Destroy(other.gameObject); // 박혀있던 창 파괴
            Destroy(gameObject);       // 지금 막 날아온 내 창도 파괴
            return;                    // 이후 다른 로직은 실행하지 않음
        }

        int platformLayer = LayerMask.NameToLayer("Platform");
        var eh = other.GetComponent<EnemyHealth>();
        var cliffEh = other.GetComponent<CliffMonsterHealth>();

        if (eh != null)
        {
            eh.TakeDamage(1, transform.position, false);
            Destroy(gameObject);
            return;
        }

        if (cliffEh != null)
        {
            if (!cliffEh.isStickable)
            {
                Destroy(gameObject);
                return;
            }
            StickToTarget(other.transform);
            cliffEh.AttachSpear(transform);
            return;
        }

        if (other.gameObject.layer == platformLayer)
        {
            StickToTarget(other.transform);
            return;
        }
    }

    void StickToTarget(Transform target)
    {
        stuck = true;
        rb.velocity = Vector2.zero;
        col.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Static;
        transform.SetParent(target, true);

        // ★추가: 박혔을 때 태그를 "StuckSpear"로 변경 (근접 공격으로 파괴될 수 있도록)
        gameObject.tag = "StuckSpear";

        CancelInvoke();
        Destroy(gameObject, stuckLife);
    }
}
