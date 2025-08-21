using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HalfHitFloor : MonoBehaviour
{
    [SerializeField]
    protected Collider2D FloorCollider; // 実際に乗る足場

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

            // 相手の中心座標と自分の中心座標を比較
            Vector2 relativePos = _IgnoreObj.GetComponent<Collider2D>().bounds.center - FloorCollider.bounds.center;

            // 高さのしきい値（例: 自分より0.1以下の高さ or 水平方向に接近）
            bool isBelowOrSide = relativePos.y < -0.05f || Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);

            // 落下中 or 横または下から接近
            if (rb.velocity.y <= 0 || isBelowOrSide)
            {
                Physics2D.IgnoreCollision(FloorCollider, _IgnoreObj.GetComponent<Collider2D>(), true);
                ignoredSet.Add(_IgnoreObj.GetComponent<Collider2D>());
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Unity特有の「destroy済みだけどnullじゃない」問題に対応
        if (other == null || other.gameObject == null) return;
        if (FloorCollider == null || FloorCollider.gameObject == null) return;
        if (ignoredSet == null || ignoredSet.Contains(other)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (!rb) return;

        // 相手の中心座標と自分の中心座標を比較
        Vector2 relativePos = other.bounds.center - FloorCollider.bounds.center;

        // 高さのしきい値
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
        if (FloorCollider != null && other != null && ignoredSet != null && ignoredSet.Contains(other))
        {
            Physics2D.IgnoreCollision(FloorCollider, other, false);
            ignoredSet.Remove(other);
        }
    }
}