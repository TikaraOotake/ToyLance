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
            // ｡ﾚｼ､: ｺﾎｵ抦・ｴ・ﾌ 'BreakableObject' ｽｺﾅｩｸｳﾆｮｸｦ ｰ｡ﾁ・ﾀﾖｴﾂﾁ・ﾈｮﾀﾎ
            BreakableObject breakableBlock = col.GetComponent<BreakableObject>();
            if (breakableBlock != null)
            {
                // ｺ昞ﾏﾀﾇ Break() ﾇﾔｼｦ ﾈ｣ﾃ簓ﾏｿｩ ﾆﾄｱｫﾇﾏｰ・ ｰﾝﾀｺ ｰ霈ﾓ ﾀﾌｾ銧ｨ
                breakableBlock.Break();
                return; // ｸﾘﾃﾟﾁ・ｾﾊｰ・ｰ霈ﾓ ﾇﾏｰｭ
            }

            // ｺﾎｵ抦・ｴ・ﾌ ﾀ・'Enemy' ｶﾇｴﾂ 'Enemy_c01')ﾀﾌｶ・
            if (col.CompareTag("Enemy") || col.CompareTag("Enemy_c01"))
            {
                col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
                col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position);
                playerMoveScript.EndDownAttackAndBounce(BounceType.Large);
                hitLock = true;
            }
            // ｺﾎｵ抦・ｴ・ﾌ ﾆﾄｱｫ ｰ｡ｴﾉﾇﾑ 'ﾃ｢'ﾀﾌｶ・
            else if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null)
            {
                Destroy(col.gameObject);
            }
            // ｺﾎｵ抦・ｴ・ﾌ ﾁ･ 'ｹﾙｴﾚ'ﾀﾌｶ・
            else if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                playerMoveScript.EndDownAttackAndBounce(BounceType.Small);
                hitLock = true;
            }
            return;
        }

        // --- ﾀﾌﾇﾏ ﾀﾏｹﾝ ｰﾝ ｷﾎﾁ・(ｺｯｰ・ｾｽ) ---
        if (col.CompareTag("Enemy_c01")) { col.GetComponent<CliffMonsterHealth>()?.TakeMelee(damage, transform.position); hitLock = true; return; }
        if (col.CompareTag("Enemy")) { col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position); hitLock = true; return; }
        if (col.GetComponent<SpearProjectile>() != null || col.GetComponent<StickableSpear>() != null) { Destroy(col.gameObject); hitLock = true; return; }
    }
}
