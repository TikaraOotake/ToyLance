using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHP = 4;
    [SerializeField] protected float knockback = 10f;
    [SerializeField] private float iTime = 0.1f;   // ����E�ð�
    [SerializeField]
    protected int currentHP;
    protected int currentHP_old;
    protected Rigidbody2D rb;
    protected bool invincible;
    protected SEManager _seManager;

    [SerializeField]
    private float DeadTime = 1.0f;//���S����
    private float DeadTimer;//���S�^�C�}�[

    [SerializeField]
    private bool IsRespawn = true;//���A�\���̃t���O
    private Vector2 RestartPos;

    private int Layer;

    /* ����������������������������������������������������������������������������������
   ���߰� : �ǰ� ����E���濁ESpriteRenderer
    ���������������������������������������������������������������������������������� */
    private SpriteRenderer sr;                                   // ���߰�

    void Awake()
    {
        currentHP = maxHP;
        currentHP_old = currentHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();                     // ���߰�

        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SE�̎擾�Ɏ��s");

        RestartPos = transform.position;//�����l�����X�|�[�����W�Ƃ��ċL�^
    }

    /// <summary> �÷��̾�ۡ�� �¾��� �� ȣÁE</summary>
    public virtual void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (currentHP <= 0) return;
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP = Mathf.Max(0, currentHP - dmg);

        StartCoroutine(HitFlash());            // ���� �� ������E����E
        /* �� �˹�E���θ� ��ô/������ ����E���� �� */
        if (doKnockback)
        {
            float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
            Vector2 dir = new Vector2(side, 0f);       // ��ƁE�˹�E

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        //�_���[�W��
        _seManager.PlaySE("damage", transform.position);
    }

    public virtual void TakeDamage(int dmg, Vector2 attackerPos, Collider2D _coll, bool doKnockback = true)
    {
        TakeDamage(dmg, attackerPos, doKnockback);
    }

    public float GetDeadTimer()
    {
        return DeadTimer;
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
        //���S��
        _seManager.PlaySE("dead", transform.position);

        if (IsRespawn == false)//�������Ȃ��ݒ�̂��ߊ��S�ɍ폜����
        {
            Destroy(this.gameObject);
            return;
        }

        
            
        //���S��ԂƂ��ă}�l�[�W���[�ɓo�^
        Enemy_Manager.Instance.SetDeadEnemy(this.gameObject);

        // TODO: ��� �ִϡ���ƼŬ�����ھ�E
        //Destroy(gameObject);      // �ʿ��ϸ�E�ִ� �� Destroy(gameObject,0.3f);
        this.gameObject.SetActive(false);//�I�u�W�F�N�g���L����
    }

    public void SetHP(int _HP)
    {
        //�������
        _HP = Mathf.Max(_HP, 0);
        _HP = Mathf.Min(_HP, maxHP);
        
        currentHP = _HP;
    }
    public int GetHP()
    {
        return currentHP;
    }
    public int GetHP_old()
    {
        return currentHP_old;
    }

    public bool CheckRespawn()//���A�\���`�F�b�N
    {
        bool result = false;
        result = !(Collision_Manager.IsPointInsideCollider(Camera.main.GetComponent<Collider2D>(), RestartPos));
        if (result)
        {
            Debug.Log("���A���J�n");

            //���A����
            transform.position = RestartPos;//���W���Đݒ�
            currentHP = maxHP;//HP����
            this.gameObject.SetActive(true);//�I�u�W�F�N�g��L����
            //���C���[�ύX
            this.gameObject.layer = Layer;
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Health_Update();
    }

    protected void Health_Update()
    {
        //HP��0�ɂȂ����u��
        if (currentHP <= 0 && currentHP != currentHP_old)
        {
            //�^�C�}�[�ݒ�
            DeadTimer = DeadTime;

            //���C���[�ύX
            Layer = this.gameObject.layer;
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        //HP��0�ȉ��̂Ƃ��^�C�}�[�I���Ŋ��S�Ȏ��S�Ƃ��ď���
        if (currentHP <= 0 && DeadTimer <= 0.0f) Die();

        //�^�C�}�[�X�V
        DeadTimer = Mathf.Max(0.0f, DeadTimer - Time.deltaTime);

        //�O�̏�Ԃ��L�^
        currentHP_old = currentHP;
    }
}
