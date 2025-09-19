using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Manager : MonoBehaviour
{
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

    /// <summary>
    /// �w�肵�����W���A�w�肵��Collider2D�͈͓̔��ɂ��邩�ǂ�����Ԃ�
    /// </summary>
    /// <param name="collider">����Ώۂ�Collider2D</param>
    /// <param name="point">�`�F�b�N���������W�i���[���h���W�j</param>
    /// <returns>�͈͓��Ȃ�true�A�����łȂ����false</returns>
    public static bool IsPointInsideCollider(Collider2D collider, Vector2 point)
    {
        if (collider == null)
            return false;

        return collider.OverlapPoint(point);
    }

    /// <summary>
    /// 2�� Collider2D ���ڐG���Ă���� true ��Ԃ��iTrigger ���܂ށj
    /// </summary>
    public static bool AreCollidersTouchingAny(Collider2D colA, Collider2D colB)
    {
        if (colA == null || colB == null)
            return false;

        // ContactFilter2D ���쐬���A���ׂĂ̏�����ʂ��悤�ɐݒ�
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; // Trigger ����������悤�ɐݒ�
        filter.SetLayerMask(Physics2D.DefaultRaycastLayers);
        filter.useLayerMask = true;

        return Physics2D.IsTouching(colA, colB, filter);
    }
}
