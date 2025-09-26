using UnityEngine;
using UnityEngine.UIElements;

public class SwitchButton : MonoBehaviour
{
    [SerializeField]
    private bool SwitchFlag;//�����񊈐����L�^
    private bool SwitchFlag_old;//�O�t���[���̃t���O��Ԃ��L�^

    [SerializeField]
    private bool ReversingResult;//���ʂ̔��]

	[SerializeField]
    SpriteRenderer _sr;//����̃X�v���C�g�����_���[
    [SerializeField]
    private GameObject Button;

    [SerializeField]
    private float PushSpeed = 1.0f;//�������ݑ��x

    Collider2D _coll;//�R���C�_�[

    enum SwitchMode
    {
        Momentary,//�ڐG���Ă��鎞����
        Toggle,//�ڐG���邽��
        Lock,//�����ɂȂ����炻�̏�Ԃ��ێ���������
    }
    [SerializeField]
    private SwitchButton[] RadioList;//�����ɂȂ����Ƃ��o�^�����X�C�b�`��S�Ĕ񊈐��ɂ���

    [SerializeField]
    private SwitchMode mode;

    [SerializeField] private float ReceptionCoolTime = 0.1f;//�l�̕ύX����w�肵�����Ԃ͕ύX���󂯕t���Ȃ�
    [SerializeField] private float ReceptionCoolTimer;//�v���p
    private float ReceptionCoolTimer_old;//�v���p

    [SerializeField] private Animator anim_Button;//���ۂɉ������{�^���̃A�j���[�V����
    [SerializeField] private Animator anim_Base;//����̃A�j���[�V����


    void Start()
    {
        //�F�X�V
        Color_Update();

        _coll = GetComponent<Collider2D>();//�R���C�_�[�擾
    }

    // Update is called once per frame
    void Update()
    {
        //�l�X�V
        SwitchFlag_old = SwitchFlag;
        ReceptionCoolTimer_old = ReceptionCoolTimer;
        ReceptionCoolTimer = Mathf.Max(0.0f, ReceptionCoolTimer - Time.deltaTime);
        CheckPushSwitch();//�X�C�b�`��������Ă��邩�`�F�b�N
        if (mode == SwitchMode.Momentary)//���[�����g���[���[�h
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                SwitchFlag = true;//�^�C�}�[�p�����̓I����Ԃ��p������
            }
            else
            {
                SwitchFlag = false;
            }
        }
        else if (mode == SwitchMode.Toggle)//�g�O�����[�h
        {
            if (ReceptionCoolTimer > 0.0f && ReceptionCoolTimer_old <= 0.0f)//�^�C�}�[��0����ς�����u��
            {
                SwitchFlag = !SwitchFlag;//�t���O���]
            }
        }
        else if (mode == SwitchMode.Lock)//���b�N���[�h
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                SwitchFlag = true;//�^�C�}�[�p�����̓I����Ԃ��p������
            }
        }

        //�t���O���ύX���ꂽ�ꍇ
        if (SwitchFlag != SwitchFlag_old)
        {
            //�I���̍ۃ��W�I���X�g�ɓo�^����Ă���X�C�b�`��S�ăI�t
            if (SwitchFlag == true)
            {
                for (int i = 0; i < RadioList.Length; ++i)
                {
                    RadioList[i].SetSwitchFlag(false);
                }
            }
        }

        if (ReceptionCoolTimer > 0.0f && ReceptionCoolTimer_old <= 0.0f)//�����ꂽ�u��
        {
            //SE���Đ�
            SEManager.instance.PlaySE("break");
        }

        //�F�X�V
        Color_Update();

    }
    public void SetSwitchFlag(bool _flag)
    {
        SwitchFlag = _flag;
        if (!_flag)
        {
            ReceptionCoolTimer = 0.0f;//�^�C�}�[��������
        }
    }
    public bool GetSwitchFlag()
    {
        return SwitchFlag_old != ReversingResult;
    }

    private void Color_Update()
    {
        if (anim_Base != null)
        {
            if (SwitchFlag != ReversingResult)
            {
                anim_Base.SetBool("IsActive", true);//�L��
            }
            else
            {
                anim_Base.SetBool("IsActive", false);//��L��
            }
        }

        if (anim_Button != null)
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                anim_Button.SetBool("IsActive", true);//������Ă�����
            }
            else
            {
                anim_Button.SetBool("IsActive", false);//������Ă��Ȃ����
            }
        }
        

    }

    private void CheckPushSwitch()
    {
        if (_coll != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(_coll, "Player") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "SpearPlatform"))
            {
                ReceptionCoolTimer = ReceptionCoolTime;//�^�C�}�[�Z�b�g
            }
        }
    }
}
