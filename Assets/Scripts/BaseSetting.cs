using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private string[] TitleSceneName;
    private void Awake()
    {
        if (Player != null)
        {
            GameManager_01.SetPlayer(Player);
        }
        GameManager_01.Initialize();//�}�l�[�W���[������
        GameManager_01.SetTitleSceneName(TitleSceneName);
    }
}
