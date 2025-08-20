using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraManager
{
    private static GameObject Camera;
    private static CameraControl _cameraControl;

    public static void SetCamera(GameObject _Camera)
    {
        Camera = _Camera;

        //コンポーネントの取得
        if (Camera != null)
        {
            _cameraControl = Camera.GetComponent<CameraControl>();
        }
    }
    public static GameObject GetCamera()
    {
        return Camera;
    }
    public static void SetCameraArea(GameObject _area)
    {
        if (Camera != null)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                _cc.SetCameraArea(_area);
            }
            else Debug.Log("カメラコントロールのコンポーネントがありません");
        }
        else Debug.Log("カメラが設定されていません");
    }
    public static GameObject GetCameraArea()
    {
        if (Camera != null)
        {
            CameraControl _cc = Camera.GetComponent<CameraControl>();
            if (_cc)
            {
                return _cc.GetCameraArea();
            }
            else Debug.Log("カメラコントロールのコンポーネントがありません");
        }
        else Debug.Log("カメラが設定されていません");

        return null;
    }

    public static void SetShakeCamera()
    {
        if (_cameraControl != null)
        {
            _cameraControl.SetShakeCamera();
        }
    }
}
