using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_01_Control : MonoBehaviour
{
    [SerializeField]
    private float MoveValue = 1.0f;//�ړ���
    [SerializeField]
    private float JumpValue = 1.0f;//�W�����v��

    [SerializeField]
    private Rigidbody2D _rb;//�����R���|�[�l���g
    [SerializeField]
    private Animator _anim;//�A�j���[�^�[�R���|�[�l���g
    [SerializeField]
    private SpriteRenderer _sr;//SpriteRenderer�R���|�[�l���g


    [SerializeField]
    private Collider2D LandingCheckCollider;//���n�`�F�b�N�R���C�_�[
   

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.Log("���W�b�g�{�f�B�̎擾�Ɏ��s");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");

        _sr = GetComponent<SpriteRenderer>();
        if (_anim == null) Debug.Log("�A�j���[�^�[�̎擾�Ɏ��s");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

    }

    private void Move()
    {
        float MoveWay = 0.0f;
        if (Input.GetKey(KeyCode.D))
        {
            MoveWay = +1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveWay = -1.0f;
        }

        if (MoveWay != 0.0f)
        {
            if (_anim != null) _anim.SetBool("isWalk", true);
        }
        else
        {
            if (_anim != null) _anim.SetBool("isWalk", false);
        }

        //Sprite�̔��]
        if(_sr)
        {
            if(MoveWay>0.0f)//�E����
            {
                _sr.flipX = false;
            }
            else if(MoveWay<0.0f)//������
            {
                _sr.flipX = true;
            }
        }

        //�ړ��ʂ̑��
        if (_rb != null)
        {
            //�ړ��ʎ擾
            Vector2 MoveVelocity = _rb.velocity;
            MoveVelocity.x = MoveWay * MoveValue;
            _rb.velocity = MoveVelocity;//���
        }
    }
    private void Jump()
    {
        if (GetTouchingObjectWithLayer(LandingCheckCollider,"Platform"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //�ړ��ʎ擾
                Vector2 MoveVelocity = _rb.velocity;
                MoveVelocity.y = JumpValue;
                _rb.velocity = MoveVelocity;//���
            }
        }
    }

    /// <summary>
    /// �w�肵���R���C�_�[�ɐڐG���Ă���A�w�背�C���[���̃I�u�W�F�N�g���擾���܂��B
    /// </summary>
    /// <param name="collider">�Ώۂ�Collider2D</param>
    /// <param name="layerName">�T�����C���[��</param>
    /// <returns>�Y�����C���[��GameObject�i�Ȃ����null�j</returns>
    public static GameObject GetTouchingObjectWithLayer(Collider2D collider, string layerName)
    {
        int targetLayer = LayerMask.NameToLayer(layerName);
        if (targetLayer == -1)
        {
            Debug.LogWarning($"�w�肳�ꂽ���C���[���u{layerName}�v�͑��݂��܂���B");
            return null;
        }

        // �ꎞ�I�Ȕz��i�ő吔��K���Ɋm�ہj
        Collider2D[] results = new Collider2D[10];
        int count = collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);

        for (int i = 0; i < count; i++)
        {
            if (results[i] != null && results[i].gameObject.layer == targetLayer)
            {
                return results[i].gameObject;
            }
        }

        return null;
    }
}
