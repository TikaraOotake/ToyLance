using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    private bool hitLock;
    private Player_move playerMoveScript;

    void Awake()
    {
        playerMoveScript = GetComponentInParent<Player_move>();
    }

    void OnEnable() => hitLock = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!CompareTag("PlayerMeleeAttack")) return;
        if (hitLock && !playerMoveScript.isDownAttacking) return;
        if (playerMoveScript == null) return;

        if (playerMoveScript.isDownAttacking)
        {
            // ★수정: 부딪힌 대상이 'BreakableObject' 스크립트를 가지고 있는지 확인
            BreakableObject breakableBlock = col.GetComponent<BreakableObject>();
            if (breakableBlock != null)
            {
                // 블록의 Break() 함수를 호출하여 파괴하고, 공격은 계속 이어감
                breakableBlock.Break();
                return; // 멈추지 않고 계속 하강
            }

            // 부딪힌 대상이 적('Enemy' 또는 'Enemy_c01')이라면
            if (col.CompareTag("Enemy") || col.CompareTag("Enemy_c01"))
            {
                col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
                col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position);
                playerMoveScript.EndDownAttackAndBounce(BounceType.Large);
                hitLock = true;
            }
            // 부딪힌 대상이 파괴 가능한 '창'이라면
            else if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null)
            {
                Destroy(col.gameObject);
            }
            // 부딪힌 대상이 진짜 '바닥'이라면
            else if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                playerMoveScript.EndDownAttackAndBounce(BounceType.Small);
                hitLock = true;
            }
            return;
        }

        // --- 이하 일반 공격 로직 (변경 없음) ---
        if (col.CompareTag("Enemy_c01")) { col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position); hitLock = true; return; }
        if (col.CompareTag("Enemy")) { col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position); hitLock = true; return; }
        if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null) { Destroy(col.gameObject); hitLock = true; return; }
    }
}
