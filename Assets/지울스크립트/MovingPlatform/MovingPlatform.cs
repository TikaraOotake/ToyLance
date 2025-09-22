using System.Drawing;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("�̵� ���� ����")]
    [Tooltip("�÷����� �̵��� ������ �����Դϴ�.")]
    public Transform pointA;

    [Tooltip("�÷����� �̵��� ��ǥ �����Դϴ�.")]
    public Transform pointB;

    [Header("�÷�ƁE����")]
    [Tooltip("�÷����� �̵� �ӵ��Դϴ�.")]
    public float speed = 2.0f;

    // �ڡڡ� �߰��� �κ� �ڡڡ�
    [Header("�÷��̾�E����E����")]
    [Tooltip("�÷��̾��� �ν��� ���̾�Ԧ �������ּ���E")]
    public LayerMask playerLayer;
    // �ڡڡڡڡڡڡڡڡڡڡڡڡ�

    // --- ���� ����E---
    private Transform currentTarget;
    private Transform currentlyCarryingPlayer = null; // ����E�¿�E�E�ִ� �÷��̾�E
    private BoxCollider2D platformCollider; // ������ �ݶ��̴�E

    void Start()
    {
        if (pointA != null) transform.position = pointA.position;
        if (pointB != null) currentTarget = pointB;
        platformCollider = GetComponent<BoxCollider2D>(); // ������ BoxCollider2D�� ã�ƿ�
    }

    void FixedUpdate()
    {
        if (currentTarget != null)
        {
            // --- 1. �÷�ƁE�̵� ����E(������E����) ---
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
            {
                currentTarget = (currentTarget == pointB) ? pointA : pointB;
            }
        }

        // --- 2. �÷��̾�E����E�� ž�� ó�� ����E(���ο�E���) ---
        DetectAndCarryPlayer();
    }

    // �ڡڡ� �ٽ� ����E ���� ���� Ȯ���Ͽ� �÷��̾�Ԧ �¿�Eų� ������ �� �ڡڡ�
    void DetectAndCarryPlayer()
    {
        if (platformCollider == null)
        {
            Debug.LogWarning("platformCollider is not assigned!");
            return;
        }

        // BoxCast�̌��_�ƃT�C�Y�̌v�Z
        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f);

        // RaycastHit2D �����S�Ɏ擾
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        if (hit.collider != null)
        {
            // �v���C���[�����݃L�����[����Ă��Ȃ��ꍇ�A�܂��͈Ⴄ�v���C���[�������Ă���ꍇ
            if (currentlyCarryingPlayer != hit.transform)
            {
                // �v���C���[��e�I�u�W�F�N�g�ɐݒ�
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else
        {
            // �v���C���[�����o����Ă��Ȃ��ꍇ
            if (currentlyCarryingPlayer != null)
            {
                // �v���C���[��e�I�u�W�F�N�g������
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }

    // OnCollisionEnter2D�� OnCollisionExit2D�� ��E�̻�E�翁E���E�����Ƿ� �����մϴ�.
}