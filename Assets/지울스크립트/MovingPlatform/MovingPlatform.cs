using System.Drawing;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("ﾀﾌｵｿ ﾁ｡ ｼｳﾁ､")]
    [Tooltip("ﾇﾃｷｧﾆ釥ﾌ ﾀﾌｵｿﾀｻ ｽﾃﾀﾛﾇﾒ ﾁ｡ﾀﾔｴﾏｴﾙ.")]
    public Transform pointA;

    [Tooltip("ﾇﾃｷｧﾆ釥ﾌ ﾀﾌｵｿﾇﾒ ｸ･ ﾁ｡ﾀﾔｴﾏｴﾙ.")]
    public Transform pointB;

    [Header("ﾇﾃｷｧﾆ・ｼｳﾁ､")]
    [Tooltip("ﾇﾃｷｧﾆ釥ﾇ ﾀﾌｵｿ ｼﾓｵｵﾀﾔｴﾏｴﾙ.")]
    public float speed = 2.0f;

    // ｡ﾚ｡ﾚ｡ﾚ ﾃﾟｰ｡ｵﾈ ｺﾎｺﾐ ｡ﾚ｡ﾚ｡ﾚ
    [Header("ﾇﾃｷｹﾀﾌｾ・ｰｨﾁ・ｼｳﾁ､")]
    [Tooltip("ﾇﾃｷｹﾀﾌｾ﨧ﾎ ﾀﾎｽﾄﾇﾒ ｷｹﾀﾌｾ鋕ｦ ｼｱﾅﾃﾇﾘﾁﾖｼｼｿ・")]
    public LayerMask playerLayer;
    // ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ｡ﾚ

    // --- ｳｻｺﾎ ｺｯｼ・---
    private Transform currentTarget;
    private Transform currentlyCarryingPlayer = null; // ﾇ・ﾅﾂｿ・・ﾀﾖｴﾂ ﾇﾃｷｹﾀﾌｾ・
    private BoxCollider2D platformCollider; // ｹﾟﾆﾇﾀﾇ ﾄﾝｶﾌｴ・

    void Start()
    {
        if (pointA != null) transform.position = pointA.position;
        if (pointB != null) currentTarget = pointB;
        platformCollider = GetComponent<BoxCollider2D>(); // ｹﾟﾆﾇﾀﾇ BoxCollider2Dｸｦ ﾃ｣ｾﾆｿﾈ
    }

    void FixedUpdate()
    {
        if (currentTarget != null)
        {
            // --- 1. ﾇﾃｷｧﾆ・ﾀﾌｵｿ ｷﾎﾁ・(ｱ簔ｸｰ・ｵｿﾀﾏ) ---
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
            {
                currentTarget = (currentTarget == pointB) ? pointA : pointB;
            }
        }

        // --- 2. ﾇﾃｷｹﾀﾌｾ・ｰｨﾁ・ｹﾗ ﾅｾｽﾂ ﾃｳｸｮ ｷﾎﾁ・(ｻﾎｿ・ｹ貎ﾄ) ---
        DetectAndCarryPlayer();
    }

    // ｡ﾚ｡ﾚ｡ﾚ ﾇﾙｽﾉ ｷﾎﾁ・ ｹﾟﾆﾇ ﾀｧｸｦ ﾈｮﾀﾎﾇﾏｿｩ ﾇﾃｷｹﾀﾌｾ鋕ｦ ﾅﾂｿ・ﾅｳｪ ｳｻｸｮｰﾔ ﾇﾔ ｡ﾚ｡ﾚ｡ﾚ
    void DetectAndCarryPlayer()
    {
        if (platformCollider == null)
        {
            Debug.LogWarning("platformCollider is not assigned!");
            return;
        }

        // BoxCastの原点とサイズの計算
        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f);

        // RaycastHit2D を安全に取得
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        if (hit.collider != null)
        {
            // プレイヤーが現在キャリーされていない場合、または違うプレイヤーを持っている場合
            if (currentlyCarryingPlayer != hit.transform)
            {
                // プレイヤーを親オブジェクトに設定
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else
        {
            // プレイヤーが検出されていない場合
            if (currentlyCarryingPlayer != null)
            {
                // プレイヤーを親オブジェクトから解放
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }

    // OnCollisionEnter2Dｿﾍ OnCollisionExit2Dｴﾂ ｴ・ﾀﾌｻ・ｻ鄙・ﾏﾁ・ｾﾊﾀｸｹﾇｷﾎ ｻ霖ｦﾇﾕｴﾏｴﾙ.
}