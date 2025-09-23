using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject CameraArea;//カメラエリア

    [SerializeField]
    private float ChaseRate = 1.0f;

    [SerializeField]
    private float AdjustCameraPosX = 0.0f;
    [SerializeField]
    private int CameraWayX = 0;

    [SerializeField]
    private Vector2 CameraGazePos;//カメラの注視座標

    //振動に関わる変数
    private Vector2 ShakeRot_Vec;//振動ベクトル
    private float ShakeRot_Length;//半径
    [SerializeField]
    private float ShakeRot_Speed = 1.0f;//回転速度
    [SerializeField]
    private float ShakeRot_LowValue = 1.0f;//減衰量
    private float ShakeRot_Angle = 0.0f;//角度

    private Vector2 cameraShakeOffset = Vector2.zero;

    private void Awake()
    {
        

        //カメラManagerにカメラをセット
        CameraManager.SetCamera(this.gameObject);

    }
    void Start()
    {
        if (player == null) player = GameManager_01.GetPlayer();//プレイヤーを取得
        if (player == null) Debug.Log("プレイヤーが設定されていません");

        //カメラの初期座標をプレイヤーの位置に設定
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }

        //注視座標をセット
        CameraGazePos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            CameraWayX = 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CameraWayX = -1;
        }
    }


    public void SetShakeCamera()
    {
        const float Length = 0.2f;
        const float Speed = 10.0f;
        const float LowValue = 10.0f;
        SetShakeCamera(Length, Speed, LowValue);
    }
    public void SetShakeCamera(float _Length, float _Speed, float _LowValue)
    {
        ShakeRot_Length = _Length;   //半径
        ShakeRot_Speed = _Speed;     //回転速度
        ShakeRot_LowValue = _LowValue;   //減衰量
        ShakeRot_Angle = Random.Range(0f, 360f);    //角度はランダムにスタート
    }
    //カメラ振動の更新処理
    private void ShakeCamera_Update()
    {
        //振動が終了していれば何もしない
        if (ShakeRot_Length <= 0.01f)
        {
            ShakeRot_Vec = Vector2.zero;
            return;
        }

        //角度を加算して回転（毎フレーム）
        ShakeRot_Angle += ShakeRot_Speed * Time.unscaledDeltaTime * 360f; //1秒で1回転
        if (ShakeRot_Angle >= 360f)
        {
            ShakeRot_Angle -= 360f;
        }

        //円運動ベクトルを計算
        float rad = ShakeRot_Angle * Mathf.Deg2Rad;
        ShakeRot_Vec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * ShakeRot_Length;

        //減衰させる
        ShakeRot_Length = Mathf.Lerp(ShakeRot_Length, 0f, ShakeRot_LowValue * Time.unscaledDeltaTime);

        //カメラの位置に振動を加える
        cameraShakeOffset = (Vector3)ShakeRot_Vec;
    }

    public void StartShake(float length, float speed, float lowValue)
    {
        ShakeRot_Length = length;   //半径
        ShakeRot_Speed = speed;     //回転速度
        ShakeRot_LowValue = lowValue;   //減衰量
        ShakeRot_Angle = Random.Range(0f, 360f);    //角度はランダムにスタート
    }

    void FixedUpdate()
    {
        ShakeCamera_Update();

        //目標座標をセット
        Vector2 TargetPos =  GetTargetPos();

        //注視点の座標計算
        Vector2 pos = CameraGazePos;
        float rate = 1.0f - Mathf.Pow(1.0f - ChaseRate, Time.deltaTime * 60f); // 秒間ChaseRateになるように
        pos.x += (TargetPos.x - pos.x) * rate;
        pos.y += (TargetPos.y - pos.y) * rate;
        CameraGazePos = pos;

        //Nanチェック
        if(!(CameraGazePos.x + CameraGazePos.y >= 0.0f) &&
           !(CameraGazePos.x + CameraGazePos.y <= 0.0f))
        {
            return;//Nanと思われる数値があったため終了
        }

        //カメラの座標をセット
        float CameraZ = transform.position.z;//z軸情報が消えてしまうため保持
        transform.position = CameraGazePos + cameraShakeOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, CameraZ);//Z座標を戻す
    }

    private Vector2 GetTargetPos()
    {
        Vector2 TargetPos = new Vector2(0.0f, 0.0f);

        //目標座標をセット
        if (CameraArea)//カメラエリア
        {
            CameraScrollArea _cameraScrollArea = CameraArea.GetComponent<CameraScrollArea>();
            if (_cameraScrollArea)
            {
                //カメラスクロールエリア
                TargetPos = _cameraScrollArea.GetCameraPos();
            }
            else
            {
                //カメラロックエリア
                TargetPos = CameraArea.transform.position;
                if (player)
                {
                    const float AttractRate = 0.1f;//一割ほどプレイヤーに寄せる
                    Vector3 tempVec = player.transform.position - CameraArea.transform.position;
                    TargetPos += new Vector2(tempVec.x, 0.0f) * AttractRate;
                }
            }
        }
        else
        if (player)//通常時
        {
            TargetPos = player.transform.position;
            TargetPos.x += AdjustCameraPosX * CameraWayX;
        }

        return TargetPos;
    }

    public void SetCameraArea(GameObject _area)
    {
        CameraArea = _area;
    }
    public GameObject GetCameraArea()
    {
        return CameraArea;
    }
    public void SetCameraGazePos(Vector2 _pos)
    {
        CameraGazePos = _pos;
        Vector3 cameraPos = _pos;
        cameraPos.z = transform.position.z;
        transform.position = cameraPos;
    }

    public void ResetCameraPos()
    {
        CameraGazePos = GetTargetPos();

        //Nanチェック
        if (!(CameraGazePos.x + CameraGazePos.y >= 0.0f) &&
           !(CameraGazePos.x + CameraGazePos.y <= 0.0f))
        {
            return;//Nanと思われる数値があったため終了
        }

        //カメラの座標をセット
        float CameraZ = transform.position.z;//z軸情報が消えてしまうため保持
        transform.position = CameraGazePos + cameraShakeOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, CameraZ);//Z座標を戻す
    }
}
