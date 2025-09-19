using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour
{

    Rigidbody2D rigid;

    public int nextMove;

    Animator anim;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    private Enemy_shield enemyshield;                   //盾

    [SerializeField]
    private Enemy_shieldFlipper enemyshieldflipper;     //盾の反転

    [SerializeField]
    private float moveSpeed = 1f;                       //移動速度

    private EnemyHealth enemyHealth;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        Invoke("Think", 5);

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyHealth = GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));


        if (rayHit.collider == null && !spriteRenderer.flipY)
        {
            nextMove *= -1;
            spriteRenderer.flipX = nextMove == 1;
            //盾の更新処理
            UpdateShield();
            CancelInvoke();
            Invoke("Think", 2);

        }
    }

    void Think()
    {
        if (enemyHealth.GetHP() <= 0) 
        {
            return;
        }

        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
            //盾の更新処理
            UpdateShield();
        }

        float nextThinkTime = Random.Range(2f, 5f);

        Invoke("Think", nextThinkTime);
    }

    //盾の更新処理
    void UpdateShield()
    {
        if (enemyshield != null) 
        {
            bool nowFacingRight = !spriteRenderer.flipX;
            //盾の反転処理
            enemyshieldflipper.UpdateShieldPosition(nowFacingRight);
            //盾のアニメーションの再生処理
            enemyshield.SetWalkSpeed(nextMove);
        }
    }

    //停止処理
    public void PauseMovement(float duration)
    {
        CancelInvoke("Think");
        nextMove = 0;
        anim.SetInteger("WalkSpeed", 0);
        rigid.velocity=Vector2.zero;

        Invoke(nameof(ResumeMovement), duration);
    }

    //移動速度上昇処理
    private void ResumeMovement()
    {
        moveSpeed = 1.5f;
        Think();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
