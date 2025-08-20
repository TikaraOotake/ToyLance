using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHitFloor : MonoBehaviour
{
    [SerializeField]
    private Collider2D FloorCollider; // 実際に乗る足場

    private HashSet<Collider2D> ignoredSet = new HashSet<Collider2D>();

    void OnTriggerStay2D(Collider2D other)
    {
        if (ignoredSet.Contains(other)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (!rb) return;

        // 相手の中心座標と自分の中心座標を比較
        Vector2 relativePos = other.bounds.center - FloorCollider.bounds.center;

        // 高さのしきい値（例: 自分より0.1以下の高さ or 水平方向に接近）
        bool isBelowOrSide = relativePos.y < -0.05f || Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);

        // 落下中 or 横または下から接近
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