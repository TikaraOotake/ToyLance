using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHitFloor_Lance : HalfHitFloor
{
    [SerializeField]
    private float RemainingTime = 1.0f;//�c������
    private float RemainingTimer;//�^�C�}�[

    [SerializeField]
    private SpriteRenderer _sr;//�X�v���C�g�����_���[�R���|

    private void Awake()
    {
        GameObject tempObj = Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player");
        SetIgnored(tempObj);

        RemainingTimer = RemainingTime;//�^�C�}�[�Z�b�g
    }
    private void Update()
    {
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
        
        if (RemainingTimer <= 0.0f) Destroy(this.gameObject);//���g���폜
    }
    protected override void SetIgnored(GameObject _IgnoreObj)
    {
        if (_IgnoreObj != null)
        {
            if (_IgnoreObj.layer == LayerMask.NameToLayer("PlayerAttack")) return;//������������Ȃ�

            Rigidbody2D rb = _IgnoreObj.GetComponent<Rigidbody2D>();
            if (!rb) return;

            // ����̒��S���W�Ǝ����̒��S���W���r
            Vector2 relativePos = _IgnoreObj.GetComponent<Collider2D>().bounds.center - FloorCollider.bounds.center;

            // �����̂������l�i��: �������0.1�ȉ��̍��� or ���������ɐڋ߁j
            bool isBelowOrSide = relativePos.y < -0.05f || Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);

            // ������ or ���܂��͉�����ڋ�
            if (rb.velocity.y <= 0 || isBelowOrSide)
            {
                Physics2D.IgnoreCollision(FloorCollider, _IgnoreObj.GetComponent<Collider2D>(), true);
                ignoredSet.Add(_IgnoreObj.GetComponent<Collider2D>());
            }
        }
    }
}
