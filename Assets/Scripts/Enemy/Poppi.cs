using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppi : MonoBehaviour
{
    private GameObject Player;//プレイヤーを格納

    [SerializeField] private float ActionTimer;//行動タイマー

    enum ActionStatus
    {
        Standby,//待機状態
        Attacking,//攻撃状態
    }

    [SerializeField] private float FindLength = 1.0f;//プレイヤーを見つける距離
    void Start()
    {
        Player =  GameManager_01.GetPlayer();
    }


    void Update()
    {
        
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
}
