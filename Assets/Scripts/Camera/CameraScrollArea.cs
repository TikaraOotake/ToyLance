using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollArea : MonoBehaviour
{
    //AB間の線分をカメラの移動に適用するエリア
    [SerializeField]
    GameObject PointA;
    [SerializeField]
    GameObject PointB;

    GameObject Player;
    GameObject Camera;

    private Vector2 CameraPos;

    [SerializeField]
    private float OffsetLength = 1.0f;//プレイヤーがどの程度進んでいるとみなすかの距離

    void Start()
    {
        //当たり判定確認用のレンダラー透明化
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        if (_sr)
        {
            _sr.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }

        //カメラ取得
        Camera = CameraManager.GetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera != null)
        {
            CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
            if (_camera_cs)
            {
                if (Player)
                {
                    //_camera_cs.SetCameraArea(this.gameObject);
                }
                else
                {
                    if (_camera_cs.GetCameraArea() == this.gameObject)
                    {
                        //_camera_cs.SetCameraArea(null);
                    }
                }
            }
        }
    }

    public Vector2 GetCameraPos()
    {
        if (PointA == null || PointB == null || Player == null)
        {
            return transform.position; // 必要な変数がそろっていない場合エリア自身の座標を返す
        }

        Vector2 aPos = PointA.transform.position;
        Vector2 bPos = PointB.transform.position;
        Vector2 pPos = Player.transform.position;

        float dx = bPos.x - aPos.x;
        float dy = bPos.y - aPos.y;

        // AとBがほぼ同じなら固定
        if (Mathf.Abs(dx) < 1e-6f && Mathf.Abs(dy) < 1e-6f)
            return aPos;

        // dxがほぼゼロ（垂直線に近い）なら特別処理
        if (Mathf.Abs(dx) < 1e-6f)
        {
            // X座標はaPos.x固定、YをABの範囲にClamp
            float camY = Mathf.Clamp(pPos.y, Mathf.Min(aPos.y, bPos.y), Mathf.Max(aPos.y, bPos.y));
            return new Vector2(aPos.x, camY);
        }

        Vector2 camPos;

        float A = dy / dx;
        float B = aPos.y - A * aPos.x;

        // 線分が縦に近い（傾き45°以上）
        if (Mathf.Abs(dy) > Mathf.Abs(dx))
        {
            // y = A*x + B → x = (y - B) / A

            // プレイヤーのYをABの範囲にClamp
            float camY = Mathf.Clamp(pPos.y, Mathf.Min(aPos.y, bPos.y), Mathf.Max(aPos.y, bPos.y));
            float camX = (camY - B) / A;

            camPos = new Vector2(camX, camY);
        }
        else
        {
            // y = A*x + B

            // プレイヤーのXをABの範囲にClamp
            float camX = Mathf.Clamp(pPos.x, Mathf.Min(aPos.x, bPos.x), Mathf.Max(aPos.x, bPos.x));
            float camY = A * camX + B;

            camPos = new Vector2(camX, camY);
        }

        return camPos;
    }
    public Vector2 GetCameraPos(float thresholdDistance, float smoothing = 0.1f)
    {
        if (PointA == null || PointB == null || Player == null)
        {
            return transform.position; // 必要な変数がそろっていない場合は現在の座標を返す
        }

        Vector2 aPos = PointA.transform.position;
        Vector2 bPos = PointB.transform.position;
        Vector2 pPos = Player.transform.position;
        Vector2 currentCamPos = CameraPos;

        //プレイヤー座標に補正値を加える
        if (Player != null)
        {
            Player_01_Control playerControl = Player.GetComponent<Player_01_Control>();
            if (playerControl != null)
            {
                pPos.x += playerControl.GetPlayerDirection() * OffsetLength;
            }
        }

        Vector2 ab = bPos - aPos;
        float dx = ab.x;
        float dy = ab.y;

        if (ab.sqrMagnitude < 1e-12f)
            return aPos;

        Vector2 idealCamPos;

        if (Mathf.Abs(dx) < 1e-6f)
        {
            float camY = Mathf.Clamp(pPos.y, Mathf.Min(aPos.y, bPos.y), Mathf.Max(aPos.y, bPos.y));
            idealCamPos = new Vector2(aPos.x, camY);
        }
        else
        {
            float A = dy / dx;
            float B = aPos.y - A * aPos.x;

            if (Mathf.Abs(dy) > Mathf.Abs(dx))
            {
                float camY = Mathf.Clamp(pPos.y, Mathf.Min(aPos.y, bPos.y), Mathf.Max(aPos.y, bPos.y));
                float camX = (camY - B) / A;
                idealCamPos = new Vector2(camX, camY);
            }
            else
            {
                float camX = Mathf.Clamp(pPos.x, Mathf.Min(aPos.x, bPos.x), Mathf.Max(aPos.x, bPos.x));
                float camY = A * camX + B;
                idealCamPos = new Vector2(camX, camY);
            }
        }

        Vector2 abDirNormalized = ab.normalized;
        Vector2 diffVec = pPos - currentCamPos;
        float projectedDistance = Mathf.Abs(Vector2.Dot(diffVec, abDirNormalized));

        if (projectedDistance > thresholdDistance)
        {
            // 補間して少しずつ理想位置に近づける
            CameraPos = Vector2.Lerp(currentCamPos, idealCamPos, smoothing);
        }
        else
        {
            // 閾値内なら現状維持（または徐々に元の位置に戻すなども可能）
            CameraPos = currentCamPos;
        }

        return CameraPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;
            CameraPos = GetCameraPos();//カメラ座標を初期化
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;

            if (Camera != null)
            {
                CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
                if (_camera_cs)
                {
                    //誰も登録されていなかったら自身を登録
                    if (_camera_cs.GetCameraArea() == null)
                    {
                        _camera_cs.SetCameraArea(this.gameObject);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Player = null;

            if (Camera != null)
            {
                CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
                if (_camera_cs)
                {
                    //自身が登録されていた場合解除
                    if (_camera_cs.GetCameraArea() == this.gameObject)
                    {
                        _camera_cs.SetCameraArea(null);
                    }
                }
            }
        }
    }
}
