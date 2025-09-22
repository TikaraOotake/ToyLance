using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_01:MonoBehaviour
{
    private static GameObject Player;
    private static GameObject Camera;
    private static Vector2 StartPlayerPos;
    private static bool SettingPlayerPosFlag = false;

    private static GameObject CheckPoint;

    private static string[] TitleSceneName = new string[5];


    // �V���O���g���̃C���X�^���X
    private static HitStopManager _instance;

	// �C���X�^���X�̎擾
	public static HitStopManager Instance
	{
		get
		{
			if (_instance == null)
			{
				// �V�[������HitStopManager�I�u�W�F�N�g���Ȃ��ꍇ�A�V�����쐬����
				_instance = FindObjectOfType<HitStopManager>();

				if (_instance == null)
				{
					GameObject hitStopObject = new GameObject("HitStopManager");
					_instance = hitStopObject.AddComponent<HitStopManager>();
				}
			}
			return _instance;
		}
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
            Debug.Log("�Q�[���}�l�[�W���[�e�X�g");
		}

        if (Input.GetKeyDown(KeyCode.F1))
        {
            //�V�[���ǂݍ���
            LoadScene(TitleSceneName[0]);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            RespawnPlayer();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            //�V�[���ǂݍ���
            LoadScene(TitleSceneName[1]);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            //�V�[���ǂݍ���
            LoadScene(TitleSceneName[2]);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //�V�[���ǂݍ���
            LoadScene(TitleSceneName[3]);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            //�V�[���ǂݍ���
            LoadScene(TitleSceneName[4]);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
		{
			// �G�f�B�^�̏ꍇ�͍Đ���~
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            // �r���h���s���̓A�v���I��
            Application.Quit();
#endif
			Debug.Log("Game Exit triggered by ESC key");
		}
	}

	// �Q�[���J�n���ɏ��������郁�\�b�h
	public static void Initialize()
    {
        // Camera���擾
        Camera = GameObject.Find("Main Camera");

        CheckPoint = null;//�`�F�b�N�|�C���g������

        Debug.Log("�Q�[���}�l�[�W���[�̏�����");
        Debug.Log(SettingPlayerPosFlag);
        if (SettingPlayerPosFlag == true)
        {
            // �����ʒu�ݒ�i��: Player�����������ꍇ�j
            if (Player)
            {
                Player.transform.position = StartPlayerPos;
            }
            else
            {
                Debug.Log("�v���C���[������܂���");
            }
            if (Camera)
            {
                Camera.transform.position = new Vector3(StartPlayerPos.x, StartPlayerPos.y, Camera.transform.position.z);
            }
            else
            {
                Debug.Log("�J����������܂���");
            }
        }
        else
        {
            if (Player != null)
            {
                StartPlayerPos = Player.transform.position;
            }
        }

        //�t���O��������
        SettingPlayerPosFlag = false;
    }

    public static GameObject GetPlayer()
    {
        return Player;
    }

    public static void SetPlayer(GameObject _player)
    {
        Player = _player;
    }
    public static GameObject GetCamera()
    {
        return Camera;
    }
    public static void SetCamera(GameObject _camera)
    {
        Camera = _camera;
    }
    public static void SetCamera()
    {
        if (Camera != null) return;
        Camera = GameObject.Find("Main Camera");
    }

    //�J�����̒����_�̍��W��ݒ�
    public static void SetCameraGazePos(Vector2 _pos)
    {
        if (Camera != null)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                _cc.SetCameraGazePos(_pos);
            }
        }
    }
    public static void ResetCameraPos()
    {
        if (Camera != null)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                _cc.ResetCameraPos();
            }
        }
    }

    public static void SetBlindFade(bool _flag)
    {
        if (Camera != null)
        {
            UIManager _UIManager = Camera.GetComponent<UIManager>();
            if (_UIManager)
            {
                _UIManager.SetBlindFade(_flag);
            }
        }
    }

    public static Vector2 GetStartPlayerPos()
    {
        return StartPlayerPos;
    }

    public static void SetStartPlayerPos(Vector2 _StartPlayerPos)
    {
        Debug.Log("�v���C���[�̏������W�ݒ�");
        SettingPlayerPosFlag = true;
        StartPlayerPos = _StartPlayerPos;
    }

    public static void SetShakeCamera()
    {
        if (Camera)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                _cc.SetShakeCamera();
            }
        }
    }
    public static void SetShakeCamera(float _Length, float _Speed, float _LowValue)
    {
        if (Camera)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                _cc.SetShakeCamera(_Length, _Speed, _LowValue);
            }
        }
    }
    public static void SetCheckPoint(GameObject _CheckPoint)
    {
        if (CheckPoint != null)
        {
            CheckPoint _cp = CheckPoint.GetComponent<CheckPoint>();
            if (_cp)
            {
                _cp.SetActive(false);//���ɂ��������̂̓I�t��Ԃ�
            }
        }

        CheckPoint = _CheckPoint;
    }
    public static void RespawnPlayer()
    {
        if (Player == null) return; //Player���ݒ肳��Ă��Ȃ�������I��

        Vector2 RespawnPoint = StartPlayerPos;
        if (CheckPoint != null)
        {
            RespawnPoint = CheckPoint.transform.position;
        }

        Player_01_Control _player = Player.GetComponent<Player_01_Control>();
        if (_player != null)
        {
            _player.RespawnPlayer(RespawnPoint);
            CameraManager.SetCameraGazePos(RespawnPoint);
        }
    }
    public static void SetGameover()
    {

    }
    public static void SetHP_UI(int _hp)
    {
        SetCamera();//�O�̂���

        if (Camera == null) return;

        UIManager _UIManager_cs = Camera.GetComponent<UIManager>();//UI�}�l�[�W���[�擾
        if (_UIManager_cs == null) return;

        _UIManager_cs.SetHP_UI(_hp);
    }
    public static void LoadScene(string _SceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(_SceneName))
        {
            SceneManager.LoadScene(_SceneName);
        }
        else
        {
            Debug.LogError($"�V�[���� '{_SceneName}' �͑��݂��܂���B");
        }
    }

    public static void CollGameOver()
    {
        if (Camera != null)
        {
            UIManager UI_mng = Camera.GetComponent<UIManager>();
            if (UI_mng)
            {
                UI_mng.CallGameOver();
            }
        }
    }
    public static void SetPlayerIsPause(bool _flag)
    {
        if (Player != null)
        {
            Player_01_Control player = Player.GetComponent<Player_01_Control>();
            if (player)
            {
                player.SetIsPause(_flag);
            }
        }
    }

    public static void SetTitleSceneName(string[] _name)
    {
        if (_name.Length == 5)
        {
            TitleSceneName = _name;
        }
        else
        {
            Debug.Log("�z��̃T�C�Y�Ⴄ");
        }
    }
}
