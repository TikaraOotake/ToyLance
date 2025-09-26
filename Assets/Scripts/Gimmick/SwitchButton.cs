using UnityEngine;
using UnityEngine.UIElements;

public class SwitchButton : MonoBehaviour
{
    [SerializeField]
    private bool SwitchFlag;//活性非活性を記録
    private bool SwitchFlag_old;//前フレームのフラグ状態を記録

    [SerializeField]
    private bool ReversingResult;//結果の反転

	[SerializeField]
    SpriteRenderer _sr;//台座のスプライトレンダラー
    [SerializeField]
    private GameObject Button;

    [SerializeField]
    private float PushSpeed = 1.0f;//押し込み速度

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

    [SerializeField] private Animator anim_Button;//実際に押されるボタンのアニメーション
    [SerializeField] private Animator anim_Base;//台座のアニメーション


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
        else if (mode == SwitchMode.Lock)//ロックモード
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                SwitchFlag = true;//タイマー継続中はオン状態を継続する
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

        if (ReceptionCoolTimer > 0.0f && ReceptionCoolTimer_old <= 0.0f)//押された瞬間
        {
            //SEを再生
            SEManager.instance.PlaySE("break");
        }

        //色更新
        Color_Update();

    }
    public void SetSwitchFlag(bool _flag)
    {
        SwitchFlag = _flag;
        if (!_flag)
        {
            ReceptionCoolTimer = 0.0f;//タイマーも初期化
        }
    }
    public bool GetSwitchFlag()
    {
        return SwitchFlag_old != ReversingResult;
    }

    private void Color_Update()
    {
        if (anim_Base != null)
        {
            if (SwitchFlag != ReversingResult)
            {
                anim_Base.SetBool("IsActive", true);//有効
            }
            else
            {
                anim_Base.SetBool("IsActive", false);//非有効
            }
        }

        if (anim_Button != null)
        {
            if (ReceptionCoolTimer > 0.0f)
            {
                anim_Button.SetBool("IsActive", true);//押されている状態
            }
            else
            {
                anim_Button.SetBool("IsActive", false);//押されていない状態
            }
        }
        

    }

    private void CheckPushSwitch()
    {
        if (_coll != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(_coll, "Player") ||
                Collision_Manager.GetTouchingObjectWithLayer(_coll, "SpearPlatform"))
            {
                ReceptionCoolTimer = ReceptionCoolTime;//タイマーセット
            }
        }
    }
}
