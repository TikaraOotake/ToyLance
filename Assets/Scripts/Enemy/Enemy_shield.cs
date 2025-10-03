using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;

public class Enemy_shield : MonoBehaviour
{
    private Enemy_move enemyMove;

    public bool isBroken = false;   //壊れているかのフラグ

    Animator anim;

    [SerializeField] private GameObject Enemy;

    GameObject Player;

    SpearAttack spear;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }

        Player = GameManager_01.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        //槍を取得
        spear = collision.GetComponent<SpearAttack>();
        if (spear != null) 
        {
            AttackType attackType= spear.GetAttackType();
            //突き攻撃なら
            if (attackType == AttackType.Trust) 
            {
                float PlayerPosX = 0.0f;    //プレイヤーのx座標
                float EnemyPosX = 0.0f;     //敵のx座標
                float ShieldPosX = 0.0f;    //盾のx座標

                if (Player != null && Enemy != null)
                {
                    //それぞれのx座標を取得
                    PlayerPosX = Player.transform.position.x;
                    EnemyPosX = Enemy.transform.position.x;
                    ShieldPosX = transform.position.x;
                }

                //プレイヤーのx座標と盾のx座標から
                float minX = Mathf.Min(PlayerPosX, ShieldPosX);     //最小値
                float maxX = Mathf.Max(PlayerPosX, ShieldPosX);     //最大値

                //敵のx座標がプレイヤーと盾の間にあるか
                bool isEnemyBetween = EnemyPosX > minX && EnemyPosX < maxX;

                //敵のx座標がプレイヤーと盾の間にない場合
                if (!isEnemyBetween)
                {
                    //盾の破壊処理
                    BreakShield();
                }
            }
        }

        if (collision.GetComponent<SpearProjectile>() != null)
        {
            return;
        }
    }

    //盾の破壊処理
    public void BreakShield()
    {
        if (isBroken)
        {
            return;
        }

        //壊れている
        isBroken = true;

        var enemyHealth = GetComponentInParent<EnemyHealth_ToySoldier>();
        if (enemyHealth != null)
        {
            enemyHealth.SetShieldJustBrokeFlag();
        }

        if (enemyMove != null)
        {
            //停止処理
            enemyMove.PauseMovement(1f);
        }

        //非表示
        //gameObject.SetActive(false);
        StartCoroutine(DelaySetActive(false));

        //6秒後に盾を再生
        Invoke(nameof(RestoreShield), 6f);
    }

    IEnumerator DelaySetActive(bool _flag)
    {
        yield return null;
        yield return null;

        //数F後に非有効に
        gameObject.SetActive(_flag);
    }

    //盾の再生処理
    private void RestoreShield()
    {
        //壊れていない
        isBroken = false;

        //表示
        gameObject.SetActive(true);
    }

    //盾のアニメーションの再生処理
    public void SetWalkSpeed(int speed)
    {
        if (anim != null)
        {
            anim.SetInteger("WalkSpeed", speed);
        }
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
