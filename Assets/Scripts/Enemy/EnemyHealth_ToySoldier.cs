using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] 
    private GameObject Shield;              //盾

    private Enemy_shield shieldScript;

    private Animator _anim;                 //アニメーター

    private void Start()
    {
        shieldScript = GetComponentInChildren<Enemy_shield>();

        _anim = GetComponent<Animator>();
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

        if (shieldScript.isBroken)
        {
            return;
        }

        GameObject Player = GameManager_01.GetPlayer();

        float PlayerPosX = 0.0f;    //プレイヤーのx座標
        float EnemyPosX = 0.0f;     //敵のx座標
        float ShieldPosX = 0.0f;    //盾のx座標

        if (Player != null && Shield != null && !shieldScript.isBroken) 
        {
            //それぞれのx座標を取得
            PlayerPosX = Player.transform.position.x;
            EnemyPosX = transform.position.x;
            ShieldPosX = Shield.transform.position.x;
        }

        //プレイヤーのx座標と敵のx座標から
        float minX = Mathf.Min(PlayerPosX, EnemyPosX);      //最小値
        float maxX = Mathf.Max(PlayerPosX, EnemyPosX);      //最大値

        //盾のx座標がプレイヤーと敵の間にあるか
        bool isShieldBetween = ShieldPosX > minX && ShieldPosX < maxX;

        //盾のx座標がプレイヤーと敵の間にない場合
        if (!isShieldBetween)
        {
            //ダメージ処理
            TakeDamage(dmg, attackerPos, doKnockback);
        }
    }

    private void Update()
    {
        Health_Update();
        SetAnim();
    }

    public void SetShieldJustBrokeFlag()
    {
        shieldScript.isBroken = true;
        if(gameObject.activeSelf)
        {
            StartCoroutine(ResetShieldBreakFlag());
        }
    }

    private IEnumerator ResetShieldBreakFlag()
    {
        // 1フレームだけ待って解除
        yield return null;
        shieldScript.isBroken = false;
    }

    private void SetAnim()
    {
        if (_anim)
        {
            int Enemy_HP = 0;

            Enemy_HP = GetHP();//体力を記録

            //一旦falseに
            _anim.SetBool("IsDead", false);

            if (Enemy_HP <= 0)
            {
                _anim.SetBool("IsDead", true);
                shieldScript.BreakShield();
            }
        }
    }
}
