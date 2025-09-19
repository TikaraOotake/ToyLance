using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    private static HitStopManager _instance;

    // �C���X�^���X�̎擾
    public static HitStopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // �V�[������HitStopManager�I�u�W�F�N�g���Ȃ��ꍇ�A�V�����쐬����
                _instance = FindObjectOfType<HitStopManager>();

                if (_instance == null)
                {
                    GameObject hitStopObject = new GameObject("HitStopManager");
                    _instance = hitStopObject.AddComponent<HitStopManager>();
                }
            }
            return _instance;
        }
    }

    // �q�b�g�X�g�b�v�̏��
    public static bool IsHitStopActive { get; private set; }
    private static float hitStopTime;
    private float timeLeft;

    void Update()
    {
        if (IsHitStopActive)
        {
            timeLeft -= Time.unscaledDeltaTime;  // �Q�[���̎��ԂɊ֌W�Ȃ��J�E���g�_�E��
            if (timeLeft <= 0)
            {
                //Debug.Log("���Ԃ����ɖ߂�");
                IsHitStopActive = false;
                Time.timeScale = 1f;  // ���Ԃ����ɖ߂�
            }
        }
    }

    // �q�b�g�X�g�b�v���J�n����ÓI���\�b�h
    public static void StartHitStop(float duration)
    {
        //Debug.Log("�q�b�g�X�g�b�v���J�n");

        if (IsHitStopActive) return;  // ���łɃq�b�g�X�g�b�v���͊J�n���Ȃ�

        IsHitStopActive = true;
        hitStopTime = duration;
        Instance.timeLeft = duration;
        Time.timeScale = 0.1f;  // ���Ԃ̃X�P�[���������ăX���[��
    }
}
