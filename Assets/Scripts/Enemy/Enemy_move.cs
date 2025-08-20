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
    private Transform shield;
    private float initialShieldX;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        Invoke("Think", 5);

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        initialShieldX = shield.localPosition.x;
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));


        if (rayHit.collider == null && !spriteRenderer.flipY)
        {
            nextMove *= -1;
            spriteRenderer.flipX = nextMove == 1;
            UpdateShieldPosition();
            CancelInvoke();
            Invoke("Think", 2);

        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
            UpdateShieldPosition();
        }
            
        float nextThinkTime = Random.Range(2f, 5f);

        Invoke("Think", nextThinkTime);
    }

    void UpdateShieldPosition()
    {
        if (shield == null) 
        {
            return;
        }

        float direction;

        if (spriteRenderer.flipX)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }

        Vector3 pos = shield.localPosition;
        pos.x = initialShieldX * direction;
        shield.localPosition = pos;
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
