using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float checkRadius = 0.1f;
    public LayerMask groundMask;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public Collider2D groundCollider; // ���߰�: ���� ��� �ִ� ���� �ݶ��̴� ����

    void Update()
    {
        // ���� ��� �ִ� ���� �ݶ��̴� ������ groundCollider ������ ����
        groundCollider = Physics2D.OverlapCircle(transform.position, checkRadius, groundMask);
        // groundCollider�� null�� �ƴϸ� isGrounded�� true�� ��
        isGrounded = groundCollider != null;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
#endif
}
