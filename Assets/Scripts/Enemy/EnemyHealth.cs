using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHP = 4;
    [SerializeField] private float knockback = 10f;
    [SerializeField] private float iTime = 0.1f;   // ���� �ð�
    private int currentHP;
    private Rigidbody2D rb;
    private bool invincible;

    /* ����������������������������������������������������������������������������������
   ���߰� : �ǰ� ���� ����� SpriteRenderer
    ���������������������������������������������������������������������������������� */
    private SpriteRenderer sr;                                   // ���߰�

    void Awake()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();                     // ���߰�
    }

    /// <summary> �÷��̾�� �¾��� �� ȣ�� </summary>
    public void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP -= dmg;
        StartCoroutine(HitFlash());            // ���� �� ������ ����

        /* �� �˹� ���θ� ��ô/������ ���� ���� �� */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // ���� �˹�

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        if (currentHP <= 0) Die();

    }

    /* ����������������������������������������������������������������������������������
   ���߰� : ��������Ʈ�� ��� �������溹��
    ���������������������������������������������������������������������������������� */
    IEnumerator HitFlash()                                        // ���߰�
    {
        Color original = sr.color;        // ���� ���� ����
        sr.color = Color.red;             // ���������� ����
        yield return new WaitForSeconds(0.1f);
        sr.color = original;              // ���� ���� ����
    }

    IEnumerator IFrame()
    {
        invincible = true;
        yield return new WaitForSeconds(iTime);
        invincible = false;
    }

    void Die()
    {
        // TODO: ��� �ִϡ���ƼŬ�����ھ�
        Destroy(gameObject);      // �ʿ��ϸ� �ִ� �� Destroy(gameObject,0.3f);
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
