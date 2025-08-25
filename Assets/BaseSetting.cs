using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting : MonoBehaviour
{

    private void Awake()
    {
        GameManager_01.Initialize();//マネージャー初期化
    }
}
