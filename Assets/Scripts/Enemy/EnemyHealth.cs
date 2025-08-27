using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHP = 4;
    [SerializeField] protected float knockback = 10f;
    [SerializeField] private float iTime = 0.1f;   // ����E�ð�
    protected int currentHP;
    protected Rigidbody2D rb;
    protected bool invincible;

    /* ����������������������������������������������������������������������������������
   ���߰� : �ǰ� ����E���濁ESpriteRenderer
    ���������������������������������������������������������������������������������� */
    private SpriteRenderer sr;                                   // ���߰�

    void Awake()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();                     // ���߰�
    }

    /// <summary> �÷��̾�ۡ�� �¾��� �� ȣÁE</summary>
    public virtual void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP -= dmg;
        StartCoroutine(HitFlash());            // ���� �� ������E����E
        /* �� �˹�E���θ� ��ô/������ ����E���� �� */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // ��ƁE�˹�E

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        if (currentHP <= 0) Die();

    }

    /* ����������������������������������������������������������������������������������
   ���߰� : ��������Ʈ�� �᱁E�������溹��
    ���������������������������������������������������������������������������������� */
    protected IEnumerator HitFlash()                                        // ���߰�
    {
        Color original = sr.color;        // ���� ����E����E
        sr.color = Color.red;             // ���������� ����E
        yield return new WaitForSeconds(0.1f);
        sr.color = original;              // ���� ����E����
    }

    protected IEnumerator IFrame()
    {
        invincible = true;
        yield return new WaitForSeconds(iTime);
        invincible = false;
    }

    protected void Die()
    {
        // TODO: ��� �ִϡ���ƼŬ�����ھ�E
        Destroy(gameObject);      // �ʿ��ϸ�E�ִ� �� Destroy(gameObject,0.3f);
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
