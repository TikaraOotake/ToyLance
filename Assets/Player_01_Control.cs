using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private GameObject LancePrefab;//�������̃v���n�u

    [SerializeField]
    private float MoveValue = 1.0f;//�ړ���
    [SerializeField]
    private float JumpValue = 1.0f;//�W�����v��

    [SerializeField]
    private float LanceSpeed;//�����x

    [SerializeField]
    private Rigidbody2D _rb;//�����R���|�[�l���g
    [SerializeField]
    private Animator _anim;//�A�j���[�^�[�R���|�[�l���g
    [SerializeField]
    private SpriteRenderer _sr;//SpriteRenderer�R���|�[�l���g

    private bool FlipX;//�v���C���[�̌������L�^

    [SerializeField] private bool IsJump = false;
    [SerializeField] private bool IsMove = false;
    [SerializeField] private bool IsThrustAtk = false;
    [SerializeField] private bool IsThrowAtk = false;

    [SerializeField]
    private Collider2D LandingCheckCollider;//���n�`�F�b�N�R���C�_�[

    [SerializeField]
    private float KnockBackValue = 1.0f;//�m�b�N�o�b�N��
    [SerializeField]
    private float KonokBackTime = 0.5f;//�m�b�N�o�b�N����
    private float KnockBackTimer;//�m�b�N�o�b�N�^�C�}�[
   
    enum PlayerStatus
    {
        Fine,
        HitDamage,
        Dead,
    }
    PlayerStatus playerStatus;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("���W�b�g�{�f�B�̎擾�Ɏ��s");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");

        KnockBackTimer = KonokBackTime;//�^�C�}�[������
    }

    void Start()
    {
        playerStatus = PlayerStatus.Fine;//�X�e�[�^�X��ʏ��
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus == PlayerStatus.Fine)
        {
            Move();
            Jump();
            Throw();
        }
        else
        {
            //...
        }



        SetAnim();

        //�^�C�}�[�X�V
        KnockBackTimer = Mathf.Max(0.0f, KnockBackTimer - Time.deltaTime);
        if (KnockBackTimer <= 0.0f) playerStatus = PlayerStatus.Fine;//�ȈՎ���
    }

    private void Move()
    {
        float MoveWay = 0.0f;
        if (Input.GetKey(KeyCode.D))
        {
            MoveWay = +1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveWay = -1.0f;
        }

        //�A�j���[�V�����̕ύX
        if (MoveWay != 0.0f)
        {
            IsMove = true;
        }
        else
        {
            IsMove = false;
        }

        //Sprite�̔��]
        if (_sr)
        {
            if (MoveWay > 0.0f)//�E����
            {
                FlipX = false;
            }
            else if (MoveWay < 0.0f)//������
            {
                FlipX = true;
            }
            _sr.flipX = FlipX;
        }

        //�ړ��ʂ̑��
        if (_rb != null)
        {
            //�ړ��ʎ擾
            Vector2 MoveVelocity = _rb.velocity;
            MoveVelocity.x = MoveWay * MoveValue;
            _rb.velocity = MoveVelocity;//���
        }
    }
    private void Jump()
    {
        if (GetTouchingObjectWithLayer(LandingCheckCollider,"Platform"))
        {
            if (_rb != null)
            {
                if (_rb.velocity.y <= 0.0f)
                {
                    IsJump = false;//�~�������n��Ȃ�W�����v���I�t
                }
            }

            //����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //�ړ��ʎ擾
                Vector2 MoveVelocity = _rb.velocity;
                MoveVelocity.y = JumpValue;
                _rb.velocity = MoveVelocity;//���

                IsJump = true;//�A�j���[�V�������W�����v��
            }
        }
    }

    public void Throw()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (LancePrefab != null)
            {
                Quaternion Rot = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                if (!FlipX)
                {
                    Rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                GameObject Lance = Instantiate(LancePrefab, transform.position, Rot);

                if (Lance.TryGetComponent(out SpearProjectile stick))
                {
                    stick.Init(1, LanceSpeed);
                }
            }
        }
    }

    /// <summary>
    /// �w�肵���R���C�_�[�ɐڐG���Ă���A�w�背�C���[���̃I�u�W�F�N�g���擾���܂��B
    /// </summary>
    /// <param name="collider">�Ώۂ�Collider2D</param>
    /// <param name="layerName">�T�����C���[��</param>
    /// <returns>�Y�����C���[��GameObject�i�Ȃ����null�j</returns>
    public static GameObject GetTouchingObjectWithLayer(Collider2D collider, string layerName)
    {
        int targetLayer = LayerMask.NameToLayer(layerName);
        if (targetLayer == -1)
        {
            Debug.LogWarning($"�w�肳�ꂽ���C���[���u{layerName}�v�͑��݂��܂���B");
            return null;
        }

        // �ꎞ�I�Ȕz��i�ő吔��K���Ɋm�ہj
        Collider2D[] results = new Collider2D[10];
        int count = collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);

        for (int i = 0; i < count; i++)
        {
            if (results[i] != null && results[i].gameObject.layer == targetLayer)
            {
                return results[i].gameObject;
            }
        }

        return null;
    }

    private void SetAnim()
    {
        if (_anim == null) return;//Anim���ݒ肳��Ă��Ȃ�������I��

        //�S�Ĉ�U���Z�b�g
        _anim.SetBool("isWalk", false);
        _anim.SetBool("isJumping", false);
        _anim.SetBool("DoMeleeAttack", false);

        //�D��ȕ��قǏ�ɂȂ�ׂ�
        if (IsThrowAtk)//����
        {

        }
        else if(IsThrustAtk)//�˂�
        {
            _anim.SetBool("DoMeleeAttack", true);
        }
        else if(IsJump)//�W�����v
        {
            _anim.SetBool("isJumping", true);
        }
        else if(IsMove)//����
        {
            _anim.SetBool("isWalk", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //�ǂ���̕�������Ԃ����������ׂ�
            float Way = collision.transform.position.x - transform.position.x;

            //�����ɂ��킹�Đ�����΂�
            if (_rb)
            {
                if (Way > 0)//����
                {
                    _rb.velocity += new Vector2(-KnockBackValue, 0.0f);
                }
                else if (Way < 0)//�E��
                {
                    _rb.velocity += new Vector2(KnockBackValue, 0.0f);
                }
            }

            KnockBackTimer = KonokBackTime;//�^�C�}�[�Z�b�g

            playerStatus = PlayerStatus.HitDamage;//�X�e�[�^�X�Z�b�g

            //�}�l�[�W���[�ɃJ�����U���˗�
            CameraManager.SetShakeCamera();
        }
    }
}
