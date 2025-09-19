using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private Animator _anim;//�A�j���[�^�[

    void Start()
    {
        //�J�����擾
        Camera = GameObject.Find("Main Camera");

        //�o�����W�擾
        if (ExitObject)
        {
            ExitPos = ExitObject.transform.position;
        }

        //�A�j���[�^�[�擾
        _anim = GetComponent<Animator>();

        Sprite_Update();//�X�v���C�g�X�V
    }
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

        SetAnim();//�A�j���[�V�����̍X�V
        Sprite_Update();//�X�v���C�g�X�V

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
        if (!Player)
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
            //GameManager_01.SetCameraGazePos(pos);
            StartCoroutine(CollResetCameraPos());

            //�v���C���[�Ɉړ��I����`����
            Player_01_Control playerControl=Player.GetComponent<Player_01_Control>();
            if(playerControl!=null)
            {
                playerControl.DoorExit();
            }
        }
        else
        {
            //���W�Z�b�g
            GameManager_01.SetStartPlayerPos(ExitPos);

            //�V�[���ǂݍ���
            GameManager_01.LoadScene(NextSceneName);
        }

        //�v���C���[�̓o�^����
        Player = null;
    }

    IEnumerator CollResetCameraPos()
    {
        yield return null;
        yield return null;

        //�t�F�[�h�C��������
        GameManager_01.SetBlindFade(false);

        //�J�����̍��W�����Z�b�g
        GameManager_01.ResetCameraPos();
    }
    public void SetDoorLock(bool _flag)
    {
        IsDoorLock = _flag;
        Sprite_Update();//�X�v���C�g�X�V
    }

    private void SetAnim()
    {
        if (_anim)
        {
            _anim.SetBool("IsDoorLock", IsDoorLock);
        }
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
    public bool TeleportSetting(GameObject _Player)
    {
        if (_Player == null) return false;//�����ȃv���C���[�ł���ΏI��
        if (IsDoorLock == true) return false;//�{�����̃h�A�̈׏I��

        Player = _Player;

        //�^�C�}�[�Z�b�g
        TransferStartTimer = 0.4f;

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

        return true;//�����Ƃ��ď���
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
