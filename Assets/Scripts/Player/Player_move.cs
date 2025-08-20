using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ★추가: 튀어오르는 종류를 구분하기 위한 열거형(Enum)
public enum BounceType
{
    Small,
    Large
}

public class Player_move : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 5f;
    public float jumpPower = 10f;

    [Header("Wall Stick & Slide Settings")]
    public float wallStickTime = 2.5f;
    public float wallSlideSpeed = 1.5f;
    public float wallJumpPower = 12f;

    [Header("Down Attack Settings")]
    public float downAttackSpeed = 20f;
    public float groundBounceForce = 8f;
    public float enemyBounceForce = 18f;

    [Header("Dependencies")]
    public GameManager gameManager;
    public Player_Attack playerAttackScript;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GroundCheck groundCheck;
    private CapsuleCollider2D capsuleCollider;
    private float horizontalInput;
    private bool jumpRequested;
    private bool isWallSticking;
    private float wallStickTimer;
    private float originalGravityScale;
    public bool isDownAttacking = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        originalGravityScale = rigid.gravityScale;
    }

    void Update()
    {
        if (isDownAttacking) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) { jumpRequested = true; }
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Melee") && !groundCheck.isGrounded && verticalInput < -0.8f) { StartDownAttack(); }
        else if (Input.GetAxisRaw("Vertical") < -0.8f && groundCheck.isGrounded) { TryDropDown(); }
        if (groundCheck.isGrounded) { isWallSticking = false; }
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        if (isDownAttacking) { rigid.velocity = new Vector2(0, -downAttackSpeed); }
        else if (isWallSticking) { HandleWallStickAndSlide(); }
        else { HandleMovement(); HandleJump(); }
    }

    private void StartDownAttack()
    {
        isDownAttacking = true;
        playerAttackScript.ActivateDownAttackHitbox();
        // ★추가: 애니메이터에 'isDownAttacking' 파라미터 값을 true로 설정하라는 신호를 보냄
        anim.SetBool("isDownAttacking", true);
    }

    public void EndDownAttackAndBounce(BounceType bounceType)
    {
        if (!isDownAttacking) return;
        isDownAttacking = false;
        playerAttackScript.DeactivateAttackHitbox();

        // ★추가: 애니메이터에 'isDownAttacking' 파라미터 값을 false로 설정하라는 신호를 보냄
        anim.SetBool("isDownAttacking", false);

        float bounceForce = 0;
        if (bounceType == BounceType.Small) { bounceForce = groundBounceForce; }
        else if (bounceType == BounceType.Large) { bounceForce = enemyBounceForce; }
        rigid.velocity = new Vector2(0, bounceForce);
    }

    public bool IsMovingOrJumping()
    {
        return !groundCheck.isGrounded || Mathf.Abs(rigid.velocity.x) > 0.1f;
    }

    private void UpdateVisuals()
    {
        if (!isWallSticking && !isDownAttacking)
        {
            if (horizontalInput != 0) spriteRenderer.flipX = horizontalInput < 0;
        }
        anim.SetBool("isWalk", Mathf.Abs(rigid.velocity.x) > 0.1f && !isDownAttacking);
        // ★수정: isJumping 애니메이션은 isDownAttacking 상태가 아닐 때만 재생되도록 함
        anim.SetBool("isJumping", !groundCheck.isGrounded && !isWallSticking && !isDownAttacking);
    }

    // --- 이하 다른 함수들은 변경 없음 ---
    public void InitiateWallStick(Transform wall, Vector2 hitPoint) { if (groundCheck.isGrounded) return; isWallSticking = true; wallStickTimer = wallStickTime; rigid.gravityScale = 0; rigid.velocity = Vector2.zero; spriteRenderer.flipX = wall.position.x < transform.position.x; }
    private void HandleWallStickAndSlide() { if (wallStickTimer > 0) { wallStickTimer -= Time.fixedDeltaTime; rigid.velocity = Vector2.zero; } else { rigid.velocity = new Vector2(0, -wallSlideSpeed); } jumpRequested = false; }
    private void HandleMovement() { rigid.gravityScale = originalGravityScale; rigid.velocity = new Vector2(horizontalInput * maxSpeed, rigid.velocity.y); }
    private void HandleJump() { if (jumpRequested && groundCheck.isGrounded) { rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); } jumpRequested = false; }
    void TryDropDown() { if (groundCheck.groundCollider != null && groundCheck.groundCollider.gameObject.layer == LayerMask.NameToLayer("SpearPlatform")) { StartCoroutine(DisableCollisionTemporarily(groundCheck.groundCollider)); } }
    IEnumerator DisableCollisionTemporarily(Collider2D platformCollider) { if (platformCollider != null && capsuleCollider != null) { Physics2D.IgnoreCollision(capsuleCollider, platformCollider, true); yield return new WaitForSeconds(0.5f); if (platformCollider != null) { Physics2D.IgnoreCollision(capsuleCollider, platformCollider, false); } } }
    public void OnDie() { if (capsuleCollider != null) capsuleCollider.enabled = false; spriteRenderer.color = new Color(1, 1, 1, 0.4f); rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse); spriteRenderer.flipY = true; enabled = false; }
}