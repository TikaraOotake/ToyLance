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

    [SerializeField]
    private float ReceptionCoolTime = 0.0f;//値の変更から指定した時間は変更を受け付けない
    private float ReceptionCoolTimer;//計測用

    void Start()
    {
        //色更新
        Color_Update();
    }

    // Update is called once per frame
    void Update()
    {
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

            //色更新
            Color_Update();

            //クールタイムセット
            ReceptionCoolTimer = ReceptionCoolTime;
        }



        //値更新
        SwitchFlag_old = SwitchFlag;
        if (ReceptionCoolTimer > 0.0f)
        {
            ReceptionCoolTimer -= Time.deltaTime;
        }
        else if (ReceptionCoolTimer < 0.0f)
        {
            ReceptionCoolTimer = 0.0f;
        }
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ReceptionCoolTimer > 0)
        {
            return;//クールタイム中であれば終了
        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//プレイヤーでも地形でもなければ終了
        }

        if (mode == SwitchMode.Toggle)
        {
            SwitchFlag = !SwitchFlag;
        }
        else if (mode == SwitchMode.Lock)
        {
            SwitchFlag = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ReceptionCoolTimer > 0)
        {
            return;//クールタイム中であれば終了
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//プレイヤーでも地形でもなければ終了
        }

        //ボタンを押した状態(非表示)に
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            Color color = _This_sr.color;
            color.a = 0.0f;//非表示

            _This_sr.color = color;//代入
        }

        //フラグを変更
        if (mode == SwitchMode.Momentary)
        {
            SwitchFlag = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
           collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            return;//プレイヤーでも地形でもなければ終了
        }

        //ボタンを離した状態(表示)に
        SpriteRenderer _This_sr = GetComponent<SpriteRenderer>();
        if (_This_sr)
        {
            Color color = _This_sr.color;
            color.a = 1.0f;//表示

            _This_sr.color = color;//代入
        }

        if (mode == SwitchMode.Momentary)
        {
            SwitchFlag = false;
        }
    }
}
