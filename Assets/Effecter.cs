using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] GameObject EffectPrefab;

    [SerializeField]
    private float SpownInterval = 0.1f;//�����Ԋu
    private float SpownTimer;

    void Start()
    {
        if (EffectPrefab == null) Debug.Log("Effect�̃v���n�u������܂���");
    }

    void Update()
    {


     �@ //�^�C�}�[�ݒ�
        if (SpownTimer <= 0.0f)
        {
            if (EffectPrefab != null)
            {
                Instantiate(EffectPrefab, transform.position, transform.rotation);
            }
            SpownTimer = SpownInterval;
        }

        //�^�C�}�[�X�V
        SpownTimer = Mathf.Max(0.0f, SpownTimer - Time.deltaTime);
    }
}
