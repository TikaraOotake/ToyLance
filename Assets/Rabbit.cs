using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private int Sequence=0;//段階
    [SerializeField] private GameObject[] SequencePointObj;//段階ごとの座標を入れる
    private List<GameObject> SequencePointObj_copy;

    [SerializeField] private GameObject SpriteBoard;//画像を表示するオブジェクト
    [SerializeField] private GameObject Player;//Player

    [SerializeField] private float BoardShiftHeight;//画像をずらす高さ

    [SerializeField] private float JumpValue;//ジャンプ量

    [SerializeField] private float MigrationProgress;//進行度　0:始　1:終

    [SerializeField] private float ProgressSpeed = 0.5f;//進行速度

    [SerializeField] private Vector2 DeparturePos;//出発座標
    [SerializeField] private Vector2 TargetPos;//目標座標

    [SerializeField] private float FindLength = 1.0f;//プレイヤーを見つける距離

    void Start()
    {
        Player = GameManager_01.GetPlayer();//プレイヤー取得



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
    }

    // Update is called once per frame
    void Update()
    {
        //進行度から画像をずらす量を計算
        BoardShiftHeight = Mathf.Sin(MigrationProgress * Mathf.PI);

        //画像をずらす
        if (SpriteBoard != null)
        {
            SpriteBoard.transform.position = new Vector3(0.0f, BoardShiftHeight, 0.0f) + transform.position;
        }

        //進行度が満たしたら戻す
        if (MigrationProgress >= 1.0f)
        {
            MigrationProgress = 0.0f;
        }

        //行先設定
        if (MigrationProgress == 0.0f)
        {
            DeparturePos = transform.position;//出発座標設定

            //プレイヤーが一定より近づいていたら
            if (Player != null)
            {
                Vector2 PlayerPos = Player.transform.position;
                Vector2 ThisPos = transform.position;
                Vector2 Length = PlayerPos - ThisPos;//距離を算出
                if (FindLength >= Length.magnitude)
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
                }
            }
        }

        //進行度に合わせてウサギを移動
        Vector2 TargetVec = TargetPos - DeparturePos;//目標までのベクトルを算出
        transform.position = TargetVec * MigrationProgress + DeparturePos;

        //進行度更新
        MigrationProgress = Mathf.Min(1.0f, MigrationProgress + ProgressSpeed * Time.deltaTime);
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
}
