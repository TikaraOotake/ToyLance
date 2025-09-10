using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    [SerializeField]
    private bool SwitchFlag;//活性非活性を記録
    private bool SwitchFlag_old;//前フレームのフラグ状態を記録

    [SerializeField]
    private bool ReversingResult;//結果の反転

	[SerializeField]
    SpriteRenderer _sr;//台座のスプライトレンダラー

    Collider2D _coll;//コライダー

    enum SwitchMode
    {
        Momentary,//接触している時だけ
        Toggle,//接触するたび
        Lock,//活性になったらその状態を維持し続ける
    }
    [SerializeField]
    private SwitchButton[] RadioList;//活性になったとき登録したスイッチを全て非活性にする

    [SerializeField]
    private SwitchMode mode;

    [SerializeField] private float ReceptionCoolTime = 0.1f;//値の変更から指定した時間は変更を受け付けない
    [SerializeField] private float ReceptionCoolTimer;//計測用
    private float ReceptionCoolTimer_old;//計測用

    void Start()
    {
        //色更新
        Color_Update();

        _coll = GetComponent<Collider2D>();//コライダー取得
    }

    // Update is called once per frame
    void Update()
    {
        //値更新
        SwitchFlag_old = SwitchFlag;
        ReceptionCoolTimer_old = ReceptionCoolTimer;
        ReceptionCoolTimer = Mathf.Max(0.0f, ReceptionCoolTimer - Time.deltaTime);
        CheckPushSwitch();//スイッチが押されているかチェック
        if (mode == SwitchMode.Momentary)//モーメントリーモード
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                
                SwitchFlag = true;//タイマー継続中はオン状態を継続する
            }
            else
            {
                SwitchFlag = false;
            }
        }
        else if (mode == SwitchMode.Toggle)//トグルモード
        {
            if (ReceptionCoolTimer > 0.0f && ReceptionCoolTimer_old <= 0.0f)//タイマーが0から変わった瞬間
            {
                SwitchFlag = !SwitchFlag;//フラグ反転
            }
        }

        //フラグが変更された場合
        if (SwitchFlag != SwitchFlag_old)
        {
            //オンの際ラジオリストに登録されているスイッチを全てオフ
            if (SwitchFlag == true)
            {
                for (int i = 0; i < RadioList.Length; ++i)
                {
                    RadioList[i].SetSwitchFlag(false);
                }
            }


        }

        //色更新
        Color_Update();

    }
    public void SetSwitchFlag(bool _flag)
    {
        SwitchFlag = _flag;
    }
    public bool GetSwitchFlag()
    {
        return SwitchFlag_old != ReversingResult;
    }

    private void Color_Update()
    {
        //色を変更
        if (_sr)
        {
            if (SwitchFlag != ReversingResult)
            {
                _sr.color = new Color(0.75f, 0.75f, 0.008f, 1.0f);//黄色
            }
            else
            {
                _sr.color = new Color(0.5f, 0.0f, 0.0f, 1.0f);//赤黒
            }
        }

        //ボタンを押した状態(非表示)に
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                Color color = _This_sr.color;
                color.a = 0.0f;//非表示

                _This_sr.color = color;//代入
            }
            else
            {
                Color color = _This_sr.color;
                color.a = 1.0f;//表示

                _This_sr.color = color;//代入
            }
        }
    }

    private void CheckPushSwitch()
    {
        if (_coll != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(_coll, "Player") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "Ground") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "SpearPlatform"))
            {
                ReceptionCoolTimer = ReceptionCoolTime;//タイマーセット
            }
        }
    }
}
