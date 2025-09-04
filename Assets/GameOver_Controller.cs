using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver_Controller : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGameOver()
    {
         GameManager_01.RespawnPlayer();
    }
}
