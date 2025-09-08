using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [Header("MoveSetting")]
    public float moveSpeed = 2f;

    [Tooltip("Total_Distance")]
    public float patrolRange = 10f; 

    private Vector3 leftPatrolPoint;
    private Vector3 rightPatrolPoint;
    private bool isMovingRight = true;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        leftPatrolPoint = transform.position;
        rightPatrolPoint = new Vector3(leftPatrolPoint.x + patrolRange, leftPatrolPoint.y, leftPatrolPoint.z);

        if (animator != null)
        {
            animator.SetBool("IsWalk", true);
        }

        FlipSprite();
    }

    void Update()
    {

        Vector3 targetPosition = isMovingRight ? rightPatrolPoint : leftPatrolPoint;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMovingRight = !isMovingRight;
            FlipSprite();
        }
    }

    void FlipSprite()
    {

        spriteRenderer.flipX = isMovingRight;
    }
}
