using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppi : MonoBehaviour
{
    private GameObject Player;//プレイヤーを格納

    [SerializeField] private Collider2D JampColl;//ジャンプするための当たり判定
    [SerializeField] private float JampValue;//ジャンプ力

    [SerializeField] private float ActionTimer;//行動タイマー

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
            if (HasChildWithComponent<HalfHitFloor_Lance>())
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

                        //攻撃に移行
                        actionStatus = ActionStatus.Attacking;
                    }

                }


                //エネミー状態を解除
                this.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                //通常の処理

                //エネミー状態に
                this.gameObject.layer = LayerMask.NameToLayer("Enemy");

                if (CheckPlayerOverlap(FindLength))
                {
                    //攻撃に設定
                    actionStatus = ActionStatus.Attacking;

                    //攻撃時間設定
                    ActionTimer = 1.0f;
                }
            }
        }
        else if (actionStatus == ActionStatus.Attacking)//攻撃
        {

        }
        else if (actionStatus == ActionStatus.Stoping)//行動停止
        {

        }
        else if (actionStatus == ActionStatus.Returning)//復帰中
        {

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

    public bool HasChildWithComponent<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        foreach (var comp in components)
        {
            if (comp.gameObject != gameObject)
                return true;
        }
        return false;
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
