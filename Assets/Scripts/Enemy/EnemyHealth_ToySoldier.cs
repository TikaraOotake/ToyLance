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

    public override void TakeDamage(int dmg, Vector2 attackerPos, Collider2D _coll, bool doKnockback = true)
    {

        if (Shield == null || _coll == null)//盾か攻撃の当たり判定がない
        {
            TakeDamage(dmg, attackerPos, doKnockback);
            Debug.Log("盾か攻撃の当たり判定がありません");
            return;
        }

        if (Shield.activeSelf == false)//盾が非アクティブ
        {
            TakeDamage(dmg, attackerPos, doKnockback);
            Debug.Log("盾が有効ではありません");
            return;
        }

        //盾と攻撃が重なっているか
        bool result = false;
        Collider2D Shield_Coll = Shield.GetComponent<Collider2D>();
        if (Shield_Coll != null && _coll != null)
        {
            result = Collision_Manager.AreCollidersTouchingAny(Shield_Coll, _coll);
            if (result == false) 
            {
                TakeDamage(dmg, attackerPos, doKnockback);
                Debug.Log("盾と攻撃判定は衝突していません");
                return;
            }
        }

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
            TakeDamage(dmg, attackerPos, doKnockback);
            return;
        }
    }

    private void Update()
    {
        Health_Update();
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
