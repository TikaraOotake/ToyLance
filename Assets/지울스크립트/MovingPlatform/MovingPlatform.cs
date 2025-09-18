using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("�̵� ���� ����")]
    [Tooltip("�÷����� �̵��� ������ �����Դϴ�.")]
    public Transform pointA;

    [Tooltip("�÷����� �̵��� ��ǥ �����Դϴ�.")]
    public Transform pointB;

    [Header("�÷��� ����")]
    [Tooltip("�÷����� �̵� �ӵ��Դϴ�.")]
    public float speed = 2.0f;

    // �ڡڡ� �߰��� �κ� �ڡڡ�
    [Header("�÷��̾� ���� ����")]
    [Tooltip("�÷��̾�� �ν��� ���̾ �������ּ���.")]
    public LayerMask playerLayer;
    // �ڡڡڡڡڡڡڡڡڡڡڡڡ�

    // --- ���� ���� ---
    private Transform currentTarget;
    private Transform currentlyCarryingPlayer = null; // ���� �¿�� �ִ� �÷��̾�
    private BoxCollider2D platformCollider; // ������ �ݶ��̴�

    void Start()
    {
        transform.position = pointA.position;
        currentTarget = pointB;
        platformCollider = GetComponent<BoxCollider2D>(); // ������ BoxCollider2D�� ã�ƿ�
    }

    void FixedUpdate()
    {
        // --- 1. �÷��� �̵� ���� (������ ����) ---
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
        }

        // --- 2. �÷��̾� ���� �� ž�� ó�� ���� (���ο� ���) ---
        DetectAndCarryPlayer();
    }

    // �ڡڡ� �ٽ� ����: ���� ���� Ȯ���Ͽ� �÷��̾ �¿�ų� ������ �� �ڡڡ�
    void DetectAndCarryPlayer()
    {
        // ������ ���� �߾ӿ��� ���� �ʺ�ŭ ���� ª�� ���� ����� ����(BoxCast)�� ��
        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f); // �ʺ�� ��¦ ����, ���̴� ª��
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        // ���� ���̰� �÷��̾�� �ε����ٸ�,
        if (hit.collider != null)
        {
            // ���� �� �÷��̾ �¿�� ���� �ʴٸ�,
            if (currentlyCarryingPlayer != hit.transform)
            {
                // �÷��̾ �ڽ����� ����� �¿�ϴ�.
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else // ���̿� �ƹ��͵� ���� �ʾҴٸ� (�÷��̾ ���ȴٸ�),
        {
            // ������ �¿�� �ִ� �÷��̾ �ִٸ�,
            if (currentlyCarryingPlayer != null)
            {
                // �ڽ� ���踦 �����Ͽ� �����Ӱ� ����ϴ�.
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }

    // OnCollisionEnter2D�� OnCollisionExit2D�� �� �̻� ������� �����Ƿ� �����մϴ�.
}