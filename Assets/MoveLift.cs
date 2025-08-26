using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLift : MonoBehaviour
{

    private Vector2 previousPosition;
    private Vector2 velocity;
    void Start()
    {
        previousPosition = transform.position;
    }
    private void Update()
    {
        velocity = new Vector2(transform.position.x, transform.position.y) - previousPosition;
        previousPosition = transform.position;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �v���C���[��Rigidbody�����I�u�W�F�N�g�̂ݑΏ�
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && !rb.isKinematic)
        {
            // ���̈ړ������v���C���[�ɉ�����
            //rb.MovePosition(rb.position + velocity);
            collision.transform.Translate(velocity);
        }
    }
}
