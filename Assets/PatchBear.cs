using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatchBear : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject FlipObj;//�ړ��ɍ��킹�Ĕ��]������I�u�W�F�N�g

    [SerializeField]
    private float MoveValue;//�ړ����x
    [SerializeField]
    private int AngerLevel;//�{�背�x��
    [SerializeField]
    private bool ChaseFlag;//�ǐՃt���O
    [SerializeField]
    private float SearchLength;//���G�͈�

    private Vector2 BaseScale;

    [SerializeField]
    private float ActionTimer;//�s������

    private bool IsChase = false;

    [SerializeField]
    private float MoveWay = 0.0f;
    [SerializeField]
    private float SearchDistance;//Player��T������

    [SerializeField] Collider2D CliffCheckColl;//�R�[���m�F����R���C�_�[
    [SerializeField] Collider2D WallCheckColl;//�ǂ��m�F����R���C�_�[

    [SerializeField]
    private Rigidbody2D _rb;//�����R���|
    [SerializeField]
    private SpriteRenderer _sr;//�X�v���C�g�����_���[

    enum ActionStatus
    {
        Idol,//�ҋ@
        Walk,//���s
    }
    [SerializeField]
    private ActionStatus actionStatus;

    private void Awake()
    {
        BaseScale = transform.localScale;//BaseScale�ݒ�
    }
    void Start()
    {
        //�擾
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        SearchPlayer();

        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);

        SetAnim();
    }
    private void Walk()
    {
        if (CliffCheckColl != null) 
        {
            if (!Collision_Manager.GetTouchingObjectWithLayer(CliffCheckColl, "Platform"))
                MoveWay *= -1;//�ړ��������]
        }

        if (FlipObj != null)
        {
            if (MoveWay > 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
            else if (MoveWay < 0)
            {
                FlipObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        if (_sr != null)
        {
            if (MoveWay > 0)
            {
                _sr.flipX = true;
            }
            else if (MoveWay < 0)
            {
                _sr.flipX = false;
            }
        }


        if (_rb)
        {
            _rb.velocity = new Vector2(MoveWay * MoveValue, _rb.velocity.y);
        }
    }
    private void SearchPlayer()
    {
        Player = GameManager_01.GetPlayer();
        if (Player != null)
        {
            float Length = Vector2.Distance(Player.transform.position, transform.position);
            if (SearchLength <= Length)
            {
                //���G�͈͓��ɓ���ΒǐՃ��[�h��
                IsChase = true;
            }

            if (IsChase)
            {
                float Direction = Player.transform.position.x - transform.position.x;
                if (Direction >= 1.0f)
                {
                    MoveWay = +1.0f;
                }
                else if (Direction <= -1.0f)
                {
                    MoveWay = -1.0f;
                }
            }
        }
    }
    private void SetAction()
    {
        if (0.5f >= Random.Range(0.0f, 1.0f))
        {
            //�ҋ@
            MoveWay = 0.0f;
            actionStatus = ActionStatus.Idol;
        }
        else
        {
            //�ړ�
            if (0.5f >= Random.Range(0.0f, 1.0f))
            {
                //�E�ړ�
                MoveWay = +1.0f;
            }
            else
            {
                //���ړ�
                MoveWay = -1.0f;
            }

            actionStatus = ActionStatus.Walk;
        }

        //���ԃZ�b�g
        ActionTimer = Random.Range(1.0f, 3.0f);
    }
    private void SetAnim()
    {
    }
}
