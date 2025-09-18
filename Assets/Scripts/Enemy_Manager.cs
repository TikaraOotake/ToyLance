using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    // シングルトンのインスタンス
    private static Enemy_Manager Enemy_ManagerInstance;

    // インスタンスの取得
    public static Enemy_Manager Instance
    {
        get
        {
            if (Enemy_ManagerInstance == null)
            {
                // シーン内にEnemy_Managerオブジェクトがない場合、新しく作成する
                Enemy_ManagerInstance = FindObjectOfType<Enemy_Manager>();

                if (Enemy_ManagerInstance == null)
                {
                    GameObject Enemy_Manager = new GameObject("Enemy_Manager");
                    Enemy_ManagerInstance = Enemy_Manager.AddComponent<Enemy_Manager>();
                }
            }
            return Enemy_ManagerInstance;
        }
    }

    //死亡したエネミーを記録しておくリスト
    private List<GameObject> DeadEnemyList = new List<GameObject>();

    public void SetDeadEnemy(GameObject _enemy)
    {
        if (_enemy != null)
        {
            DeadEnemyList.Add(_enemy);

            Debug.Log(_enemy.name + "を死亡リストに登録");
        }
    }

    private void Update()
    {
        // リストを後ろからループして、リスポーンするかチェック
        for (int i = DeadEnemyList.Count - 1; i >= 0; i--)
        {
            if (DeadEnemyList[i] != null)
            {
                bool result = false;
                EnemyHealth enemyHealth = DeadEnemyList[i].GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    result = enemyHealth.CheckRespawn();
                }

                if (result == true)
                {
                    DeadEnemyList.RemoveAt(i);//復帰に成功したらリストから外す
                }

            }
        }
    }

}
