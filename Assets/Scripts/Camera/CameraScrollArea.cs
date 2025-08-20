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
                    _camera_cs.SetCameraArea(this.gameObject);
                }
                else
                {
                    if (_camera_cs.GetCameraArea() == this.gameObject)
                    {
                        _camera_cs.SetCameraArea(null);
                    }
                }
            }
        }
    }

    public Vector2 GetCameraPos()
    {
        if (PointA == null || PointB == null || Player == null) 
        {
            return transform.position;//必要な変数がそろっていない場合エリア自身の座標を返す
        }

        Vector2 aPos = PointA.transform.position;
        Vector2 bPos = PointB.transform.position;
        Vector2 pPos = Player.transform.position;



        float dx = bPos.x - aPos.x;
        float dy = bPos.y - aPos.y;

        // AとBが同じなら固定
        if (Mathf.Abs(dx) < 1e-6f && Mathf.Abs(dy) < 1e-6f)
            return aPos;

        Vector2 camPos;

        // 線分が縦に近い（傾き45°以上）
        if (Mathf.Abs(dy) > Mathf.Abs(dx))
        {
            // y = A*x + B → x = (y - B) / A
            float A = dy / dx;
            float B = aPos.y - A * aPos.x;

            // プレイヤーのYをABの範囲にClamp
            float camY = Mathf.Clamp(pPos.y, Mathf.Min(aPos.y, bPos.y), Mathf.Max(aPos.y, bPos.y));
            float camX = (camY - B) / A;

            camPos = new Vector2(camX, camY);
        }
        else
        {
            // y = A*x + B
            float A = dy / dx;
            float B = aPos.y - A * aPos.x;

            // プレイヤーのXをABの範囲にClamp
            float camX = Mathf.Clamp(pPos.x, Mathf.Min(aPos.x, bPos.x), Mathf.Max(aPos.x, bPos.x));
            float camY = A * camX + B;

            camPos = new Vector2(camX, camY);
        }

        return camPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;//プレイヤーの登録
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            Player = null;//プレイヤーの登録解除
        }
    }
}
