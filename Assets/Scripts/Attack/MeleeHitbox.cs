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
            // �ڼ���: �ε�ȁE��E��� 'BreakableObject' ��ũ��Ʈ�� ������E�ִ���EȮ��
            BreakableObject breakableBlock = col.GetComponent<BreakableObject>();
            if (breakableBlock != null)
            {
                // ������ Break() �Լ��� ȣ���Ͽ� �ı��ϰ�E ������ ��� �̾�̨
                breakableBlock.Break();
                return; // ������E�ʰ�E��� �ϰ�
            }

            // �ε�ȁE��E��� ��E'Enemy' �Ǵ� 'Enemy_c01')�̶�E
            if (col.CompareTag("Enemy") || col.CompareTag("Enemy_c01"))
            {
                col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
                col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position);
                playerMoveScript.EndDownAttackAndBounce(BounceType.Large);
                hitLock = true;
            }
            // �ε�ȁE��E��� �ı� ������ 'â'�̶�E
            else if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null)
            {
                Destroy(col.gameObject);
            }
            // �ε�ȁE��E��� ��¥ '�ٴ�'�̶�E
            else if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                playerMoveScript.EndDownAttackAndBounce(BounceType.Small);
                hitLock = true;
            }
            return;
        }

        // --- ���� �Ϲ� ���� ����E(����E����) ---
        if (col.CompareTag("Enemy_c01")) { col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position); hitLock = true; return; }
        if (col.CompareTag("Enemy")) { col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position); hitLock = true; return; }
        if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null) { Destroy(col.gameObject); hitLock = true; return; }
    }
}
