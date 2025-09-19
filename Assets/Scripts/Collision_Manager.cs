using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Manager : MonoBehaviour
{
    /// <summary>
    /// 指定したコライダーに接触している、指定レイヤー名のオブジェクトを取得します。
    /// </summary>
    /// <param name="collider">対象のCollider2D</param>
    /// <param name="layerName">探すレイヤー名</param>
    /// <returns>該当レイヤーのGameObject（なければnull）</returns>
    public static GameObject GetTouchingObjectWithLayer(Collider2D collider, string layerName)
    {
        int targetLayer = LayerMask.NameToLayer(layerName);
        if (targetLayer == -1)
        {
            Debug.LogWarning($"指定されたレイヤー名「{layerName}」は存在しません。");
            return null;
        }

        // 一時的な配列（最大数を適当に確保）
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

    /// <summary>
    /// 指定した座標が、指定したCollider2Dの範囲内にあるかどうかを返す
    /// </summary>
    /// <param name="collider">判定対象のCollider2D</param>
    /// <param name="point">チェックしたい座標（ワールド座標）</param>
    /// <returns>範囲内ならtrue、そうでなければfalse</returns>
    public static bool IsPointInsideCollider(Collider2D collider, Vector2 point)
    {
        if (collider == null)
            return false;

        return collider.OverlapPoint(point);
    }

    /// <summary>
    /// 2つの Collider2D が接触していれば true を返す（Trigger も含む）
    /// </summary>
    public static bool AreCollidersTouchingAny(Collider2D colA, Collider2D colB)
    {
        if (colA == null || colB == null)
            return false;

        // ContactFilter2D を作成し、すべての条件を通すように設定
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; // Trigger も反応するように設定
        filter.SetLayerMask(Physics2D.DefaultRaycastLayers);
        filter.useLayerMask = true;

        return Physics2D.IsTouching(colA, colB, filter);
    }
}
