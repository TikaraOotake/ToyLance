using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;
public class SpearAttack : MonoBehaviour
{
    [SerializeField]
    private int AttackValue = 1;//çUåÇóÕ
    [SerializeField]
    private AttackType attackType;//çUåÇÉ^ÉCÉv

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
}
