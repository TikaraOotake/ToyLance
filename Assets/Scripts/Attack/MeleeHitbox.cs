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
            // �ڼ���: �ε��� ����� 'BreakableObject' ��ũ��Ʈ�� ������ �ִ��� Ȯ��
            BreakableObject breakableBlock = col.GetComponent<BreakableObject>();
            if (breakableBlock != null)
            {
                // ����� Break() �Լ��� ȣ���Ͽ� �ı��ϰ�, ������ ��� �̾
                breakableBlock.Break();
                return; // ������ �ʰ� ��� �ϰ�
            }

            // �ε��� ����� ��('Enemy' �Ǵ� 'Enemy_c01')�̶��
            if (col.CompareTag("Enemy") || col.CompareTag("Enemy_c01"))
            {
                col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
                col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position);
                playerMoveScript.EndDownAttackAndBounce(BounceType.Large);
                hitLock = true;
            }
            // �ε��� ����� �ı� ������ 'â'�̶��
            else if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null)
            {
                Destroy(col.gameObject);
            }
            // �ε��� ����� ��¥ '�ٴ�'�̶��
            else if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                playerMoveScript.EndDownAttackAndBounce(BounceType.Small);
                hitLock = true;
            }
            return;
        }

        // --- ���� �Ϲ� ���� ���� (���� ����) ---
        if (col.CompareTag("Enemy_c01")) { col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position); hitLock = true; return; }
        if (col.CompareTag("Enemy")) { col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position); hitLock = true; return; }
        if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null) { Destroy(col.gameObject); hitLock = true; return; }
    }
}
