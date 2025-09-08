using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private int Sequence=0;//�i�K
    [SerializeField] private GameObject[] SequencePointObj;//�i�K���Ƃ̍��W������
    private List<GameObject> SequencePointObj_copy;

    [SerializeField] private GameObject SpriteBoard;//�摜��\������I�u�W�F�N�g
    [SerializeField] private GameObject Player;//Player

    [SerializeField] private float BoardShiftHeight;//�摜�����炷����

    [SerializeField] private float JumpValue;//�W�����v��

    [SerializeField] private float MigrationProgress;//�i�s�x�@0:�n�@1:�I

    [SerializeField] private float ProgressSpeed = 0.5f;//�i�s���x

    [SerializeField] private Vector2 DeparturePos;//�o�����W
    [SerializeField] private Vector2 TargetPos;//�ڕW���W

    [SerializeField] private float FindLength = 1.0f;//�v���C���[�������鋗��

    void Start()
    {
        Player = GameManager_01.GetPlayer();//�v���C���[�擾



        DeparturePos = transform.position;//�o�����W�ݒ�
        TargetPos = DeparturePos;//�O�̂��߉��㏑��

        if (Sequence < SequencePointObj.Length)//�z��O�`�F�b�N
        {
            GameObject TargetObj = SequencePointObj[Sequence];
            if (TargetObj != null)
            {
                TargetPos = TargetObj.transform.position;//�ڕW���W�ݒ�
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�i�s�x����摜�����炷�ʂ��v�Z
        BoardShiftHeight = Mathf.Sin(MigrationProgress * Mathf.PI);

        //�摜�����炷
        if (SpriteBoard != null)
        {
            SpriteBoard.transform.position = new Vector3(0.0f, BoardShiftHeight, 0.0f) + transform.position;
        }

        //�i�s�x������������߂�
        if (MigrationProgress >= 1.0f)
        {
            MigrationProgress = 0.0f;
        }

        //�s��ݒ�
        if (MigrationProgress == 0.0f)
        {
            DeparturePos = transform.position;//�o�����W�ݒ�

            //�v���C���[�������߂Â��Ă�����
            if (Player != null)
            {
                Vector2 PlayerPos = Player.transform.position;
                Vector2 ThisPos = transform.position;
                Vector2 Length = PlayerPos - ThisPos;//�������Z�o
                if (FindLength >= Length.magnitude)
                {
                    Debug.Log("�v���C���[���ߕt���܂���");

                    ++Sequence;//���̒i�K��

                    
                    TargetPos = DeparturePos;//�O�̂��߉��㏑��

                    if (Sequence < SequencePointObj.Length)//�z��O�`�F�b�N
                    {
                        GameObject TargetObj = SequencePointObj[Sequence];
                        if (TargetObj != null)
                        {
                            TargetPos = TargetObj.transform.position;//�ڕW���W�ݒ�
                        }
                    }
                }
            }
        }

        //�i�s�x�ɍ��킹�ăE�T�M���ړ�
        Vector2 TargetVec = TargetPos - DeparturePos;//�ڕW�܂ł̃x�N�g�����Z�o
        transform.position = TargetVec * MigrationProgress + DeparturePos;

        //�i�s�x�X�V
        MigrationProgress = Mathf.Min(1.0f, MigrationProgress + ProgressSpeed * Time.deltaTime);
    }

    private void OnValidate()
    {
        //�ŏ��̒i�K���W�͎��g�̍��W�ɂ���

        //�z��O�`�F�b�N
        if (0 < SequencePointObj.Length)
        {
            if (SequencePointObj[0] != null) return;

            SequencePointObj[0] = new GameObject();
            SequencePointObj[0].transform.position = transform.position;
            SequencePointObj[0].name = "TargetPoint";
        }
    }
}
