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

    [SerializeField]
    private float ReceptionCoolTime = 0.0f;//�l�̕ύX����w�肵�����Ԃ͕ύX���󂯕t���Ȃ�
    private float ReceptionCoolTimer;//�v���p

    void Start()
    {
        //�F�X�V
        Color_Update();
    }

    // Update is called once per frame
    void Update()
    {
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

            //�F�X�V
            Color_Update();

            //�N�[���^�C���Z�b�g
            ReceptionCoolTimer = ReceptionCoolTime;
        }



        //�l�X�V
        SwitchFlag_old = SwitchFlag;
        if (ReceptionCoolTimer > 0.0f)
        {
            ReceptionCoolTimer -= Time.deltaTime;
        }
        else if (ReceptionCoolTimer < 0.0f)
        {
            ReceptionCoolTimer = 0.0f;
        }
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ReceptionCoolTimer > 0)
        {
            return;//�N�[���^�C�����ł���ΏI��
        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//�v���C���[�ł��n�`�ł��Ȃ���ΏI��
        }

        if (mode == SwitchMode.Toggle)
        {
            SwitchFlag = !SwitchFlag;
        }
        else if (mode == SwitchMode.Lock)
        {
            SwitchFlag = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ReceptionCoolTimer > 0)
        {
            return;//�N�[���^�C�����ł���ΏI��
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//�v���C���[�ł��n�`�ł��Ȃ���ΏI��
        }

        //�{�^�������������(��\��)��
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            Color color = _This_sr.color;
            color.a = 0.0f;//��\��

            _This_sr.color = color;//���
        }

        //�t���O��ύX
        if (mode == SwitchMode.Momentary)
        {
            SwitchFlag = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//�v���C���[�ł��n�`�ł��Ȃ���ΏI��
        }

        //�{�^���𗣂������(�\��)��
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            Color color = _This_sr.color;
            color.a = 1.0f;//�\��

            _This_sr.color = color;//���
        }

        if (mode == SwitchMode.Momentary)
        {
            SwitchFlag = false;
        }
    }
}
