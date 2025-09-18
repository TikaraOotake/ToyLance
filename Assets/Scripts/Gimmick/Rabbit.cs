using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private int Sequence = 0;//�i�K
    [SerializeField] private GameObject[] SequencePointObj;//�i�K���Ƃ̍��W������
    private List<GameObject> SequencePointObj_copy;

    bool IsIdle;


    [SerializeField] private GameObject Player;//Player

    [SerializeField]
    private float BoardShiftHeight;//�摜�����炷����
    private float BoardShiftHeight_old;

    [SerializeField] private float JumpValue;//�W�����v��

    [SerializeField] private float MigrationProgress;//�i�s�x�@0:�n�@1:�I

    [SerializeField] private float ProgressSpeed = 0.5f;//�i�s���x

    [SerializeField] private Vector2 DeparturePos;//�o�����W
    [SerializeField] private Vector2 TargetPos;//�ڕW���W

    [SerializeField] private float FindLength = 1.0f;//�v���C���[�������鋗��

    [SerializeField] private float IdleTime = 1.0f;//�ҋ@����
    [SerializeField] private float IdleTimer;//�ҋ@�^�C�}�[

    [SerializeField] 
    private GameObject SpriteBoard;//�摜��\������I�u�W�F�N�g
    private Animator _anim;//�A�j���[�^�[

    private SEManager _seManager;
    private Renderer _renderer;

    void Start()
    {
        Player = GameManager_01.GetPlayer();//�v���C���[�擾
        if (SpriteBoard != null)
        {
            _anim = SpriteBoard.GetComponent<Animator>();//�A�j���[�^�[�擾
        }


        DeparturePos = transform.position;//�o�����W�ݒ�
        TargetPos = DeparturePos;//�O�̂��߉��㏑��

        if (Sequence < SequencePointObj.Length)//�z��O�`�F�b�N
        {
            GameObject TargetObj = SequencePointObj[Sequence];
            if (TargetObj != null)
            {
                TargetPos = TargetObj.transform.position;//�ڕW���W�ݒ�
            }
        }

        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SE�̎擾�Ɏ��s");

        _renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HoppingSprite();//�X�v���C�g���㉺�ɂ��炵�Ē��˂�����

        //�i�s�x�𖞗�������Đݒ�
        if (MigrationProgress >= 1.0f)
        {
            MigrationProgress = 0.0f;//�i���x���Z�b�g
            IdleTimer = IdleTime;//�ҋ@�^�C�}�[�Z�b�g
            FacingPlayerSprite();//�v���C���[�̕���������
        }

        if (IdleTimer <= 0.0f)//�ҋ@���łȂ��ꍇ
        {
            //�s��ݒ�
            if (MigrationProgress == 0.0f)
            {
                DeparturePos = transform.position;//�o�����W�ݒ�

                if (CheckPlayerOverlap())
                {
                    Debug.Log("�v���C���[���ߕt���܂���");

                    ++Sequence;//���̒i�K��


                    TargetPos = DeparturePos;//�O�̂��߉��㏑��

                    if (Sequence < SequencePointObj.Length)//�z��O�`�F�b�N
                    {
                        GameObject TargetObj = SequencePointObj[Sequence];
                        if (TargetObj != null)
                        {
                            TargetPos = TargetObj.transform.position;//�ڕW���W�ݒ�
                        }
                    }

                    //�X�v���C�g�̌��������킹��
                    if (TargetPos != DeparturePos)//�ړ��悪�ݒ肳��Ă����ꍇ
                    {
                        FacingMoveWaySprite();
                    }

                }
            }

            if (MigrationProgress == 0.0f && IsVisible())
            {
                //�ړ���
                _seManager.PlaySE("rabbit");
            }


            //�i�s�x�ɍ��킹�ăE�T�M���ړ�
            Vector2 TargetVec = TargetPos - DeparturePos;//�ڕW�܂ł̃x�N�g�����Z�o
            transform.position = TargetVec * MigrationProgress + DeparturePos;
            
            //�i�s�x�X�V
            MigrationProgress = Mathf.Min(1.0f, MigrationProgress + ProgressSpeed * Time.deltaTime);
        }

        //�v���C���[�����͈͓��ɂ���@&�@���̍s�悪���܂��Ă���ꍇ
        if (CheckPlayerOverlap() && TargetPos != DeparturePos)
        {
            IdleTimer = 0.0f;//�ҋ@���Ԃ𓥂ݓ|��
        }

        //�ҋ@�^�C�}�[�X�V
        IdleTimer = Mathf.Max(0.0f, IdleTimer - Time.deltaTime);

        SetAnim();//�A�j���[�V�����Z�b�g
    }
    private void HoppingSprite()
    {
        //�i�s�x����摜�����炷�ʂ��v�Z
        BoardShiftHeight_old = BoardShiftHeight;//�O�̒l���L�^
        BoardShiftHeight = Mathf.Sin(MigrationProgress * Mathf.PI);

        //�摜�����炷
        if (SpriteBoard != null)
        {
            SpriteBoard.transform.position = new Vector3(0.0f, BoardShiftHeight, 0.0f) + transform.position;
        }
    }
    private void FacingPlayerSprite()//�v���C���[�̕���������
    {
        if (SpriteBoard == null) return;//�X�v���C�g�{�[�h���Ȃ����ߏI��

        //�v���C���[�̕����ɍ��킹��
        if (Player != null)
        {
            if (Player.transform.position.x > transform.position.x)
            {
                SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 180, 0.0f);
            }
            else
            {
                SpriteBoard.transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private void FacingMoveWaySprite()//�ړ�����������
    {
        if (SpriteBoard == null) return;//�X�v���C�g�{�[�h���Ȃ����ߏI��

        if (TargetPos.x > DeparturePos.x)
        {
            SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 180, 0.0f);
        }
        else
        {
            SpriteBoard.transform.eulerAngles = Vector3.zero;
        }
    }
    private void SetAnim()
    {
        if (_anim == null) return;
        //�t���O����U���Z�b�g
        _anim.SetBool("IsIdle", false);
        _anim.SetBool("IsHopping_Rising", false);
        _anim.SetBool("IsHopping_Floating", false);
        _anim.SetBool("IsHopping_Falling", false);

        if (IdleTimer > 0.0f)
        {
            _anim.SetBool("IsIdle", true);
        }
        else if (BoardShiftHeight >= 0.8f)
        {
            _anim.SetBool("IsHopping_Floating", true);
        }
        else if(BoardShiftHeight >= BoardShiftHeight_old)
        {
            _anim.SetBool("IsHopping_Rising", true);
        }
        else
        {
            _anim.SetBool("IsHopping_Falling", true);
        }
    }

    private bool CheckPlayerOverlap()//�v���C���[�����͈͓��ɓ��������m�F
    {
        if (Player != null)
        {
            Vector2 PlayerPos = Player.transform.position;
            Vector2 ThisPos = transform.position;
            Vector2 Length = PlayerPos - ThisPos;//�������Z�o
            if (FindLength >= Length.magnitude)
            {
                return true;
            }
        }

        return false;
    }

    private void OnValidate()
    {
        //�ŏ��̒i�K���W�͎��g�̍��W�ɂ���

        //�z��O�`�F�b�N
        if (0 < SequencePointObj.Length)
        {
            if (SequencePointObj[0] != null) return;

            SequencePointObj[0] = new GameObject();
            SequencePointObj[0].transform.position = transform.position;
            SequencePointObj[0].name = "TargetPoint";
        }
    }

    private bool IsVisible()
    {
        return _renderer.isVisible;
    }
}
