using UnityEngine;
using UnityEngine.SceneManagement;

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


    void Start()
    {
        //カメラ取得
        Camera = GameObject.Find("Main Camera");

        //出口座標取得
        if (ExitObject)
        {
            ExitPos = ExitObject.transform.position;
        }

        Sprite_Update();//スプライト更新
    }

    // Update is called once per frame
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


        Sprite_Update();//スプライト更新

        //プレイヤーが登録されていたら移動処理
        if (Player)
        {
            if (Input.GetKeyDown(KeyCode.W) && !IsDoorLock)//移動入力かつ施錠されていないとき
            {
                //タイマーセット
                TransferStartTimer = 1.0f;

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
            }
        }

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
        if(!Player)
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
            GameManager_01.SetCameraGazePos(pos);

            //フェードアウトさせる
            GameManager_01.SetBlindFade(false);
        }
        else
        {
            //座標セット
            GameManager_01.SetStartPlayerPos(ExitPos);

            //シーン読み込み
            SceneManager.LoadScene(NextSceneName);
        }

        //プレイヤーの登録解除
        Player = null;
    }
    public void SetDoorLock(bool _flag)
    {
        IsDoorLock = _flag;
        Sprite_Update();//スプライト更新
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
