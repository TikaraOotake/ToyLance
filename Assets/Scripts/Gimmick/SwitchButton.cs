using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    [SerializeField]
    private bool SwitchFlag;//�����񊈐����L�^
    private bool SwitchFlag_old;//�O�t���[���̃t���O��Ԃ��L�^

    [SerializeField]
    private bool ReversingResult;//���ʂ̔��]

	[SerializeField]
    SpriteRenderer _sr;//����̃X�v���C�g�����_���[

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

        //�F�X�V
        Color_Update();

    }
    public void SetSwitchFlag(bool _flag)
    {
        SwitchFlag = _flag;
    }
    public bool GetSwitchFlag()
    {
        return SwitchFlag_old != ReversingResult;
    }

    private void Color_Update()
    {
        //�F��ύX
        if (_sr)
        {
            if (SwitchFlag != ReversingResult)
            {
                _sr.color = new Color(0.75f, 0.75f, 0.008f, 1.0f);//���F
            }
            else
            {
                _sr.color = new Color(0.5f, 0.0f, 0.0f, 1.0f);//�ԍ�
            }
        }

        //�{�^�������������(��\��)��
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                Color color = _This_sr.color;
                color.a = 0.0f;//��\��

                _This_sr.color = color;//���
            }
            else
            {
                Color color = _This_sr.color;
                color.a = 1.0f;//�\��

                _This_sr.color = color;//���
            }
        }
    }

    private void CheckPushSwitch()
    {
        if (_coll != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(_coll, "Player") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "Ground") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "SpearPlatform"))
            {
                ReceptionCoolTimer = ReceptionCoolTime;//�^�C�}�[�Z�b�g
            }
        }
    }
}
