using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] 
    private GameObject Shield;              //盾

    private Enemy_shield shieldScript;

    private bool shieldJustBroke = false;   //盾が壊れているかのフラグ

    private void Start()
    {
        shieldScript = GetComponentInChildren<Enemy_shield>();
    }

    public override void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (shieldJustBroke)
        {
            return;
        }

        GameObject Player = GameManager_01.GetPlayer();
        float PlayerPosX = 0.0f;
        float EnemyPosX = 0.0f;
        float ShieldPosX = 0.0f;

        if (Player != null && Shield != null && !shieldScript.isBroken) 
        {
            PlayerPosX = Player.transform.position.x;
            EnemyPosX = transform.position.x;
            ShieldPosX = Shield.transform.position.x;
        }

        float minX = Mathf.Min(PlayerPosX, EnemyPosX);
        float maxX = Mathf.Max(PlayerPosX, EnemyPosX);

        bool isShieldBetween = ShieldPosX > minX && ShieldPosX < maxX;

        //プレイヤーと敵の間に盾がなかったら
        if (!isShieldBetween)
        {
            //ダメージ処理
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
    }

    public void SetShieldJustBrokeFlag()
    {
        shieldJustBroke = true;
        StartCoroutine(ResetShieldBreakFlag());
    }

    private IEnumerator ResetShieldBreakFlag()
    {
        // 1フレームだけ待って解除
        yield return null;
        shieldJustBroke = false;
    }
}
