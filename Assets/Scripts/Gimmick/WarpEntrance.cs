using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarpEntrance : MonoBehaviour
{
    [SerializeField]
    private Vector2 ExitPos;//出口の座標
    [SerializeField]
    private GameObject ExitObject;//出口のオブジェクト(オブジェクトが設定されていたらそちらの座標を優先)

    [SerializeField]
    private string NextSceneName;//次のシーンの名前

    [SerializeField]
    private bool IsDoorLock = false;//ドアロックフラグ
    [SerializeField]
    private Sprite Open;//開錠時のスプライト
    [SerializeField]
    private Sprite Close;//閉錠時のスプライト

    [SerializeField]
    GameObject Camera;
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Vector2 FadeOutPlayerPos;//フェードアウト時のプレイヤー座標を記録

    private float TransferStartTimer = 0.0f;

    [SerializeField]
    private SwitchButton[] SwitchList;//登録されたスイッチが全てオンであれば扉を開ける(逆もしかり)

    private Animator _anim;//アニメーター

    void Start()
    {
        //カメラ取得
        Camera = GameObject.Find("Main Camera");

        //出口座標取得
        if (ExitObject)
        {
            ExitPos = ExitObject.transform.position;
        }

        //アニメーター取得
        _anim = GetComponent<Animator>();

        Sprite_Update();//スプライト更新
    }
    void Update()
    {
        if (SwitchList.Length > 0)//スイッチが登録されているときのみ
        {
            bool IsDoorLock_old = IsDoorLock;//変更前の状態を記録

            IsDoorLock = false;//初期化
            //スイッチのどれかひとつでもオフであれば施錠する
            for (int i = 0; i < SwitchList.Length; ++i)
            {
                if (!SwitchList[i].GetSwitchFlag()) { IsDoorLock = true; }
            }

            //開錠されたらカメラを振動させる
            if (IsDoorLock != IsDoorLock_old && IsDoorLock == false)//開錠された瞬間を見る条件式
            {
                const float Length = 0.05f;
                const float Speed = 10.0f;
                const float LowValue = 10.0f;
                CameraManager.SetShakeCamera(Length, Speed, LowValue);
            }
        }

        SetAnim();//アニメーションの更新
        Sprite_Update();//スプライト更新

        //タイマー更新前に時間を記録しておく
        float StartTimer_old = TransferStartTimer;

        //タイマー更新
        TransferStartTimer = Mathf.Max(0.0f, TransferStartTimer - Time.deltaTime);

        //タイマー継続中にプレイヤーの座標を固定
        if (TransferStartTimer > 0)
        {
            if (Player)
            {
                Player.transform.position = new Vector2(FadeOutPlayerPos.x, Player.transform.position.y);
            }
        }

        //タイマーが0になった瞬間で転送を実行
        if (StartTimer_old > 0.0f && TransferStartTimer <= 0.0f)
        {
            TransferPlayer();
        }
    }

    private void TransferPlayer()
    {
        if (!Player)
        {
            return;//プレイヤーが設定されていないので終了
        }

        //Debug.Log("移動処理中");

        // 移動先のシーンが設定されていないなら座標をセットするだけ
        if (NextSceneName == "")
        {
            //座標セット
            Vector3 pos = ExitPos;
            pos.z = Player.transform.position.z;
            Player.transform.position = pos;    //プレイヤー側
            GameManager_01.SetStartPlayerPos(pos); //ゲームマネージャー側 ※記録先は変更するかもしれません

            //加速度初期化
            Rigidbody2D _rb = Player.GetComponent<Rigidbody2D>();
            if (_rb)
            {
                _rb.velocity = Vector3.zero;
            }

            //カメラの座標
            pos = ExitPos;
            //GameManager_01.SetCameraGazePos(pos);
            StartCoroutine(CollResetCameraPos());

            //プレイヤーに移動終了を伝える
            Player_01_Control playerControl=Player.GetComponent<Player_01_Control>();
            if(playerControl!=null)
            {
                playerControl.DoorExit();
            }
        }
        else
        {
            //座標セット
            GameManager_01.SetStartPlayerPos(ExitPos);

            //シーン読み込み
            GameManager_01.LoadScene(NextSceneName);
        }

        //プレイヤーの登録解除
        Player = null;
    }

    IEnumerator CollResetCameraPos()
    {
        yield return null;
        yield return null;

        //フェードインさせる
        GameManager_01.SetBlindFade(false);

        //カメラの座標をリセット
        GameManager_01.ResetCameraPos();
    }
    public void SetDoorLock(bool _flag)
    {
        IsDoorLock = _flag;
        Sprite_Update();//スプライト更新
    }

    private void SetAnim()
    {
        if (_anim)
        {
            _anim.SetBool("IsDoorLock", IsDoorLock);
        }
    }

    //スプライトの更新
    private void Sprite_Update()
    {
        //スプライト更新
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        if (_sr)
        {
            if (IsDoorLock)
            {
                if (Close) { _sr.sprite = Close; }
            }
            else
            {
                if (Open) { _sr.sprite = Open; }
            }
        }
    }
    public bool TeleportSetting(GameObject _Player)
    {
        if (_Player == null) return false;//無効なプレイヤーであれば終了
        if (IsDoorLock == true) return false;//施錠中のドアの為終了

        Player = _Player;

        //タイマーセット
        TransferStartTimer = 0.4f;

        //フェードアウトさせる
        if (Camera)
        {
            UIManager _uiMng = Camera.GetComponent<UIManager>();
            if (_uiMng)
            {
                _uiMng.SetBlindFade(true);
            }
        }

        //座標を記録
        if (Player)
        {
            FadeOutPlayerPos = Player.transform.position;
        }

        return true;//成功として処理
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーを登録
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーの登録解除
        if (collision.gameObject == Player)
        {
            //移動処理中でないとき
            if (TransferStartTimer <= 0.0f)
            {
                Player = null;
            }
        }
    }
}
