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

    // ゲーム開始時に初期化するメソッド
    public static void Initialize()
    {
        // Playerを取得
        Player = GameObject.Find("Player01");
        // Cameraを取得
        Camera = GameObject.Find("Main Camera");

        Debug.Log("ゲームマネージャーの初期化");
        Debug.Log(SettingPlayerPosFlag);
        if (SettingPlayerPosFlag == true)
        {
            // 初期位置設定（例: Playerが見つかった場合）
            if (Player)
            {
                Player.transform.position = StartPlayerPos;
            }
            else
            {
                Debug.Log("プレイヤーがありません");
            }
            if (Camera)
            {
                Camera.transform.position = new Vector3(StartPlayerPos.x, StartPlayerPos.y, Camera.transform.position.z);
            }
            else
            {
                Debug.Log("カメラがありません");
            }

            //復帰地点初期化
            CheckPointPos = StartPlayerPos;
        }

        //フラグを初期化
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

    //カメラの注視点の座標を設定
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
        Debug.Log("プレイヤーの初期座標設定");
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
        SetCamera();//念のため

        if (Camera == null) return;

        UIManager _UIManager_cs = Camera.GetComponent<UIManager>();//UIマネージャー取得
        if (_UIManager_cs == null) return;

        _UIManager_cs.SetHP_UI(_hp);
    }
}
