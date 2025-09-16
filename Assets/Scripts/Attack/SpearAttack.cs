using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;
public class SpearAttack : MonoBehaviour
{
    [SerializeField]
    private int AttackValue = 1;//�U����
    [SerializeField]
    private AttackType attackType;//�U���^�C�v

    private GameObject HitGround;//�ڐG�����n�`

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
           // Debug.Log("�n�`�ƐڐG�I");

            //�I�u�W�F�N�g��o�^
            HitGround = collision.gameObject;
        }
    }
}
