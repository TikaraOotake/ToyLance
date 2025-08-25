using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchBear : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

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

    [SerializeField]
    private float MoveWay = 0.0f;

    [SerializeField]
    private Rigidbody2D _rb;//�����R���|

    enum ActionStatus
    {
        Idol,//�ҋ@
        Walk,���s
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
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        if (ActionTimer <= 0.0f)
        {
            SetAction();
        }
        ActionTimer = Mathf.Max(0.0f, ActionTimer - Time.deltaTime);
    }
    private void Walk()
    {
        if (_rb)
        {
            _rb.velocity = new Vector2(MoveWay * MoveValue, _rb.velocity.y);
        }
    }
    private void SetAction()
    {
        float Rand = Random.Range(0.0f, 1.0f);
        if (Rand <= 0.33f)//�E�ړ�
        {
            MoveWay = 1.0f;
        }
        else if (Rand <= 0.66f)//���ړ�
        {
            MoveWay = -1.0f;
        }
        else//�ҋ@
        {
            MoveWay = 0.0f;
        }

        //���ԃZ�b�g
        ActionTimer = Random.Range(3.0f, 6.0f);
    }
}
