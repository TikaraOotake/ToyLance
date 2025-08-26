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
            if (IsShieldBlocking(transform.position, _enemyHealth.transform.position))
            {
                Debug.Log("b");
                return;
            }
            _enemyHealth.TakeDamage(AttackValue, transform.position, true);
        }
    }

    private bool IsShieldBlocking(Vector2 attackerPos, Vector2 enemyPos)
    {
        Vector2 direction = (enemyPos - attackerPos).normalized;
        float distance = Vector2.Distance(attackerPos, enemyPos);

        Debug.DrawRay(attackerPos, direction * distance, Color.red, 1f);
        //int layerMask = LayerMask.GetMask("Platform");
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackerPos, direction, distance/*, layerMask*/);
        foreach (var hit in hits)
        {
            Debug.Log($"Ray hit: {hit.collider.gameObject.name}, Tag: {hit.collider.tag}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            if (hit.collider != null && hit.collider.CompareTag("Shield"))
            {
                Enemy_shield shield = hit.collider.GetComponent<Enemy_shield>();
                if (shield != null && !shield.isBroken)
                {
                    Debug.Log("c");
                    return true;
                }
            }
        }
        return false;
    }
}
