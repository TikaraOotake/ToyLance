using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;
using UnityEngine.SceneManagement;
using PlayerStatusEnum;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private GameObject LancePrefab;//�������̃v���n�u
    [SerializeField]
    private List<GameObject> LanceList = new List<GameObject>();//�������������X�g�ŋL�^
    [SerializeField]
    private int LanceMaxNam = 1;//���̍ő�l
    [SerializeField]
    private GameObject AttackPrefab;//�ߋ����F�����U���@�p�̃R���C�_�[�v���n�u
    [SerializeField]
    private GameObject AttackObj;//

    [SerializeField] private WarpEntrance Door;//�h�A���i�[

    [SerializeField] private GameObject FlipObj;//Player�̌����ɍ��킹�Ĕ��]����q�I�u�W�F�N�g

    [SerializeField]
    private bool HaveLance = true;
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

    [SerializeField] private bool IsPause;//�|�[�Y�t���O

    //�A�j���[�V�����ϐ�
    [SerializeField] private bool IsJump = false;
    [SerializeField] private bool IsMove = false;
    [SerializeField] private bool IsThrustAtk = false;
    [SerializeField] private bool IsThrowAtk = false;
    [SerializeField] private bool IsFallAtk = false;
    [SerializeField] private bool IsDoorEnter = false;
    [SerializeField] private float IsThrowTimer = 0.0f;
    [SerializeField] private bool IsWallGrab = false;
    [SerializeField] private bool IsWallGrab_jamp = false;

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
    private float TrustAtkTimer = 0.7f;//�˂��U������

    private float KeyboardInputTimer;
    private bool IsInputDown = false;//�����͏����L�^����
    private bool IsInputDown_old = false;
    private Vector2 StickInputValue_old;//�E�X�e�B�b�N�̓��͒l���L�^

    private float AttackMoveValue_TrustBase = 10.0f;//�U�����̊�b�ړ���
    private float AttackMoveValue;//�U�����̈ړ���
    private float AttackMoveValueDecayRate = 30.0f;//�b�Ԃ̌�����

    [SerializeField] private int TrustAttackValue;//�h�ˍU����
    [SerializeField] private int ThrowAttackValue;//�����U����
    [SerializeField] private int FallAttackValue;//�����U����

    [SerializeField] private float TestRot;

    PlayerStatus playerStatus;

    private SEManager _seManager;
    [SerializeField]
    private float footstepInterval = 0.5f;      //�炷�Ԋu
    private float footstepTimer = 0.0f;         //�炷�^�C�}�[

    enum AtkStatus
    {
        None,//����
        Thrust,//�˂�
        Throw,//����
        Falling,//����
        WallGrab,//�Ǔ˂��������
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

        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SE�̎擾�Ɏ��s");

        HP = HP_Max;
    }

    void Start()
    {
        playerStatus = PlayerStatus.Fine;//�X�e�[�^�X��ʏ��
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPause == true) return;//�|�[�Y�͉��������I��
        if (Time.timeScale == 0.0f) return;//���Ԓ�~���͉������Ȃ�

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.LeftArrow))
        {
            KeyboardInputTimer = 1.0f;
        }

        if (playerStatus == PlayerStatus.Fine)
        {

            if (atkStatus == AtkStatus.None)
            {
                //��{����
                Move();
                Jump();
                Throw();
                DoorEnter();

                InputAtkSetting();
            }
            else
            {
                FallingAtk();
                TrustAtk();

                WallGrab();
            }


        }
        else if (playerStatus == PlayerStatus.Dead)//���S���
        {
            if (_rb != null)
            {
                _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            }
            //GameManager_01.RespawnPlayer();//�Q�[���I�[�o�[��ʂ��ł���܂ł͂����ŏ�������
        }


        if (_rb)
        {
            //�������x�𐧌�
            if (_rb.velocity.y < -JumpValue * 2.0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -JumpValue * 2.0f);
            }
        }

        //��e����
        GameObject Enemy = GetTouchingObjectWithLayer(GetComponent<Collider2D>(), "Enemy");
        if (Enemy != null)
        {
            SetDamage(Enemy);
        }

        SetAnim();

        CheckLanding_Update();

        GameManager_01.SetHP_UI((int)HP);

        Timer_Update();//�^�C�}�[�X�V

        //���͂��L�^
        IsInputDown_old = IsInputDown;
        IsInputDown = Input.GetAxis("Vertical") < -0.1f;
        StickInputValue_old.x = Input.GetAxis("Horizontal");//����
        StickInputValue_old.y = Input.GetAxis("Vertical");//�c��

        CleanLanceList();//�j���ς̑��̓��X�g����O��

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
                GameManager_01.CollGameOver();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_rb)
        {
            transform.Translate(_rb.velocity * 0.1f * Time.deltaTime);//�˂�������h�~�̂��ߏ����߂荞��
        }
    }

    private void Timer_Update()
    {
        //�^�C�}�[�X�V
        AtkTimer = Mathf.Max(0.0f, AtkTimer - Time.deltaTime);
        InvincibleTimer = Mathf.Max(0.0f, InvincibleTimer - Time.deltaTime);
        BlinkingTimer = Mathf.Max(0.0f, BlinkingTimer - Time.deltaTime);
        KnockBackTimer_old = KnockBackTimer;
        KnockBackTimer = Mathf.Max(0.0f, KnockBackTimer - Time.deltaTime);
        ThrowCooltimer = Mathf.Max(0.0f, ThrowCooltimer - Time.deltaTime);
        KeyboardInputTimer = Mathf.Max(0.0f, KeyboardInputTimer - Time.deltaTime);
        IsThrowTimer = Mathf.Max(0.0f, IsThrowTimer - Time.deltaTime);
    }
    private void InputAtkSetting()
    {
        if (!HaveLance) return;//���������Ă��Ȃ��׏I��
        if (LanceList.Count >= LanceMaxNam) return;//�o���鑄�̍ő�l�𒴂��Ă��邽�ߏI��
        if (AtkTimer > 0.0f) return;//�U�����͏��������Ȃ�

        if (Input.GetKeyDown(KeyCode.S) || (IsInputDown != IsInputDown_old && IsInputDown) && KeyboardInputTimer <= 0.0f)
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
            AtkTimer = TrustAtkTimer;
            //�h�ˍU����
            _seManager.PlaySE("trust");

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                KeyboardInputTimer = 1.0f;
            }

            if (Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.A) ||
                (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && KeyboardInputTimer <= 0.0f))
            {
                AttackMoveValue = AttackMoveValue_TrustBase;//�ړ��ʃZ�b�g
            }
            else
            {
                AttackMoveValue = 0.0f;//�ړ�����
            }

            return;
        }
    }
    private void Move()
    {
        float MoveWay = 0.0f;
        bool isMoving = false;
        if (Input.GetKey(KeyCode.D) || (Input.GetAxis("Horizontal") > 0.1f && KeyboardInputTimer <= 0.0f))
        {
            MoveWay = +1.0f;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetAxis("Horizontal") < -0.1f && KeyboardInputTimer <= 0.0f))
        {
            MoveWay = -1.0f;
            isMoving = true;
        }

        //�ړ������ڒn��
        if (isMoving && IsLanding)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0.0f)
            {
                //����
                _seManager.PlaySE("footsteps");
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0.0f;
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
                FlipX = true;
            }
            else if (MoveWay < 0.0f)//������
            {
                FlipX = false;
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

                //�W�����v
                _seManager.PlaySE("jump");
            }
        }
    }

    public void Throw()
    {
        if (!HaveLance) return;//���������Ă��Ȃ��׏I��

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Ranged"))
        {
            if (ThrowCooltimer > 0.0f) return;//�N�[���^�C�����ł���Ȃ�I��

            if (LancePrefab != null && LanceList.Count < LanceMaxNam)//�v���n�u���L�����@�ő�l�ɒB���Ă��Ȃ����@�`�F�b�N
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

                if (!FlipX)
                {
                    Rot.y += 180.0f;
                    ThrowVec.x *= -1;//���x�̌����𔽓]
                }
                GameObject Lance = Instantiate(LancePrefab, transform.position, Quaternion.identity);//���𐶐�
                Lance.transform.eulerAngles = Rot;

                Rigidbody2D _rb = Lance.GetComponent<Rigidbody2D>();//�����R���|�擾
                if (_rb != null) _rb.velocity = ThrowVec;//���x���

                IsThrowTimer = 0.8f;//�A�j���[�V�����^�C�}�[�Z�b�g

                ThrowCooltimer = ThrowCooltime;//�N�[���^�C�}�[�Z�b�g

                //�����X�g�ɑ��I�u�W�F�N�g��o�^
                LanceList.Add(Lance);

                //�����U����
                _seManager.PlaySE("throw");

                return;
            }
        }
    }
    private void FallingAtk()
    {
        if (atkStatus == AtkStatus.Falling)
        {
            //�I������
            if (AtkTimer <= 0.0f)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

                IsFallAtk = false;
                return;
            }

            if (AtkTimer <= 0.8f)
            {
                if (!IsLanding)
                {
                    if (_rb) _rb.velocity = new Vector2(0.0f, -JumpValue * 2.0f);//�󒆎��̂݋}�~��
                }

                InvincibleTimer = 0.1f;//���G���ԃZ�b�g(��u)

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
            if (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.1f && KeyboardInputTimer <= 0.0f)
            {
                if (AtkTimer <= 0.0f)//0�ȉ�
                {
                    AtkTimer = Time.deltaTime * 2.0f;
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
                if (_rb) _rb.velocity = new Vector2(0.0f, 0.0f);//�ړ����Ȃ�
            }
        }
    }
    private void TrustAtk()
    {
        if (atkStatus == AtkStatus.Thrust)
        {
            //�ړ������̎擾
            float Way = -1.0f;
            if (_sr)
            {
                if (_sr.flipX) Way *= -1.0f;
            }

            if (_rb) _rb.velocity = new Vector2(AttackMoveValue * Way, _rb.velocity.y);//�ړ�

            //�ړ��ʂ�����������
            AttackMoveValue = Mathf.Max(0.0f, AttackMoveValue - AttackMoveValueDecayRate * Time.deltaTime);

            if (AtkTimer <= TrustAtkTimer)
            {
                IsThrustAtk = true;//�h�ˍU��Anim
            }

            if (AtkTimer <= 0.9f)
            {
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

                    //�Ǔ˂��h������`�F�b�N
                    if (AtkTimer >= 0.5f)
                    {
                        if (IsLanding == false)
                        {
                            SpearAttack spearAttack = AttackObj.GetComponent<SpearAttack>();
                            if (spearAttack != null)
                            {
                                GameObject Ground = spearAttack.GetHitGround();
                                if (Ground != null)
                                {
                                    AtkTimer = 0.0f;
                                    atkStatus = AtkStatus.WallGrab;
                                    if (AttackObj != null) Destroy(AttackObj);//�U������̔j��
                                    if (_rb)
                                    {
                                        //������ړ��𖳌�
                                        _rb.velocity = Vector2.zero;
                                        _rb.bodyType = RigidbodyType2D.Kinematic;
                                    }

                                    IsThrustAtk = false;
                                }
                            }
                        }
                    }
                }
            }

            //�I������
            if (AtkTimer <= 0.0f && atkStatus != AtkStatus.WallGrab)
            {
                atkStatus = AtkStatus.None;

                if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

                IsThrustAtk = false;
            }
        }
    }
    private void WallGrab()//�Ǔ˂��h��
    {
        if (atkStatus == AtkStatus.WallGrab)
        {
            IsWallGrab = true;//�R�������



            //��ɃW�����v
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") > 0.4f && StickInputValue_old.y <= 0.4f && KeyboardInputTimer <= 0.0f) ||
                Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                //�ړ��ʎ擾
                Vector2 MoveVelocity = _rb.velocity;
                MoveVelocity.y = JumpValue * 1.2f;
                _rb.velocity = MoveVelocity;//���

                atkStatus = AtkStatus.None;//�ʏ�ɖ߂�
            }

            //�˂��h������
            if (Input.GetKeyDown(KeyCode.S) || (Input.GetAxis("Vertical") < -0.4f && StickInputValue_old.y >= -0.4f && KeyboardInputTimer <= 0.0f))
            {
                atkStatus = AtkStatus.None;//�ʏ�ɖ߂�
            }

            if (atkStatus == AtkStatus.None)
            {
                if (_rb)
                {
                    _rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }
    private void DoorEnter()
    {
        if (IsDoorEnter == true) return;//�h�A�i�����Ȃ�I��

        if (Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("Vertical") > 0.4f && StickInputValue_old.y <= 0.4f && KeyboardInputTimer <= 0.0f))
        {
            if (Door != null)
            {
                bool result = false;//�������������L�^
                result = Door.TeleportSetting(this.gameObject);
                if (result == true)
                {
                    IsDoorEnter = true;//�h�A�i�����

                    //������̈ړ���}��
                    if (_rb)
                    {
                        if (_rb.velocity.y > 0.0f)
                        {
                            _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                        }
                    }
                }
            }
        }
    }
    public void DoorExit()
    {
        IsDoorEnter = false;//�h�A����o��
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
        _anim.SetBool("IsDoorEnter", false);
        _anim.SetBool("IsDead", false);
        _anim.SetBool("IsThrowAttack", false);
        _anim.SetBool("IsWallGrab", false);

        //�D��ȕ��قǏ�ɂȂ�ׂ�
        if (playerStatus == PlayerStatus.Dead)
        {
            _anim.SetBool("IsDead", true);
        }
        else if (IsDoorEnter)
        {
            _anim.SetBool("IsDoorEnter", true);
        }
        else if (IsThrowTimer > 0.0f)
        {
            _anim.SetBool("IsThrowAttack", true);
        }
        else if (KnockBackTimer > 0.0f)//��e���
        {
            _anim.SetBool("doDamaged", true);
        }
        else if (atkStatus == AtkStatus.WallGrab)
        {
            _anim.SetBool("IsWallGrab", true);
        }
        else if (IsThrowAtk)//����
        {

        }
        else if (IsThrustAtk)//�˂�
        {
            _anim.SetBool("isTrustAttack", true);
        }
        else if (IsFallAtk)//�����U��
        {
            _anim.SetBool("isDownAttacking", true);
        }
        else if (IsJump)//�W�����v
        {
            _anim.SetBool("isJumping", true);
        }
        else if (IsMove)//����
        {
            _anim.SetBool("isWalk", true);
        }

        //���̏����󋵂�`����
        _anim.SetBool("IsHaveLance", LanceList.Count < LanceMaxNam);

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

    public void RespawnPlayer(Vector2 _pos)
    {
        //���X�|�[��
        transform.position = _pos;//���W�ݒ�
        HP = HP_Max;//�̗͏�����
        playerStatus = PlayerStatus.Fine;
        if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

        if (_rb)
        {
            _rb.velocity = Vector2.zero;//���x������
        }
        if (_sr)
        {
            _sr.flipY = false;
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
            atkStatus = AtkStatus.None;//�U�����Z�b�g
            if (AttackObj != null) Destroy(AttackObj);//�U������̔j��

            //�A�j���[�V�����t���O���Z�b�g
            IsFallAtk = false;
            IsThrowAtk = false;
            IsThrustAtk = false;

            //�}�l�[�W���[�ɃJ�����U���˗�
            CameraManager.SetShakeCamera();

            _seManager.PlaySE("damage");
        }
    }

    public void SetLance(bool _flag)
    {
        HaveLance = _flag;
    }
    public void SetIsPause(bool _flag)
    {
        IsPause = _flag;
    }

    //����HP
    public void SetPlayerHP(float _HP)
    {
        HP = Mathf.Min(HP_Max, _HP);//����𒴂��Ȃ��悤�ɑ��
    }
    public float GetPlayerHP()
    {
        return HP;
    }

    //�ő�HP
    public void SetPlayerHP_Max(float _HP_Max)
    {
        HP_Max = _HP_Max;//
    }
    public float GetPlayerHP_Max()
    {
        return HP_Max;
    }

    //���̏�����
    public void SetLanceNum(int _lance)
    {
        LanceMaxNam = _lance;
    }
    public int GetLanceNum()
    {
        return LanceMaxNam;
    }

    private void CleanLanceList()//�j���ς̑��̓��X�g����O��
    {
        for (int i = LanceList.Count - 1; i >= 0; i--)
        {
            if (LanceList[i] == null) // Destroy�ς݂Ȃ�null���肪true�ɂȂ�
            {
                LanceList.RemoveAt(i);
            }
        }
    }
    public void HandoverLance(GameObject _old, GameObject _new)
    {
        if (_old == null || _new == null) return;

        int Index = LanceList.IndexOf(_old);//��v����v�f�̔ԍ����擾
        if (Index >= 0)
        {
            LanceList[Index] = _new;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spear")) return;

        if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }

        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance != null)
        {
            Door = warpEntrance;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance == Door)
        {
            Door = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance != null)
        {
            Door = warpEntrance;
        }
        if (collision.gameObject.tag == "Default")
        {
            Debug.Log(collision.gameObject.name);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            //SetDamage(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        WarpEntrance warpEntrance = collision.gameObject.GetComponent<WarpEntrance>();
        if (warpEntrance == Door)
        {
            Door = null;
        }
    }
}
