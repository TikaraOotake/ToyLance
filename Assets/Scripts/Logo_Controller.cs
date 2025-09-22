using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo_Controller : MonoBehaviour
{
    [SerializeField] private string TitleSceneName;

    [SerializeField] private float EventTimer = 0.0f;

    [SerializeField]
    private float FadeoutTime = 4.5f;
    private bool FadeoutFlag;

    [SerializeField]
    private float ChangeSceneTime = 6.0f;
    private bool ChangeSceneFlag;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeoutTime <= EventTimer && FadeoutFlag == false)
        {
            Debug.Log("�t�F�[�h�A�E�g���܂�");
            FadeoutFlag = true;
            GameManager_01.SetBlindFade(true);
        }
        if (ChangeSceneTime <= EventTimer && ChangeSceneFlag == false)
        {
            Debug.Log("�`�F���W�V�[�����J�n���܂�");
            ChangeSceneFlag = true;
            GameManager_01.LoadScene(TitleSceneName);
        }

        //�^�C�}�[���Z
        EventTimer += Time.deltaTime;
    }
}
