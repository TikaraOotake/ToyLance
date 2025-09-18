using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private int Sequence = 0;//段階
    [SerializeField] private GameObject[] SequencePointObj;//段階ごとの座標を入れる
    private List<GameObject> SequencePointObj_copy;

    bool IsIdle;


    [SerializeField] private GameObject Player;//Player

    [SerializeField]
    private float BoardShiftHeight;//画像をずらす高さ
    private float BoardShiftHeight_old;

    [SerializeField] private float JumpValue;//ジャンプ量

    [SerializeField] private float MigrationProgress;//進行度　0:始　1:終

    [SerializeField] private float ProgressSpeed = 0.5f;//進行速度

    [SerializeField] private Vector2 DeparturePos;//出発座標
    [SerializeField] private Vector2 TargetPos;//目標座標

    [SerializeField] private float FindLength = 1.0f;//プレイヤーを見つける距離

    [SerializeField] private float IdleTime = 1.0f;//待機時間
    [SerializeField] private float IdleTimer;//待機タイマー

    [SerializeField] 
    private GameObject SpriteBoard;//画像を表示するオブジェクト
    private Animator _anim;//アニメーター

    private SEManager _seManager;
    private Renderer _renderer;

    void Start()
    {
        Player = GameManager_01.GetPlayer();//プレイヤー取得
        if (SpriteBoard != null)
        {
            _anim = SpriteBoard.GetComponent<Animator>();//アニメーター取得
        }


        DeparturePos = transform.position;//出発座標設定
        TargetPos = DeparturePos;//念のため仮上書き

        if (Sequence < SequencePointObj.Length)//配列外チェック
        {
            GameObject TargetObj = SequencePointObj[Sequence];
            if (TargetObj != null)
            {
                TargetPos = TargetObj.transform.position;//目標座標設定
            }
        }

        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SEの取得に失敗");

        _renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HoppingSprite();//スプライトを上下にずらして跳ねさせる

        //進行度を満了したら再設定
        if (MigrationProgress >= 1.0f)
        {
            MigrationProgress = 0.0f;//進捗度リセット
            IdleTimer = IdleTime;//待機タイマーセット
            FacingPlayerSprite();//プレイヤーの方向を向く
        }

        if (IdleTimer <= 0.0f)//待機中でない場合
        {
            //行先設定
            if (MigrationProgress == 0.0f)
            {
                DeparturePos = transform.position;//出発座標設定

                if (CheckPlayerOverlap())
                {
                    Debug.Log("プレイヤーが近付きました");

                    ++Sequence;//次の段階に


                    TargetPos = DeparturePos;//念のため仮上書き

                    if (Sequence < SequencePointObj.Length)//配列外チェック
                    {
                        GameObject TargetObj = SequencePointObj[Sequence];
                        if (TargetObj != null)
                        {
                            TargetPos = TargetObj.transform.position;//目標座標設定
                        }
                    }

                    //スプライトの向きを合わせる
                    if (TargetPos != DeparturePos)//移動先が設定されていた場合
                    {
                        FacingMoveWaySprite();
                    }

                }
            }

            if (MigrationProgress == 0.0f && IsVisible())
            {
                //移動音
                _seManager.PlaySE("rabbit");
            }


            //進行度に合わせてウサギを移動
            Vector2 TargetVec = TargetPos - DeparturePos;//目標までのベクトルを算出
            transform.position = TargetVec * MigrationProgress + DeparturePos;
            
            //進行度更新
            MigrationProgress = Mathf.Min(1.0f, MigrationProgress + ProgressSpeed * Time.deltaTime);
        }

        //プレイヤーが一定範囲内にいる　&　次の行先が決まっている場合
        if (CheckPlayerOverlap() && TargetPos != DeparturePos)
        {
            IdleTimer = 0.0f;//待機時間を踏み倒す
        }

        //待機タイマー更新
        IdleTimer = Mathf.Max(0.0f, IdleTimer - Time.deltaTime);

        SetAnim();//アニメーションセット
    }
    private void HoppingSprite()
    {
        //進行度から画像をずらす量を計算
        BoardShiftHeight_old = BoardShiftHeight;//前の値を記録
        BoardShiftHeight = Mathf.Sin(MigrationProgress * Mathf.PI);

        //画像をずらす
        if (SpriteBoard != null)
        {
            SpriteBoard.transform.position = new Vector3(0.0f, BoardShiftHeight, 0.0f) + transform.position;
        }
    }
    private void FacingPlayerSprite()//プレイヤーの方向を向く
    {
        if (SpriteBoard == null) return;//スプライトボードがないため終了

        //プレイヤーの方向に合わせる
        if (Player != null)
        {
            if (Player.transform.position.x > transform.position.x)
            {
                SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 180, 0.0f);
            }
            else
            {
                SpriteBoard.transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private void FacingMoveWaySprite()//移動方向を向く
    {
        if (SpriteBoard == null) return;//スプライトボードがないため終了

        if (TargetPos.x > DeparturePos.x)
        {
            SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 180, 0.0f);
        }
        else
        {
            SpriteBoard.transform.eulerAngles = Vector3.zero;
        }
    }
    private void SetAnim()
    {
        if (_anim == null) return;
        //フラグを一旦リセット
        _anim.SetBool("IsIdle", false);
        _anim.SetBool("IsHopping_Rising", false);
        _anim.SetBool("IsHopping_Floating", false);
        _anim.SetBool("IsHopping_Falling", false);

        if (IdleTimer > 0.0f)
        {
            _anim.SetBool("IsIdle", true);
        }
        else if (BoardShiftHeight >= 0.8f)
        {
            _anim.SetBool("IsHopping_Floating", true);
        }
        else if(BoardShiftHeight >= BoardShiftHeight_old)
        {
            _anim.SetBool("IsHopping_Rising", true);
        }
        else
        {
            _anim.SetBool("IsHopping_Falling", true);
        }
    }

    private bool CheckPlayerOverlap()//プレイヤーが一定範囲内に入ったか確認
    {
        if (Player != null)
        {
            Vector2 PlayerPos = Player.transform.position;
            Vector2 ThisPos = transform.position;
            Vector2 Length = PlayerPos - ThisPos;//距離を算出
            if (FindLength >= Length.magnitude)
            {
                return true;
            }
        }

        return false;
    }

    private void OnValidate()
    {
        //最初の段階座標は自身の座標にする

        //配列外チェック
        if (0 < SequencePointObj.Length)
        {
            if (SequencePointObj[0] != null) return;

            SequencePointObj[0] = new GameObject();
            SequencePointObj[0].transform.position = transform.position;
            SequencePointObj[0].name = "TargetPoint";
        }
    }

    private bool IsVisible()
    {
        return _renderer.isVisible;
    }
}
