using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppi : MonoBehaviour
{
    private GameObject Player;//�v���C���[���i�[

    [SerializeField] private Collider2D JampColl;//�W�����v���邽�߂̓����蔻��
    [SerializeField] private float JampValue;//�W�����v��

    [SerializeField] private float ActionTimer;//�s���^�C�}�[

    [SerializeField]
    Animator Head_animator;//���̃A�j���[�^�[

    enum ActionStatus
    {
        Standby,//�ҋ@���
        Attacking,//�U�����
        Stoping,//��~��
        Returning,//���A��
    }

    private ActionStatus actionStatus;//�s��Status

    [SerializeField] private float FindLength = 1.0f;//�v���C���[�������鋗��

    [SerializeField] private GameObject EnemyAttackCollPrefab;//�U������v���n�u
    [SerializeField] private GameObject EnemyAttackColl;
    [SerializeField] private GameObject Head;//��

    void Start()
    {
        Player =  GameManager_01.GetPlayer();
    }


    void Update()
    {
        if (actionStatus == ActionStatus.Standby)//�ҋ@
        {
            //�����˂��h�����Ă��邩�m�F
            if (HasChildWithComponent<HalfHitFloor_Lance>() != null)
            {
                if (JampColl != null)
                {
                    //�v���C���[���W�����v����ɏՓ˂��Ă��邩����
                    GameObject Player = Collision_Manager.GetTouchingObjectWithLayer(JampColl, "Player");
                    if (Player != null)
                    {
                        //��ɃW�����v����
                        Rigidbody2D _rb = Player.GetComponent<Rigidbody2D>();
                        if (_rb != null)
                        {
                            Vector2 vel = _rb.velocity;
                            vel.y = JampValue;
                            _rb.velocity = vel;
                        }

                        GameObject Lance = HasChildWithComponent<HalfHitFloor_Lance>();//����j��
                        if (Lance)
                        {
                            Destroy(Lance);
                        }

                        //�U���Ɉڍs
                        actionStatus = ActionStatus.Attacking;
                        //�U�����Ԑݒ�
                        ActionTimer = 1.0f;

                        if (Head_animator != null) Head_animator.SetBool("IsAppear", true);//�o���A�j���[�V�����ɐݒ�
                    }
                }
            }
            else
            {
                //�ʏ�̏���

                if (CheckPlayerOverlap(FindLength))
                {
                    //�U���ɐݒ�
                    actionStatus = ActionStatus.Attacking;
                    //�U�����Ԑݒ�
                    ActionTimer = 1.0f;

                    if (Head_animator != null) Head_animator.SetBool("IsAppear", true);//�o���A�j���[�V�����ɐݒ�
                }
            }
        }
        else if (actionStatus == ActionStatus.Attacking)//�U��
        {
            if (ActionTimer <= 0.5f)
            {
                //�U�����萶��
                if (EnemyAttackCollPrefab != null && EnemyAttackColl == null)
                {
                    Vector2 Pos = transform.position;
                    if (Head != null) Pos = Head.transform.position;
                    EnemyAttackColl = Instantiate(EnemyAttackCollPrefab, Pos, Quaternion.identity);

                    //�傫�������킹��
                    Vector2 Scale = EnemyAttackColl.transform.localScale;
                    Scale.x *= transform.localScale.x;
                    Scale.y *= transform.localScale.y;
                    EnemyAttackColl.transform.localScale = Scale;
                }
            }
            if (ActionTimer <= 0.0f)
            {
                //�U�������j��
                if (EnemyAttackColl != null)
                {
                    Destroy(EnemyAttackColl);
                    EnemyAttackColl = null;
                }

                //�s���𕜋A����
                actionStatus = ActionStatus.Returning;

                //�s�����Ԑݒ�
                ActionTimer = 2.0f;

                if (Head_animator != null) Head_animator.SetBool("IsAppear", false);//��������A�j���[�V�����ɐݒ�
            }
        }
        else if (actionStatus == ActionStatus.Stoping)//�s����~
        {

        }
        else if (actionStatus == ActionStatus.Returning)//���A��
        {
            //�����˂��h�����Ă��邩�m�F
            if (HasChildWithComponent<HalfHitFloor_Lance>() != null)
            {
                actionStatus = ActionStatus.Standby;//�����h�����Ă���ꍇ�͂����ɑҋ@��Ԃɖ߂�
            }

            if (ActionTimer <= 0.0f)
            {
                actionStatus = ActionStatus.Standby;//�ҋ@��Ԃɖ߂�
            }
        }

        //�^�C�}�[�X�V
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
    }

    private bool CheckPlayerOverlap(float _FindLength)//�v���C���[�����͈͓��ɓ��������m�F
    {
        if (Player != null)
        {
            Vector2 PlayerPos = Player.transform.position;
            Vector2 ThisPos = transform.position;
            Vector2 Length = PlayerPos - ThisPos;//�������Z�o
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
