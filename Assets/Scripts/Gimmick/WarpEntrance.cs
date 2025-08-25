using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpEntrance : MonoBehaviour
{
    [SerializeField]
    private Vector2 ExitPos;//�o���̍��W
    [SerializeField]
    private GameObject ExitObject;//�o���̃I�u�W�F�N�g(�I�u�W�F�N�g���ݒ肳��Ă����炻����̍��W��D��)

    [SerializeField]
    private string NextSceneName;//���̃V�[���̖��O

    [SerializeField]
    private bool IsDoorLock = false;//�h�A���b�N�t���O
    [SerializeField]
    private Sprite Open;//�J�����̃X�v���C�g
    [SerializeField]
    private Sprite Close;//�����̃X�v���C�g

    [SerializeField]
    GameObject Camera;
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Vector2 FadeOutPlayerPos;//�t�F�[�h�A�E�g���̃v���C���[���W���L�^

    private float TransferStartTimer = 0.0f;

    [SerializeField]
    private SwitchButton[] SwitchList;//�o�^���ꂽ�X�C�b�`���S�ăI���ł���Δ����J����(�t��������)


    void Start()
    {
        //�J�����擾
        Camera = GameObject.Find("Main Camera");

        //�o�����W�擾
        if (ExitObject)
        {
            ExitPos = ExitObject.transform.position;
        }

        Sprite_Update();//�X�v���C�g�X�V
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchList.Length > 0)//�X�C�b�`���o�^����Ă���Ƃ��̂�
        {
            bool IsDoorLock_old = IsDoorLock;//�ύX�O�̏�Ԃ��L�^

            IsDoorLock = false;//������
            //�X�C�b�`�̂ǂꂩ�ЂƂł��I�t�ł���Ύ{������
            for (int i = 0; i < SwitchList.Length; ++i)
            {
                if (!SwitchList[i].GetSwitchFlag()) { IsDoorLock = true; }
            }

            //�J�����ꂽ��J������U��������
            if (IsDoorLock != IsDoorLock_old && IsDoorLock == false)//�J�����ꂽ�u�Ԃ����������
            {
                const float Length = 0.05f;
                const float Speed = 10.0f;
                const float LowValue = 10.0f;
                CameraManager.SetShakeCamera(Length, Speed, LowValue);
            }
        }


        Sprite_Update();//�X�v���C�g�X�V

        //�v���C���[���o�^����Ă�����ړ�����
        if (Player)
        {
            if (Input.GetKeyDown(KeyCode.W) && !IsDoorLock)//�ړ����͂��{������Ă��Ȃ��Ƃ�
            {
                //�^�C�}�[�Z�b�g
                TransferStartTimer = 1.0f;

                //�t�F�[�h�A�E�g������
                if (Camera)
                {
                    UIManager _uiMng = Camera.GetComponent<UIManager>();
                    if (_uiMng)
                    {
                        _uiMng.SetBlindFade(true);
                    }
                }

                //���W���L�^
                if (Player)
                {
                    FadeOutPlayerPos = Player.transform.position;
                }
            }
        }

        //�^�C�}�[�X�V�O�Ɏ��Ԃ��L�^���Ă���
        float StartTimer_old = TransferStartTimer;

        //�^�C�}�[�X�V
        TransferStartTimer = Mathf.Max(0.0f, TransferStartTimer - Time.deltaTime);

        //�^�C�}�[�p�����Ƀv���C���[�̍��W���Œ�
        if (TransferStartTimer > 0)
        {
            if (Player)
            {
                Player.transform.position = new Vector2(FadeOutPlayerPos.x, Player.transform.position.y);
            }
        }

        //�^�C�}�[��0�ɂȂ����u�Ԃœ]�������s
        if (StartTimer_old > 0.0f && TransferStartTimer <= 0.0f)
        {
            TransferPlayer();
        }
    }

    private void TransferPlayer()
    {
        if(!Player)
        {
            return;//�v���C���[���ݒ肳��Ă��Ȃ��̂ŏI��
        }

        //Debug.Log("�ړ�������");

        // �ړ���̃V�[�����ݒ肳��Ă��Ȃ��Ȃ���W���Z�b�g���邾��
        if (NextSceneName == "")
        {
            //���W�Z�b�g
            Vector3 pos = ExitPos;
            pos.z = Player.transform.position.z;
            Player.transform.position = pos;    //�v���C���[��
            GameManager_01.SetStartPlayerPos(pos); //�Q�[���}�l�[�W���[�� ���L�^��͕ύX���邩������܂���

            //�����x������
            Rigidbody2D _rb = Player.GetComponent<Rigidbody2D>();
            if (_rb)
            {
                _rb.velocity = Vector3.zero;
            }


            //�J�����̍��W
            pos = ExitPos;
            GameManager_01.SetCameraGazePos(pos);

            //�t�F�[�h�A�E�g������
            GameManager_01.SetBlindFade(false);
        }
        else
        {
            //���W�Z�b�g
            GameManager_01.SetStartPlayerPos(ExitPos);

            //�V�[���ǂݍ���
            SceneManager.LoadScene(NextSceneName);
        }

        //�v���C���[�̓o�^����
        Player = null;
    }
    public void SetDoorLock(bool _flag)
    {
        IsDoorLock = _flag;
        Sprite_Update();//�X�v���C�g�X�V
    }
    //�X�v���C�g�̍X�V
    private void Sprite_Update()
    {
        //�X�v���C�g�X�V
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        if (_sr)
        {
            if (IsDoorLock)
            {
                if (Close) { _sr.sprite = Close; }
            }
            else
            {
                if (Open) { _sr.sprite = Open; }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[��o�^
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�v���C���[�̓o�^����
        if (collision.gameObject == Player)
        {
            //�ړ��������łȂ��Ƃ�
            if (TransferStartTimer <= 0.0f)
            {
                Player = null;
            }
        }
    }
}
