using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBoard : MonoBehaviour
{
    GameObject obj;
    private Vector2 ShakeOffset;
    private Vector2 Offset;

    //�U���Ɋւ��ϐ�
    private Vector2 ShakeRot_Vec;//�U���x�N�g��
    private float ShakeRot_Length;//���a
    [SerializeField]
    private float ShakeRot_Speed = 1.0f;//��]���x
    [SerializeField]
    private float ShakeRot_LowValue = 1.0f;//������
    private float ShakeRot_Angle = 0.0f;//�p�x
    void Start()
    {
        obj = transform.parent.gameObject;//�e�̃I�u�W�F�N�g���擾

        if (obj != null)
        {
            Offset = transform.position - obj.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCamera_Update();
        if (obj != null)
        {
            Vector2 ParentPos = obj.transform.position;
            transform.position = ShakeOffset + Offset + ParentPos;
        }
    }
    private void ShakeCamera_Update()
    {
        //�U�����I�����Ă���Ή������Ȃ�
        if (ShakeRot_Length <= 0.01f)
        {
            ShakeRot_Vec = Vector2.zero;
            return;
        }

        //�p�x�����Z���ĉ�]�i���t���[���j
        ShakeRot_Angle += ShakeRot_Speed * Time.unscaledDeltaTime * 360f; //1�b��1��]
        if (ShakeRot_Angle >= 360f)
        {
            ShakeRot_Angle -= 360f;
        }

        //�~�^���x�N�g�����v�Z
        float rad = ShakeRot_Angle * Mathf.Deg2Rad;
        ShakeRot_Vec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * ShakeRot_Length;

        //����������
        ShakeRot_Length = Mathf.Lerp(ShakeRot_Length, 0f, ShakeRot_LowValue * Time.unscaledDeltaTime);

        //�X�v���C�g�̈ʒu�ɐU����������
        ShakeOffset = (Vector3)ShakeRot_Vec;
    }


    public void SetShakeSprite()
    {
        const float Length = 0.2f;
        const float Speed = 10.0f;
        const float LowValue = 10.0f;
        SetShakeSprite(Length, Speed, LowValue);
    }
    public void SetShakeSprite(float _Length, float _Speed, float _LowValue)
    {
        ShakeRot_Length = _Length;   //���a
        ShakeRot_Speed = _Speed;     //��]���x
        ShakeRot_LowValue = _LowValue;   //������
        ShakeRot_Angle = Random.Range(0f, 360f);    //�p�x�̓����_���ɃX�^�[�g
    }
}
