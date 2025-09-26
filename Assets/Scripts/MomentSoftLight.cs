using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentSoftLight : MonoBehaviour
{
    private SpriteRenderer _sr;//スプライトレンダラー

    [SerializeField] 
    private float MaxSize = 1.0f;//最大サイズ
    private float Size = 0.0f;
  

    [SerializeField]
    private float ExpansionRate = 1.0f;//膨張率

    private float BaseAlpha;//基準となる透明度
    private float AlphaRate = 1.0f;//透過率

    [SerializeField]
    private float DecayAlphaRate = 1.0f;//透過率減衰度

    [SerializeField]
    private float WaveAlpha;//波形の透明度
    [SerializeField]
    private float Amplitude = 1.0f;//振幅
    [SerializeField]
    private float Cycle = 1.0f;//周期

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (_sr)
        {
            BaseAlpha = _sr.color.a;//透明度取得
        }

        transform.localScale = new Vector2(Size, Size);//大きさ代入
    }

    // Update is called once per frame
    void Update()
    {
        //波形の計算
        WaveAlpha = Mathf.Sin(Time.time * Cycle) * Amplitude;

        if (_sr)
        {
            Color BaseCollar = _sr.color;//色を取得
            BaseCollar.a = 0.0f;//アルファ値を初期化
            _sr.color = new Color(0.0f, 0.0f, 0.0f, (0.5f + WaveAlpha) * AlphaRate) + BaseCollar;
        }


        float rate = 1.0f - Mathf.Pow(1.0f - ExpansionRate, Time.deltaTime * 60f); // 秒間ChaseRateになるように
        Size += (MaxSize - Size) * rate;

        transform.localScale = new Vector2(Size, Size);//大きさ代入

        //透過率を下げていく
        AlphaRate = Mathf.Max(0.0f, AlphaRate - Time.deltaTime * DecayAlphaRate);

        //透過率が0を下回ったら削除
        if (AlphaRate <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
