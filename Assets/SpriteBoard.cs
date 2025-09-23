using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBoard : MonoBehaviour
{
    GameObject obj;
    private Vector2 ShakeOffset;
    private Vector2 Offset;

    //振動に関わる変数
    private Vector2 ShakeRot_Vec;//振動ベクトル
    private float ShakeRot_Length;//半径
    [SerializeField]
    private float ShakeRot_Speed = 1.0f;//回転速度
    [SerializeField]
    private float ShakeRot_LowValue = 1.0f;//減衰量
    private float ShakeRot_Angle = 0.0f;//角度
    void Start()
    {
        obj = transform.parent.gameObject;//親のオブジェクトを取得

        if (obj != null)
        {
            Offset = transform.position - obj.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCamera_Update();
        if (obj != null)
        {
            Vector2 ParentPos = obj.transform.position;
            transform.position = ShakeOffset + Offset + ParentPos;
        }
    }
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

        //スプライトの位置に振動を加える
        ShakeOffset = (Vector3)ShakeRot_Vec;
    }


    public void SetShakeSprite()
    {
        const float Length = 0.2f;
        const float Speed = 10.0f;
        const float LowValue = 10.0f;
        SetShakeSprite(Length, Speed, LowValue);
    }
    public void SetShakeSprite(float _Length, float _Speed, float _LowValue)
    {
        ShakeRot_Length = _Length;   //半径
        ShakeRot_Speed = _Speed;     //回転速度
        ShakeRot_LowValue = _LowValue;   //減衰量
        ShakeRot_Angle = Random.Range(0f, 360f);    //角度はランダムにスタート
    }
}
