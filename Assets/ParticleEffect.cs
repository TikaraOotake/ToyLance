using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public float ScatterAngle;       // ��юU��p�x�̊�i�x�j
    public float RandScatterAngle;   // �p�x�̃u�����i�}�x�j

    public float BaseSpeed;          // ���x�̊
    public float RandSpeed;          // ���x�̃u�����i�}�j
    public float GravityScale;       // �d�͉����x�X�P�[��

    public float DeleteTime = 1.0f;//�폜����
    private float DeleteTimer;

    private Vector2 SpeedVec;         // ���݂̑��x�x�N�g��
    private void Start()
    {
        // �p�x�������_���Ɍ���i�u�����t���j
        float angle = ScatterAngle + Random.Range(-RandScatterAngle * 0.5f, RandScatterAngle * 0.5f);
        float angleRad = angle * Mathf.Deg2Rad;

        // ���x�������_���Ɍ���i�u�����t���j
        float speed = BaseSpeed + Random.Range(-RandSpeed * 0.5f, RandSpeed * 0.5f);

        // �������x�x�N�g�����v�Z
        SpeedVec = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;

        //�^�C�}�[�ݒ�
        DeleteTimer = DeleteTime;
    }

    private void Update()
    {
        // �d�͉����x��K�p�iy�����ɉ����j
        SpeedVec += Vector2.down * GravityScale * Time.deltaTime;

        // ���݂̑��x�ňʒu���X�V
        //transform.position += (Vector3)(SpeedVec * Time.deltaTime); �I�u�W�F�N�g�̌������l��������ѕ������������ߕύX
        transform.Translate(SpeedVec * Time.deltaTime, Space.World);

        //�^�C�}�[�X�V
        DeleteTimer = Mathf.Max(0.0f, DeleteTimer - Time.deltaTime);

        //���Ԍo�߂Ŏ��g���폜
        if (DeleteTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
