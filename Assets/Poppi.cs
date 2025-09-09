using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppi : MonoBehaviour
{
    private GameObject Player;//�v���C���[���i�[

    [SerializeField] private float ActionTimer;//�s���^�C�}�[

    enum ActionStatus
    {
        Standby,//�ҋ@���
        Attacking,//�U�����
    }

    [SerializeField] private float FindLength = 1.0f;//�v���C���[�������鋗��
    void Start()
    {
        Player =  GameManager_01.GetPlayer();
    }


    void Update()
    {
        
    }

    private bool CheckPlayerOverlap()//�v���C���[�����͈͓��ɓ��������m�F
    {
        if (Player != null)
        {
            Vector2 PlayerPos = Player.transform.position;
            Vector2 ThisPos = transform.position;
            Vector2 Length = PlayerPos - ThisPos;//�������Z�o
            if (FindLength >= Length.magnitude)
            {
                return true;
            }
        }

        return false;
    }
}
