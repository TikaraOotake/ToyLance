using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting : MonoBehaviour
{
    [SerializeField] private string[] TitleSceneName;
    private void Awake()
    {
        GameManager_01.Initialize();//�}�l�[�W���[������
        GameManager_01.SetTitleSceneName(TitleSceneName);
    }
}
