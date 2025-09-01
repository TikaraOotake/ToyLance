using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager_01
{
    private static GameObject Player;
    private static GameObject Camera;
    private static Vector2 StartPlayerPos;
    private static bool SettingPlayerPosFlag = false;

    private static Vector2 CheckPointPos;

    // �Q�[���J�n���ɏ��������郁�\�b�h
    public static void Initialize()
    {
        // Player���擾
        Player = GameObject.Find("Player01");
        // Camera���擾
        Camera = GameObject.Find("Main Camera");

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

            //���A�n�_������
            CheckPointPos = StartPlayerPos;
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
    public static void SetCheckPointPos(Vector2 _Pos)
    {
        CheckPointPos = _Pos;
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
}
