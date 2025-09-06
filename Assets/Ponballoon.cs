using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Ponballoon : MonoBehaviour
{
    [SerializeField] private GameObject HangLift;//�݂艺���鑫��
    [SerializeField] private float AnchorRange;//�݂艺������

    [SerializeField] private int BalloonNam;//���D��
    [SerializeField] private int BalloonNam_Max = 1;//�ő啗�D��
    private int BalloonNam_old;


    [SerializeField] private float RevivalTime;//��������(0�̏ꍇ�͕����Ȃ�)
    [SerializeField] private float RevivalTimer;//�����^�C�}�[
    private float RevivalTimer_old = 0.0f;

    [SerializeField] private float MoveSpeed = 1.0f;//�ړ����x

    [SerializeField] private float TargetHeight;//�ڕW���W���x

    [SerializeField] private float[] SequenceHeight;//�i�K���x(����0�Ԗڂɂ�0)

    [SerializeField] private float ChaseRate;//�ǐ՗�

    [SerializeField] float BasePos;//��{���W

    Rigidbody2D _rb;

    private void Awake()
    {
        TargetHeight = transform.position.y;//���g�̍��W�ɃZ�b�g
        BasePos = transform.position.y;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();//�����R���|�擾

        if (_rb)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;//�d�͖�����
        }

        if (MoveSpeed <= 0) Debug.Log("���x��0�ȉ��ł��@����ȓ��삪�ł��܂���");

        BalloonNam = BalloonNam_Max;

        if (HangLift != null)
        {
            // �e�q�֌W�������iworldPositionStays = true �Ń��[���h���W��ێ��j
            HangLift.transform.SetParent(null, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�i�K���x�ɖڕW���x���ړ�
        int Index = BalloonNam - 1;
        if (Index >= 0 && Index < SequenceHeight.Length)//�z��O�`�F�b�N
        {
            if (!(MoveSpeed <= 0))//�ړ����x���傫��
            {
                if (TargetHeight != SequenceHeight[Index])//�i�K���x�ƖڕW���x���Ⴄ
                {
                    //�ړ����x�ȓ��ł���Έʒu�����킹�ďI��
                    if (SequenceHeight[Index] - MoveSpeed > TargetHeight ||
                        SequenceHeight[Index] + MoveSpeed < TargetHeight)
                    {
                        TargetHeight = SequenceHeight[Index];
                    }

                    else if (SequenceHeight[Index] > TargetHeight)//�i�K���x��菬�������
                    {
                        TargetHeight += MoveSpeed * Time.deltaTime;//���Z(�㏸)
                    }
                    else
                    {
                        TargetHeight -= MoveSpeed * Time.deltaTime;//���Z(���~)
                    }
                }
            }
        }

        if (BalloonNam <= 0)
        {
            //���D�������ꍇ
            if (_rb)
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;//�d�͗L����
            }
        }
        else
        {
            //�ڕW���x�Ɉړ�
            Vector2 pos = transform.position;
            pos.y -= BasePos;
            float rate = 1.0f - Mathf.Pow(1.0f - ChaseRate, Time.deltaTime * 60f); // �b��ChaseRate�ɂȂ�悤��
            pos.y += (TargetHeight - pos.y) * rate;

            //Nan�`�F�b�N
            if (!(pos.y >= 0.0f && pos.y <= 0.0f))
            {
                transform.position = pos + new Vector2(0.0f, BasePos);
            }

            if (_rb)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;//�d�͖�����
                _rb.velocity = Vector2.zero;//���x������
            }
        }



        //�^�C�}�[��0�ɂȂ����u��
        if (RevivalTimer <= 0.0f && RevivalTimer != RevivalTimer_old)
        {
            //���D�𕜊�
            BalloonNam_old = BalloonNam;
            BalloonNam = Mathf.Min(BalloonNam_Max, BalloonNam + 1);

            //0���畜�A������
            if (BalloonNam != BalloonNam_old && BalloonNam > 0)
            {
                //����W��������Ă�����ڕW���x�����ݍ��W�ɃZ�b�g
                if (BasePos > transform.position.y)
                {
                    TargetHeight = BasePos - transform.position.y;
                }
            }
        }

        //�^�C�}�[�Z�b�g
        if (RevivalTimer == 0.0f && BalloonNam < BalloonNam_Max)//���D���ő吔�ɖ������Ă��Ȃ��Ƃ�
        {
            RevivalTimer = RevivalTime;
        }

        //�^�C�}�[�X�V
        if (RevivalTime > 0)//0�̏ꍇ�̓^�C�}�[�X�V�Ȃ�
        {
            RevivalTimer_old = RevivalTimer;
            RevivalTimer = Mathf.Max(0.0f, RevivalTimer - Time.deltaTime);
        }

        //�݂艺���Ă��鑫��������񂹂�
        if (HangLift != null)
        {
            Vector2 LiftPos = HangLift.transform.position;
            Vector2 BalloonPos = transform.position;
            Vector2 Vec = BalloonPos - LiftPos;
            float Range = Vec.magnitude;//�������Z�o

            if (Range > AnchorRange &&//�݂艺��������������������
                Range != 0.0f)//0�ȊO�̎�
            {
                //������߂Â���
                Vec.Normalize();//���K��
                HangLift.transform.position = BalloonPos - Vec * AnchorRange;

                //���x��������
                Rigidbody2D _rb = HangLift.GetComponent<Rigidbody2D>();
                if (_rb) _rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            //���D������炷
            BalloonNam_old = BalloonNam;
            BalloonNam = Mathf.Max(0, BalloonNam - 1);

            //�����^�C�}�[�ݒ�
            RevivalTimer = RevivalTime;
        }
    }
}
