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
        // プレイヤーやRigidbodyを持つオブジェクトのみ対象
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && !rb.isKinematic)
        {
            // 床の移動分をプレイヤーに加える
            //rb.MovePosition(rb.position + velocity);
            collision.transform.Translate(velocity);
        }
    }
}
