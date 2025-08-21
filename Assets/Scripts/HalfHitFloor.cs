using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HalfHitFloor : MonoBehaviour
{
    [SerializeField]
    protected Collider2D FloorCollider; // ���ۂɏ�鑫��

    protected HashSet<Collider2D> ignoredSet = new HashSet<Collider2D>();
    private void Awake()
    {
        GameObject tempObj = Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player");
        SetIgnored(tempObj);
    }
    protected virtual void SetIgnored(GameObject _IgnoreObj)
    {
        if (_IgnoreObj != null)
        {
            if (ignoredSet.Contains(_IgnoreObj.GetComponent<Collider2D>())) return;

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
    void OnTriggerStay2D(Collider2D other)
    {
        // Unity���L�́udestroy�ς݂�����null����Ȃ��v���ɑΉ�
        if (other == null || other.gameObject == null) return;
        if (FloorCollider == null || FloorCollider.gameObject == null) return;
        if (ignoredSet == null || ignoredSet.Contains(other)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (!rb) return;

        // ����̒��S���W�Ǝ����̒��S���W���r
        Vector2 relativePos = other.bounds.center - FloorCollider.bounds.center;

        // �����̂������l
        bool isBelowOrSide = relativePos.y < -0.05f || Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);

        // ������ or ���܂��͉�����ڋ�
        if (rb.velocity.y <= 0 || isBelowOrSide)
        {
            Physics2D.IgnoreCollision(FloorCollider, other, true);
            ignoredSet.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (FloorCollider != null && other != null && ignoredSet != null && ignoredSet.Contains(other))
        {
            Physics2D.IgnoreCollision(FloorCollider, other, false);
            ignoredSet.Remove(other);
        }
    }
}