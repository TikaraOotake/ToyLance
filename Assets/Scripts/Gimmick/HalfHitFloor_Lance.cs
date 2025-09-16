using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHitFloor_Lance : HalfHitFloor
{
    [SerializeField]
    private GameObject ReturnLancePrefab;//�߂葄�̃v���n�u
    [SerializeField]
    private GameObject CenterPos;//�����Ƃ��Ĉ����I�u�W�F�N�g

    [SerializeField]
    private float RemainingTime = 1.0f;//�c������
    private float RemainingTimer;//�^�C�}�[

    [SerializeField]
    private SpriteRenderer _sr;//�X�v���C�g�����_���[�R���|

    [SerializeField] bool IsFalling;//

    private GameObject Player;

    private void Awake()
    {
        Player = GameManager_01.GetPlayer();//���擾

        GameObject tempObj = Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player");
        SetIgnored(tempObj);

        RemainingTimer = RemainingTime;//�^�C�}�[�Z�b�g
    }

    //�����X�g�������p������
    private void HandoverLance(GameObject _new)
    {
        if (Player != null)
        {
            Player_01_Control player = Player.GetComponent<Player_01_Control>();
            if (player != null)
            {
                player.HandoverLance(this.gameObject, _new);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //GenerateReturnLance();//�߂葄�𐶐�
            Destroy(this.gameObject);
            return;
        }


        RemainingTimer = Mathf.Max(0.0f, RemainingTimer - Time.deltaTime);
        
        if(_sr)
        {
            Color tempColor = _sr.color;
            if (RemainingTimer <= 1.0f)
            {
                if ((int)(RemainingTimer * 10.0f) % 2 == 0)
                {
                    tempColor = Color.black;
                }
                else
                {
                    tempColor = Color.white;
                }
                _sr.color = tempColor;
            }
        }

        if (RemainingTimer <= 0.0f)
        {
            //GenerateReturnLance();//�߂葄��
            Destroy(this.gameObject);
            return;
        } 

        if (FloorCollider != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player") && IsFalling)
            {
                transform.Translate(new Vector2(0.0f, -Time.deltaTime));
            }
        }
    }

    private void GenerateReturnLance()
    {
        if (ReturnLancePrefab != null)
        {
            GameObject Lance = Instantiate(ReturnLancePrefab, transform.position, Quaternion.identity);
            //Lance.transform.localScale = transform.localScale;//�傫�������p��
            Lance.transform.eulerAngles = new Vector3(0.0f, 0.0f, transform.eulerAngles.y);

            if (CenterPos != null)
            {
                Lance.transform.position = CenterPos.transform.position;//���W��ݒ�
            }

            HandoverLance(Lance);//�����X�g�̈��p��

            //Destroy(this.gameObject);
        }
    }
}
