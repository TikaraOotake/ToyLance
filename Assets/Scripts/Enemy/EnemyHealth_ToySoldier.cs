using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] 
    private GameObject Shield;              //��

    private Enemy_shield shieldScript;

    private bool shieldJustBroke = false;   //�������Ă��邩�̃t���O

    private void Start()
    {
        shieldScript = GetComponentInChildren<Enemy_shield>();
    }

    public override void TakeDamage(int dmg, Vector2 attackerPos, Collider2D _coll, bool doKnockback = true)
    {

        if (Shield == null || _coll == null)//�����U���̓����蔻�肪�Ȃ�
        {
            TakeDamage(dmg, attackerPos, doKnockback);
            Debug.Log("�����U���̓����蔻�肪����܂���");
            return;
        }

        if (Shield.activeSelf == false)//������A�N�e�B�u
        {
            TakeDamage(dmg, attackerPos, doKnockback);
            Debug.Log("�����L���ł͂���܂���");
            return;
        }

        //���ƍU�����d�Ȃ��Ă��邩
        bool result = false;
        Collider2D Shield_Coll = Shield.GetComponent<Collider2D>();
        if (Shield_Coll != null && _coll != null)
        {
            result = Collision_Manager.AreCollidersTouchingAny(Shield_Coll, _coll);
            if (result == false) 
            {
                TakeDamage(dmg, attackerPos, doKnockback);
                Debug.Log("���ƍU������͏Փ˂��Ă��܂���");
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

        //�v���C���[�ƓG�̊Ԃɏ����Ȃ�������
        if (!isShieldBetween)
        {
            //�_���[�W����
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
        // 1�t���[�������҂��ĉ���
        yield return null;
        shieldJustBroke = false;
    }
}
