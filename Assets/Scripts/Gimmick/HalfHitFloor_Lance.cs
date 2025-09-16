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

    private void Awake()
    {
        GameObject tempObj = Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player");
        SetIgnored(tempObj);

        RemainingTimer = RemainingTime;//�^�C�}�[�Z�b�g
    }

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
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

        if (RemainingTimer <= 0.0f) GenerateReturnLance();//�߂葄�𐶐�

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

            Destroy(this.gameObject);
        }
    }

    /*protected override void SetIgnored(GameObject _IgnoreObj)
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
     */


}
