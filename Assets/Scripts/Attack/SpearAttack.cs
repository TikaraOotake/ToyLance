using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;
public class SpearAttack : MonoBehaviour
{
    [SerializeField]
    private int AttackValue = 1;//攻撃力
    [SerializeField]
    private AttackType attackType;//攻撃タイプ

    private GameObject HitGround;//接触した地形

    public void SetAttackValue(int _AttackValue)
    {
        AttackValue = _AttackValue;
    }
    public int GetAttackValue()
    {
        return AttackValue;
    }
    public void SetAttackType(AttackType _attackType)
    {
        attackType = _attackType;
    }
    public AttackType GetAttackType()
    {
        return attackType;
    }
    public GameObject GetHitGround()
    {
        return HitGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BreakableObject _breakable = collision.GetComponent<BreakableObject>();
        if (_breakable != null)
        {
            _breakable.Break();
        }

        EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
        if (_enemyHealth != null)
        {
            _enemyHealth.TakeDamage(AttackValue, transform.position, true);
        }

        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
           // Debug.Log("地形と接触！");

            //オブジェクトを登録
            HitGround = collision.gameObject;
        }
    }
}
