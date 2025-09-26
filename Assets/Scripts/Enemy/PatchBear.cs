using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PatchBear : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject FlipObj;//�ړ��ɍ��킹�Ĕ��]������I�u�W�F�N�g

    [SerializeField] private GameObject CottonPrefab;       //�R�b�g����Prefab
    [SerializeField] private float CottonSpownRate = 1.0f;  //�R�b�g���̐����p�x
    private bool CottonSpownFlag = false;                   //�R�b�g�������t���O
    private Vector2 Position_old;                           //�O�ɂ������W
    private float TotalMoveValue;                           //�݌v�ړ�����

    [SerializeField]
    private float MoveValue;//�ړ����x
    [SerializeField]
    private int AngerLevel = 0;//�{�背�x��
    [SerializeField]
    private bool IsChase;//�ǐՃt���O
    [SerializeField]
    private float SearchLength;//���G�͈�

    private Vector2 BaseScale;
    private float BaseSpeed;

    [SerializeField] private float ActionTimer;//�s������
    [SerializeField] private float MoveFlipCoolTime = 0.5f;//�U������̃N�[���^�C��
    [SerializeField] private float MoveFlipCoolTimer;//�U������̃N�[���^�C�}�[

    [SerializeField]
    private float MoveWay = 0.0f;
    [SerializeField]
    private float SearchDistance;//Player��T������

    [SerializeField] Collider2D CliffCheckColl;//�R�[���m�F����R���C�_�[
    [SerializeField] Collider2D WallCheckColl;//�ǂ��m�F����R���C�_�[

    [SerializeField] private bool IsWalk;

    [SerializeField]
    private Animator _anim;//�A�j���[�^�[
    [SerializeField]
    private Rigidbody2D _rb;//�����R���|
    [SerializeField]
    private SpriteRenderer _sr;//�X�v���C�g�����_���[

    [SerializeField]
    private EnemyHealth _eh;//�G�̗�
    private int HP_old;

    public struct AngerStatus
    {
        public float MoveValue;
        public float Scale;
    }
    [SerializeField] private AngerStatus[] PhaseStatusList = new AngerStatus[3]; 

    enum ActionStatus
    {
        Idol,//�ҋ@
        Walk,//���s
        Dead,//���S
    }
    [SerializeField]
    private ActionStatus actionStatus;

    private void Awake()
    {
        BaseScale = transform.localScale;//BaseScale�ݒ�
        BaseSpeed = MoveValue;
    }

    private void OnEnable()
    {
        CottonSpownFlag = false;//������
    }
    void Start()
    {
        //�擾
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _eh = GetComponent<EnemyHealth>();

        if (_eh) HP_old = _eh.GetHP();//�̗͂��L�^

        if (0 < PhaseStatusList.Length)
        {
            PhaseStatusList[0].MoveValue = 1.0f;
            PhaseStatusList[0].Scale = 1.0f;
        }
        if (1 < PhaseStatusList.Length)
        {
            PhaseStatusList[1].MoveValue = 3.0f;
            PhaseStatusList[1].Scale = 1.1f;
        }
        if (2 < PhaseStatusList.Length)
        {
            PhaseStatusList[2].MoveValue = 5.0f;
            PhaseStatusList[2].Scale = 1.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Status���瑬�x�ƃT�C�Y��K�p
        if (AngerLevel >= 0 && AngerLevel < PhaseStatusList.Length)//�v�f�`�F�b�N
        {
            float Scale = PhaseStatusList[AngerLevel].Scale;
            transform.localScale = BaseScale * Scale;
            MoveValue = PhaseStatusList[AngerLevel].MoveValue * BaseSpeed;
        }

        if(actionStatus == ActionStatus.Idol)
        {
            if (_rb)
            {
                _rb.velocity = new Vector2(0.0f, _rb.velocity.y);//�Î~����
            }
        }
        else if(actionStatus == ActionStatus.Walk)
        {
            Walk();
        }
        else if (actionStatus == ActionStatus.Dead)
        {
            if (_eh != null)
            {
                int hp = _eh.GetHP();//HP���擾
                if (hp > 0)
                {
                    AngerLevel = 0;
                    actionStatus = ActionStatus.Idol;//�ҋ@��Ԃɖ߂�
                }
            }
        }

        SearchPlayer();
        SpownCotton();
        
        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        //�^�C�}�[�X�V
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
        MoveFlipCoolTimer = Mathf.Max(0.0f, MoveFlipCoolTimer - Time.deltaTime);

        SetAnim();

        if (_eh != null)
        {
            int hp = _eh.GetHP();//HP���擾
            
            if (hp != HP_old && hp < HP_old)//�_���[�W���󂯂���
            {
                AngerLevel = Mathf.Min(3, AngerLevel + 1);//�{�背�x�����グ��(�ő�l:3)
                IsChase = true;                           //�ǐՏ�Ԃ�
                CottonSpownFlag = true;                   //�R�b�g���̐����J�n
            }

            if(hp<=0.0f)//�̗͂�0�ȉ��̏ꍇ
            {
                actionStatus = ActionStatus.Dead;//���S��Ԃ�
            }

            HP_old = hp;//�̗͂��L�^
        }
    }
    private void Walk()
    {
        if (CliffCheckColl != null && MoveFlipCoolTimer <= 0.0f)
        {
            if (!Collision_Manager.GetTouchingObjectWithLayer(CliffCheckColl, "Platform"))
            {
                MoveFlipCoolTimer = MoveFlipCoolTime;//�^�C�}�[�Z�b�g
                MoveWay *= -1;//�ړ��������]
            }
        }

        if (FlipObj != null)
        {
            if (MoveWay > 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
            else if (MoveWay < 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        if (_sr != null)
        {
            if (MoveWay > 0)
            {
                _sr.flipX = true;
            }
            else if (MoveWay < 0)
            {
                _sr.flipX = false;
            }
        }


        if (_rb)
        {
            _rb.velocity = new Vector2(MoveWay * MoveValue, _rb.velocity.y);
        }
    }
    private void SearchPlayer()
    {
        Player = GameManager_01.GetPlayer();
        if (Player != null)
        {
            float Length = Vector2.Distance(Player.transform.position, transform.position);
            if (SearchLength > Length)
            {
                //���G�͈͓��ɓ���ΒǐՃ��[�h��
                IsChase = true;
            }

            if (IsChase && MoveFlipCoolTimer <= 0.0f)
            {
                //�v���C���[�̕����Ɍ��������킹��
                float Direction = Player.transform.position.x - transform.position.x;
                if (Direction >= 1.0f)
                {
                    MoveWay = +1.0f;
                }
                else if (Direction <= -1.0f)
                {
                    MoveWay = -1.0f;
                }
            }
        }
    }
    private void SpownCotton()
    {
        if (CottonSpownFlag)
        {
            //�ړ��������v�Z
            Vector2 Pos = transform.position;
            Vector2 LengthVec = Pos - Position_old;
            TotalMoveValue += LengthVec.magnitude;//���������Z

            //���W���X�V
            Position_old = transform.position;

            //��苗���𒴂�����
            if (TotalMoveValue >= CottonSpownRate)
            {
                //�R�b�g���𐶐�
                if (CottonPrefab != null)
                {
                    GameObject Cotton = Instantiate(CottonPrefab, transform.position + new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity);
                    Rigidbody2D _rb = Cotton.GetComponent<Rigidbody2D>();
                    if (_rb != null)
                    {
                        _rb.velocity = new Vector2(_rb.velocity.x, 1.0f);//��ɒ��˂�
                    }
                }

                TotalMoveValue = 0.0f;//�݌v�ړ��������Z�b�g
            }
        }
    }
    private void SetAction()
    {
        //��U���Z�b�g
        IsWalk = false;

        if (0.5f >= Random.Range(0.0f, 1.0f))
        {
            //�ҋ@
            actionStatus = ActionStatus.Idol;
            MoveWay = 0.0f;
        }
        else
        {
            //�ړ�
            actionStatus = ActionStatus.Walk;
            if (0.5f >= Random.Range(0.0f, 1.0f))
            {
                //�E�ړ�
                MoveWay = +1.0f;
            }
            else
            {
                //���ړ�
                MoveWay = -1.0f;
            }
        }

        //���ԃZ�b�g
        ActionTimer = Random.Range(1.0f, 3.0f);
    }
    private void SetAnim()
    {
        if(_anim)
        {
            int Enemy_HP = 0;
            if (_eh)
            {
                Enemy_HP = _eh.GetHP();//�̗͂��L�^
            } 

            //��Ufalse��
            _anim.SetBool("IsIdle", false);
            _anim.SetBool("IsWalk", false);
            _anim.SetBool("IsDead", false);

            if (Enemy_HP <= 0)
            {
                _anim.SetBool("IsDead", true);
            }
            else if (actionStatus == ActionStatus.Walk)
            {
                _anim.SetBool("IsWalk", true);
            }
            else
            {
                _anim.SetBool("IsIdle", true);
            }

            //�{�背�x�����X�V
            _anim.SetInteger("AngerLevel", AngerLevel);
        }
    }
}
