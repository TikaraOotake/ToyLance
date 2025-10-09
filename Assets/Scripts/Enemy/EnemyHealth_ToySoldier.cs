using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] 
    private GameObject Shield;              //��

    private Enemy_shield shieldScript;

    private Animator _anim;                 //�A�j���[�^�[

    private void Start()
    {
        shieldScript = GetComponentInChildren<Enemy_shield>();

        _anim = GetComponent<Animator>();
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

        if (shieldScript.isBroken)
        {
            return;
        }

        GameObject Player = GameManager_01.GetPlayer();

        float PlayerPosX = 0.0f;    //�v���C���[��x���W
        float EnemyPosX = 0.0f;     //�G��x���W
        float ShieldPosX = 0.0f;    //����x���W

        if (Player != null && Shield != null && !shieldScript.isBroken) 
        {
            //���ꂼ���x���W���擾
            PlayerPosX = Player.transform.position.x;
            EnemyPosX = transform.position.x;
            ShieldPosX = Shield.transform.position.x;
        }

        //�v���C���[��x���W�ƓG��x���W����
        float minX = Mathf.Min(PlayerPosX, EnemyPosX);      //�ŏ��l
        float maxX = Mathf.Max(PlayerPosX, EnemyPosX);      //�ő�l

        //����x���W���v���C���[�ƓG�̊Ԃɂ��邩
        bool isShieldBetween = ShieldPosX > minX && ShieldPosX < maxX;

        //����x���W���v���C���[�ƓG�̊ԂɂȂ��ꍇ
        if (!isShieldBetween)
        {
            //�_���[�W����
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
        // 1�t���[�������҂��ĉ���
        yield return null;
        shieldScript.isBroken = false;
    }

    private void SetAnim()
    {
        if (_anim)
        {
            int Enemy_HP = 0;

            Enemy_HP = GetHP();//�̗͂��L�^

            //��Ufalse��
            _anim.SetBool("IsDead", false);

            if (Enemy_HP <= 0)
            {
                _anim.SetBool("IsDead", true);
                shieldScript.BreakShield();
            }
        }
    }
}
