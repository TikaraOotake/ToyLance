using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shieldFlipper : MonoBehaviour
{
    private bool isFacingRight = false;     //右を向いているか

    //盾の反転処理
    public void UpdateShieldPosition(bool FacingRight)
    {
        if (FacingRight == isFacingRight)
        {
            return;
        }

        isFacingRight = FacingRight;

        float yRotation;

        //右を向いているなら
        if (FacingRight)
        {
            //0°
            yRotation = 0f;
        }
        else
        {
            //180°
            yRotation = 180f;
        }

        //現在の回転を取得
        Vector3 currentRotation = transform.localEulerAngles;
        //回転を更新
        currentRotation.y = yRotation;
        //回転を適用
        transform.localEulerAngles = currentRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
