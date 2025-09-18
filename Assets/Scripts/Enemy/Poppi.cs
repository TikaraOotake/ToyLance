using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppi : MonoBehaviour
{
    private GameObject Player;//プレイヤーを格納

    [SerializeField] private Collider2D JampColl;//ジャンプするための当たり判定
    [SerializeField] private float JampValue;//ジャンプ力

    [SerializeField] private float ActionTimer;//行動タイマー

    [SerializeField]
    Animator Head_animator;//頭のアニメーター

    enum ActionStatus
    {
        Standby,//待機状態
        Attacking,//攻撃状態
        Stoping,//停止中
        Returning,//復帰中
    }

    private ActionStatus actionStatus;//行動Status

    [SerializeField] private float FindLength = 1.0f;//プレイヤーを見つける距離

    [SerializeField] private GameObject EnemyAttackCollPrefab;//攻撃判定プレハブ
    [SerializeField] private GameObject EnemyAttackColl;
    [SerializeField] private GameObject Head;//頭

    void Start()
    {
        Player =  GameManager_01.GetPlayer();
    }


    void Update()
    {
        if (actionStatus == ActionStatus.Standby)//待機
        {
            //槍が突き刺さっているか確認
            if (HasChildWithComponent<HalfHitFloor_Lance>() != null)
            {
                if (JampColl != null)
                {
                    //プレイヤーがジャンプ判定に衝突しているか判定
                    GameObject Player = Collision_Manager.GetTouchingObjectWithLayer(JampColl, "Player");
                    if (Player != null)
                    {
                        //上にジャンプする
                        Rigidbody2D _rb = Player.GetComponent<Rigidbody2D>();
                        if (_rb != null)
                        {
                            Vector2 vel = _rb.velocity;
                            vel.y = JampValue;
                            _rb.velocity = vel;
                        }

                        GameObject Lance = HasChildWithComponent<HalfHitFloor_Lance>();//槍を破棄
                        if (Lance)
                        {
                            Destroy(Lance);
                        }

                        //攻撃に移行
                        actionStatus = ActionStatus.Attacking;
                        //攻撃時間設定
                        ActionTimer = 1.0f;

                        if (Head_animator != null) Head_animator.SetBool("IsAppear", true);//出現アニメーションに設定
                    }
                }
            }
            else
            {
                //通常の処理

                if (CheckPlayerOverlap(FindLength))
                {
                    //攻撃に設定
                    actionStatus = ActionStatus.Attacking;
                    //攻撃時間設定
                    ActionTimer = 1.0f;

                    if (Head_animator != null) Head_animator.SetBool("IsAppear", true);//出現アニメーションに設定
                }
            }
        }
        else if (actionStatus == ActionStatus.Attacking)//攻撃
        {
            if (ActionTimer <= 0.5f)
            {
                //攻撃判定生成
                if (EnemyAttackCollPrefab != null && EnemyAttackColl == null)
                {
                    Vector2 Pos = transform.position;
                    if (Head != null) Pos = Head.transform.position;
                    EnemyAttackColl = Instantiate(EnemyAttackCollPrefab, Pos, Quaternion.identity);

                    //大きさを合わせる
                    Vector2 Scale = EnemyAttackColl.transform.localScale;
                    Scale.x *= transform.localScale.x;
                    Scale.y *= transform.localScale.y;
                    EnemyAttackColl.transform.localScale = Scale;
                }
            }
            if (ActionTimer <= 0.0f)
            {
                //攻撃判定を破棄
                if (EnemyAttackColl != null)
                {
                    Destroy(EnemyAttackColl);
                    EnemyAttackColl = null;
                }

                //行動を復帰中に
                actionStatus = ActionStatus.Returning;

                //行動時間設定
                ActionTimer = 2.0f;

                if (Head_animator != null) Head_animator.SetBool("IsAppear", false);//閉じこもりアニメーションに設定
            }
        }
        else if (actionStatus == ActionStatus.Stoping)//行動停止
        {

        }
        else if (actionStatus == ActionStatus.Returning)//復帰中
        {
            //槍が突き刺さっているか確認
            if (HasChildWithComponent<HalfHitFloor_Lance>() != null)
            {
                actionStatus = ActionStatus.Standby;//槍が刺さっている場合はすぐに待機状態に戻る
            }

            if (ActionTimer <= 0.0f)
            {
                actionStatus = ActionStatus.Standby;//待機状態に戻る
            }
        }

        //タイマー更新
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
    }

    private bool CheckPlayerOverlap(float _FindLength)//プレイヤーが一定範囲内に入ったか確認
    {
        if (Player != null)
        {
            Vector2 PlayerPos = Player.transform.position;
            Vector2 ThisPos = transform.position;
            Vector2 Length = PlayerPos - ThisPos;//距離を算出
            if (_FindLength >= Length.magnitude)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject HasChildWithComponent<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        foreach (var comp in components)
        {
            if (comp.gameObject != gameObject)
                return comp.gameObject;
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ThrowLance_01 Lance =  collision.gameObject.GetComponent<ThrowLance_01>();
        if (Lance != null)
        {
            actionStatus = ActionStatus.Stoping;
        }
    }
}
