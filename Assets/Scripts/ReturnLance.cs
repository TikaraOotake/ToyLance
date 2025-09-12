using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnLance : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;//��]���x
    [SerializeField] private GameObject SpriteBoard;//�X�v���C�g��\������q�I�u�W�F�N�g

    [SerializeField] private GameObject Player;//���𓊂���Player

    [SerializeField] private float MoveSpeed;//�ړ����x
    [SerializeField] private float AccValue = 1.0f;//�����x��

    [SerializeField] private float StopTimer = 1.0f;//��~����

    [SerializeField] private int AttackValue;
    void Start()
    {
        Player = GameManager_01.GetPlayer();//���擾
    }

    // Update is called once per frame
    void Update()
    {
        if (StopTimer <= 0.0f)//��~���Ԓ��łȂ��Ƃ�
        {
            //���x�ɉ����x��������
            MoveSpeed += AccValue * Time.deltaTime;

            if (Player != null)
            {
                //�ړ�
                Vector2 TargetVec = Player.transform.position - transform.position;//�ړ������v�Z
                float Length = TargetVec.magnitude;
                TargetVec.Normalize();//���K��
                transform.Translate(TargetVec * MoveSpeed * Time.deltaTime, Space.World);

                //�������m�F
                if (Length < MoveSpeed * Time.deltaTime)
                {
                    Destroy(this.gameObject);//�ړ����x�������𒴂�����폜
                }
            }


            //�X�v���C�g�{�[�h����]
            if (SpriteBoard != null)
            {
                SpriteBoard.transform.eulerAngles += new Vector3(0.0f, 0.0f, RotateSpeed) * Time.deltaTime;
            }
        }
        else
        {
            if (Player != null && SpriteBoard != null)
            {
                //�ړ�
                Vector2 TargetVec = Player.transform.position - transform.position;//�ړ������v�Z
                TargetVec.Normalize();//���K��

                float angleDeg = Mathf.Atan2(TargetVec.y, TargetVec.x) * Mathf.Rad2Deg;    // �x���@�ɕϊ�

                //SpriteBoard.transform.eulerAngles = new Vector3(0.0f, 0.0f, angleDeg + 90.0f);


                // ���݂�Z�p�x���擾
                float currentAngle = transform.eulerAngles.z;

                // deltaTime���g���ăt���[�����[�g�Ɉˑ����Ȃ���]���x��
                float step = RotateSpeed * 0.2f * Time.deltaTime;

                // �ŒZ�o�H�ŖڕW�p�x�֋߂Â��i360�x�̊����߂���l���j
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, angleDeg, step);

                // ��]��K�p�iEuler�p�Œ��ڐݒ�j
                transform.eulerAngles = new Vector3(0f, 0f, newAngle);
            }
        }

            //�^�C�}�[�X�V
            StopTimer = Mathf.Max(0.0f, StopTimer - Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BreakableObject _breakable = collision.GetComponent<BreakableObject>();
        if (_breakable != null)
        {
            _breakable.Break();
        }

        EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
        if (_enemyHealth != null)
        {
            _enemyHealth.TakeDamage(AttackValue, transform.position, true);
        }
    }
}
