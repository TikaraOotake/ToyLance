using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Damaged : MonoBehaviour
{
    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;

    Animator anim;

    public GameManager gameManager;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("222222");
            OnDamaged(collision.transform.position);
        }
    }

    void OnDamaged(Vector2 targetPos)
    {


        //layer��ȭ
        gameObject.layer = 8;

        //�ǰݽ� ����ȭ
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);// 4��° ���ڴ� ����

        //�ǰݽ� ƨ�踮�׼� 
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 2);

    }
    void OffDamaged()
    {
        gameObject.layer = 7;

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
