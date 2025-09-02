using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private GameObject LancePrefab;//�������̃v���n�u
    [SerializeField]
    private GameObject AttackPrefab;//�ߋ����F�����U���@�p�̃R���C�_�[�v���n�u
    [SerializeField]
    private GameObject AttackObj;//

    [SerializeField] private GameObject FlipObj;//Player�̌����ɍ��킹�Ĕ��]����q�I�u�W�F�N�g

    [SerializeField]
    private float MoveValue = 1.0f;//�ړ���
    [SerializeField]
    private float JumpValue = 1.0f;//�W�����v��

    [SerializeField] private float HP;//�̗�
    [SerializeField] private float HP_Max = 5.0f;//�ő�̗�

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
    [SerializeField] private bool IsFallAtk = false;

    [SerializeField] private bool IsLanding = false;//���n�������肷��
    [SerializeField] private bool IsLanding_old = false;

    [SerializeField] private Collider2D LandingCheckCollider;//���n�`�F�b�N�R���C�_�[
    [SerializeField] private GameObject FallAttackPos;//�����U���R���C�_�[�̍��W
    [SerializeField] private GameObject TrustAttackPos;//�˂��U���R���C�_�[�̍��W

    [SerializeField]
    private float KnockBackValue = 1.0f;//�m�b�N�o�b�N��
    [SerializeField]
    private float KonokBackTime = 0.5f;//�m�b�N�o�b�N����
    private float KnockBackTimer;//�m�b�N�o�b�N�^�C�}�[
    private float KnockBackTimer_old;
    [SerializeField]
    private float InvincibleTime = 1.0f;//���G����
    private float InvincibleTimer = 0.0f;//���G�^�C�}�[
    private float BlinkingTimer = 0.0f;//�_�Ń^�C�}�[

    [SerializeField]
    private float ThrowCooltime = 1.0f;//�����̃N�[���^�C��
    private float ThrowCooltimer;

    private float AtkTimer;//�U���^�C�}�[

    private float KeyboardInputTimer;

    [SerializeField] private int TrustAttackValue;//�h�ˍU����
    [SerializeField] private int ThrowAttackValue;//�����U����
    [SerializeField] private int FallAttackValue;//�����U����

    [SerializeField] private float TestRot;
    enum PlayerStatus
    {
        Fine,
        HitDamage,
        Dead,
    }
    PlayerStatus playerStatus;

    enum AtkStatus
    {
        None,//����
        Thrust,//�˂�
        Throw,//����
        Falling,//����
    }
    AtkStatus atkStatus = AtkStatus.None;
    private void Awake()
    {
        //�}�l�[�W���[�Ɏ��g��o�^
        GameManager_01.SetPlayer(this.gameObject);

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("���W�b�g�{�f�B�̎擾�Ɏ��s");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");

        HP = HP_Max;
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
            
            if(atkStatus==AtkStatus.None)
            {
                //��{����
                Move();
                Jump();
                Throw();
            }
            else
            {
                FallingAtk();
                TrustAtk();
            }

            InputAtkSetting();
        }
        else if (playerStatus == PlayerStatus.Dead)//���S���
        {
            if (_rb != null)
            {
                _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            }
            if (_sr != null)
            {
                _sr.flipY = true;
            }
        }

        GameObject Enemy = GetTouchingObjectWithLayer(GetComponent<Collider2D>(), "Enemy");
        if (Enemy != null)
        {
            SetDamage(Enemy);
        }

        SetAnim();

        CheckLanding_Update();

        GameManager_01.SetHP_UI((int)HP);

        //�^�C�}�[�X�V
        AtkTimer = Mathf.Max(0.0f, AtkTimer - Time.deltaTime);
        InvincibleTimer = Mathf.Max(0.0f, InvincibleTimer - Time.deltaTime);
        BlinkingTimer = Mathf.Max(0.0f, BlinkingTimer - Time.deltaTime);
        KnockBackTimer_old = KnockBackTimer;
        KnockBackTimer = Mathf.Max(0.0f, KnockBackTimer - Time.deltaTime);
        ThrowCooltimer = Mathf.Max(0.0f, ThrowCooltimer - Time.deltaTime);
        KeyboardInputTimer= Mathf.Max(0.0f, KeyboardInputTimer - Time.deltaTime);

        if (KnockBackTimer <= 0.0f && KnockBackTimer != KnockBackTimer_old)//�^�C�}�[��0�ɂȂ����u�Ԃ���
        {
            //�ȈՎ���
            playerStatus = PlayerStatus.Fine;//�X�e�[�^�X��ʏ��
            InvincibleTimer = InvincibleTime;//�^�C�}�[�Z�b�g
            BlinkingTimer = InvincibleTimer;//�_�Ń^�C�}�[�Z�b�g

            //���S�`�F�b�N
            if (HP <= 0.0f)
            {
                playerStatus = PlayerStatus.Dead;
            }
        }
    }
    private void InputAtkSetting()
    {
        if (AtkTimer > 0.0f) return;//�U�����͏��������Ȃ�

        if (Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") < -0.1f)) 
        {
            if (GetTouchingObjectWithLayer(LandingCheckCollider, "Platform") ||
                GetTouchingObjectWithLayer(LandingCheckCollider, "SpearPlatform"))
                return;//���n���ł���Ώ��O

            //�����U��
            atkStatus = AtkStatus.Falling;
            AtkTimer = 1.0f;
            if (_rb) _rb.velocity = new Vector2(0.0f, JumpValue * 0.5f);
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("Melee")) 
        {
            //�˂��U��
            atkStatus = AtkStatus.Thrust;
            AtkTimer = 0.5f;
            return;
        }
    }
    private void Move()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            KeyboardInputTimer = 1.0f;
        }

        float MoveWay = 0.0f;
        if (Input.GetKey(KeyCode.D) || (Input.GetAxis("Horizontal") > 0.1f && KeyboardInputTimer <= 0.0f))
        {
            MoveWay = +1.0f;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetAxis("Horizontal") < -0.1f && KeyboardInputTimer <= 0.0f))
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
        if (FlipObj != null)
        {
            if (MoveWay > 0.0f)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else if (MoveWay < 0.0f)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
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
        if (IsLanding)//�ڒn��������
        {
            if (_rb != null)
            {
                if (_rb.velocity.y <= 0.0f)
                {
                    IsJump = false;//�~�������n��Ȃ�W�����v���I�t
                }
                if (_rb.velocity.y >= JumpValue * 0.1f)//�㏸���x�����ȏ゠��ꍇ�W�����v���Ȃ�
                {
                    return;
                }
            }

            //����
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) 
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
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Ranged")) 
        {
            if (ThrowCooltimer > 0.0f) return;//�N�[���^�C�����ł���Ȃ�I��

            if (LancePrefab != null)
            {
                Vector3 Rot = new Vector3(0.0f, 0.0f, 0.0f);//�������`
                Vector2 ThrowVec = new Vector2(1.0f, 0.0f);//������x�N�g�����`

                if (Input.GetKey(KeyCode.W))//����͒��ł���Ύ΂߂�
                {
                    ThrowVec += new Vector2(0.0f, 1.0f);
                    ThrowVec.Normalize();//���K��
                    Rot.z += 45.0f;//�p�x���Ȃ���
                }

                ThrowVec *= LanceSpeed;//���x��K�p

                if (FlipX)
                {
                    Rot.y += 180.0f;
                    ThrowVec.x *= -1;//���x�̌����𔽓]
                }
                GameObject Lance = Instantiate(LancePrefab, transform.position, Quaternion.identity);//���𐶐�
                Lance.transform.eulerAngles = Rot;

                Rigidbody2D _rb = Lance.GetComponent<Rigidbody2D>();//�����R���|�擾
                if (_rb != null) _rb.velocity = ThrowVec;//���x���

                ThrowCooltimer = ThrowCooltime;//�N�[���^�C�}�[�Z�b�g

                return;
            }
        }
    }
    private void FallingAtk()
    {
        if (atkStatus == AtkStatus.Falling)
        {
            if (AtkTimer <= 0.8f)
            {
                if(!IsLanding)
                {
                    if (_rb) _rb.velocity = new Vector2(0.0f, -JumpValue * 2.0f);//�󒆎��̂݋}�~��
                }
                
                InvincibleTimer = 0.3f;//���G���ԃZ�b�g(��u)

                //�U������̐���
                if (AttackObj == null && AttackPrefab != null)
                {
                    AttackObj = Instantiate(AttackPrefab);

                    //�U���^�C�v�̐ݒ�
                    SpearAttack spearAttack = AttackObj.GetComponent<SpearAttack>();
                    if (spearAttack != null)
                    {
                        spearAttack.SetAttackType(AttackType.Fall);
                        spearAttack.SetAttackValue(FallAttackValue);
                    }
                }
                //�U������̐ݒ�X�V
                if (AttackObj != null && FallAttackPos != null)
                {
                    AttackObj.transform.localScale = FallAttackPos.transform.lossyScale;//�X�P�[��
                    AttackObj.transform.position = FallAttackPos.transform.position;//���W
                    AttackObj.transform.eulerAngles = FallAttackPos.transform.eulerAngles;//�p�x
                }

                IsFallAtk = true;//�����U��Anim
            }

            //�������������Ă����玞�ԉ���
            if (Input.GetKey(KeyCode.S))
            {
                if (AtkTimer <= 0.0f)//0�ȉ�
                {
                    AtkTimer = Time.deltaTime;
                }
            }

            //���n���肪�G�ƐڐG������L�����Z������
            if (GetTouchingObjectWithLayer(LandingCheckCollider, "Enemy"))
            {
                if (_rb) _rb.velocity = new Vector2(0.0f, JumpValue * 1.5f);//�ʏ�W�����v��菭���������˂�

                //�}�l�[�W���[�ɃJ�����U���˗�
                CameraManager.SetShakeCamera();

                AtkTimer = 0.0f;//�^�C�}�[��0��
            }

            if (IsLanding != IsLanding_old && IsLanding == true)//���n�����u�Ԃ���
            {
                CameraManager.SetShakeCamera();//�J������h�炷
            }

            //�I������
            if (AtkTimer <= 0.0f)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

                IsFallAtk = false;
            }
        }
    }
    private void TrustAtk()
    {
        if (atkStatus == AtkStatus.Thrust)
        {
            if (_rb) _rb.velocity = new Vector2(0.0f, _rb.velocity.y);//�~�܂�

            if (AtkTimer <= 0.5f)
            {
                IsThrustAtk = true;//�h�ˍU��Anim
            }

            if (AtkTimer <= 0.3f)
            {
                InvincibleTimer = 0.3f;//���G���ԃZ�b�g(��u)

                //�U������̐���
                if (AttackObj == null && AttackPrefab != null)
                {
                    AttackObj = Instantiate(AttackPrefab);

                    //�U���^�C�v�̐ݒ�
                    SpearAttack spearAttack = AttackObj.GetComponent<SpearAttack>();
                    if (spearAttack != null)
                    {
                        spearAttack.SetAttackType(AttackType.Trust);
                        spearAttack.SetAttackValue(TrustAttackValue);
                    }
                }
                //�U������̐ݒ�X�V
                if (AttackObj != null && TrustAttackPos != null)
                {
                    AttackObj.transform.localScale = TrustAttackPos.transform.lossyScale;//�X�P�[��
                    AttackObj.transform.position = TrustAttackPos.transform.position;//���W
                    AttackObj.transform.eulerAngles = TrustAttackPos.transform.eulerAngles;//�p�x
                }
            }

            //�I������
            if (AtkTimer <= 0.0f)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

                IsThrustAtk = false;
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

    private void CheckLanding_Update()
    {
        IsLanding_old = IsLanding;//�ύX�O�̏�Ԃ��L�^
        IsLanding = true;//������

        //���ꂪ���邩�`�F�b�N���A����Ί֐������̂܂܏I��
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "Platform") != null) return;
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "SpearPlatform") != null) return;
        if (GetTouchingObjectWithLayer(LandingCheckCollider, "PlayerPlatform") != null) return;

        //��(�񒅒n)�Ƃ��ċL�^
        IsLanding = false;
    }
    private void SetAnim()
    {
        if (_anim == null) return;//Anim���ݒ肳��Ă��Ȃ�������I��

        //�S�Ĉ�U���Z�b�g
        _anim.SetBool("isWalk", false);
        _anim.SetBool("isJumping", false);
        _anim.SetBool("isTrustAttack", false);
        _anim.SetBool("isDownAttacking", false);
        _anim.SetBool("doDamaged", false);

        //�D��ȕ��قǏ�ɂȂ�ׂ�
        if (KnockBackTimer > 0.0f)//��e���
        {
            _anim.SetBool("doDamaged", true);
        }
        else if (IsThrowAtk)//����
        {

        }
        else if (IsThrustAtk)//�˂�
        {
            _anim.SetBool("isTrustAttack", true);
        }
        else if(IsFallAtk)//�����U��
        {
            _anim.SetBool("isDownAttacking", true);
        }
        else if(IsJump)//�W�����v
        {
            _anim.SetBool("isJumping", true);
        }
        else if(IsMove)//����
        {
            _anim.SetBool("isWalk", true);
        }

        //�F�̕ύX
        if (_sr != null)
        {
            //��e���͐ԐF�ɂ�����
            if (KnockBackTimer > 0.0f)
            {
                _sr.color = Color.red;//�ԐF�ɃZ�b�g
            }
            else
            {
                _sr.color = Color.white;//���F�ɃZ�b�g
            }


            //�_�Ŏ��Ԓ��͓_�ł�����
            Color tempColor = _sr.color;
            if (BlinkingTimer > 0.0f)
            {
                if ((int)(BlinkingTimer * 10.0f) % 2 == 0)
                {
                    tempColor.a = 0.5f;
                }
                else
                {
                    tempColor.a = 1.0f;
                }
            }
            else
            {
                tempColor.a = 1.0f;
            }
            _sr.color = tempColor;
        }
    }

    private void SetDamage(GameObject _Enemy)
    {
        if (InvincibleTimer > 0.0f)
        {
            return;//���G���Ԓ��͏������s�킸�I��
        }

        if (playerStatus == PlayerStatus.Fine)
        {
            HP -= 1.0f;//�̗͂����炷
            UIManager.Instance.SetHP_UI((int)HP);

            //�ǂ���̕�������Ԃ����������ׂ�
            float Way = _Enemy.transform.position.x - transform.position.x;

            //�����ɂ��킹�Đ�����΂�
            if (_rb)
            {
                Vector2 KnockBackVelocity = Vector2.zero;
                if (Way > 0)//����
                {
                    Debug.Log("�E����Ԃ���܂���");
                    KnockBackVelocity += new Vector2(-KnockBackValue, 0.0f);
                }
                else if (Way < 0)//�E��
                {
                    Debug.Log("������Ԃ���܂���");
                    KnockBackVelocity += new Vector2(KnockBackValue, 0.0f);
                }
                KnockBackVelocity += new Vector2(0.0f, JumpValue);//������̗͂���

                _rb.velocity = KnockBackVelocity;
            }

            //�^�C�}�[�Z�b�g
            KnockBackTimer = KonokBackTime;//�m�b�N�o�b�N����
            InvincibleTimer = KnockBackTimer;//���G����

            playerStatus = PlayerStatus.HitDamage;//�X�e�[�^�X�Z�b�g

            //�}�l�[�W���[�ɃJ�����U���˗�
            CameraManager.SetShakeCamera();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spear")) return;

        if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }
    }
}
