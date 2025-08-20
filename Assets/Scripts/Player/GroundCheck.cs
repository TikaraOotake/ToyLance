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
    public Collider2D groundCollider; // ★추가: 내가 밟고 있는 땅의 콜라이더 정보

    void Update()
    {
        // 내가 밟고 있는 땅의 콜라이더 정보를 groundCollider 변수에 저장
        groundCollider = Physics2D.OverlapCircle(transform.position, checkRadius, groundMask);
        // groundCollider가 null이 아니면 isGrounded는 true가 됨
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
