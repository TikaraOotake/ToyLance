using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentSoftLight : MonoBehaviour
{
    private SpriteRenderer _sr;//�X�v���C�g�����_���[

    [SerializeField] 
    private float MaxSize = 1.0f;//�ő�T�C�Y
    private float Size = 0.0f;
  

    [SerializeField]
    private float ExpansionRate = 1.0f;//�c����

    private float BaseAlpha;//��ƂȂ铧���x
    private float AlphaRate = 1.0f;//���ߗ�

    [SerializeField]
    private float DecayAlphaRate = 1.0f;//���ߗ������x

    [SerializeField]
    private float WaveAlpha;//�g�`�̓����x
    [SerializeField]
    private float Amplitude = 1.0f;//�U��
    [SerializeField]
    private float Cycle = 1.0f;//����

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (_sr)
        {
            BaseAlpha = _sr.color.a;//�����x�擾
        }

        transform.localScale = new Vector2(Size, Size);//�傫�����
    }

    // Update is called once per frame
    void Update()
    {
        //�g�`�̌v�Z
        WaveAlpha = Mathf.Sin(Time.time * Cycle) * Amplitude;

        if (_sr)
        {
            Color BaseCollar = _sr.color;//�F���擾
            BaseCollar.a = 0.0f;//�A���t�@�l��������
            _sr.color = new Color(0.0f, 0.0f, 0.0f, (0.5f + WaveAlpha) * AlphaRate) + BaseCollar;
        }


        float rate = 1.0f - Mathf.Pow(1.0f - ExpansionRate, Time.deltaTime * 60f); // �b��ChaseRate�ɂȂ�悤��
        Size += (MaxSize - Size) * rate;

        transform.localScale = new Vector2(Size, Size);//�傫�����

        //���ߗ��������Ă���
        AlphaRate = Mathf.Max(0.0f, AlphaRate - Time.deltaTime * DecayAlphaRate);

        //���ߗ���0�����������폜
        if (AlphaRate <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
