using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHitFloor : MonoBehaviour
{
    [SerializeField]
    private Collider2D FloorCollider; // ���ۂɏ�鑫��

    private HashSet<Collider2D> ignoredSet = new HashSet<Collider2D>();

    void OnTriggerStay2D(Collider2D other)
    {
        if (ignoredSet.Contains(other)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (!rb) return;

        // ����̒��S���W�Ǝ����̒��S���W���r
        Vector2 relativePos = other.bounds.center - FloorCollider.bounds.center;

        // �����̂������l�i��: �������0.1�ȉ��̍��� or ���������ɐڋ߁j
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
        if (ignoredSet.Contains(other))
        {
            Physics2D.IgnoreCollision(FloorCollider, other, false);
            ignoredSet.Remove(other);
        }
    }
}