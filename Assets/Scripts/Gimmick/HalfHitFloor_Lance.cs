using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHitFloor_Lance : HalfHitFloor
{
    [SerializeField]
    private GameObject ReturnLancePrefab;//戻り槍のプレハブ
    [SerializeField]
    private GameObject CenterPos;//中央として扱うオブジェクト

    [SerializeField]
    private float RemainingTime = 1.0f;//残留時間
    private float RemainingTimer;//タイマー

    [SerializeField]
    private SpriteRenderer _sr;//スプライトレンダラーコンポ

    [SerializeField] bool IsFalling;//

    private void Awake()
    {
        GameObject tempObj = Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player");
        SetIgnored(tempObj);

        RemainingTimer = RemainingTime;//タイマーセット
    }

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            return;
        }


        RemainingTimer = Mathf.Max(0.0f, RemainingTimer - Time.deltaTime);
        
        if(_sr)
        {
            Color tempColor = _sr.color;
            if (RemainingTimer <= 1.0f)
            {
                if ((int)(RemainingTimer * 10.0f) % 2 == 0)
                {
                    tempColor = Color.black;
                }
                else
                {
                    tempColor = Color.white;
                }
                _sr.color = tempColor;
            }
        }

        if (RemainingTimer <= 0.0f) GenerateReturnLance();//戻り槍を生成

        if (FloorCollider != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(FloorCollider, "Player") && IsFalling)
            {
                transform.Translate(new Vector2(0.0f, -Time.deltaTime));
            }
        }
    }

    private void GenerateReturnLance()
    {
        if (ReturnLancePrefab != null)
        {
            GameObject Lance = Instantiate(ReturnLancePrefab, transform.position, Quaternion.identity);
            //Lance.transform.localScale = transform.localScale;//大きさを引継ぎ
            Lance.transform.eulerAngles = new Vector3(0.0f, 0.0f, transform.eulerAngles.y);

            if (CenterPos != null)
            {
                Lance.transform.position = CenterPos.transform.position;//座標を設定
            }

            Destroy(this.gameObject);
        }
    }

    /*protected override void SetIgnored(GameObject _IgnoreObj)
    {
        if (_IgnoreObj != null)
        {
            if (_IgnoreObj.layer == LayerMask.NameToLayer("PlayerAttack")) return;//投槍だったら省く

            Rigidbody2D rb = _IgnoreObj.GetComponent<Rigidbody2D>();
            if (!rb) return;

            // 相手の中心座標と自分の中心座標を比較
            Vector2 relativePos = _IgnoreObj.GetComponent<Collider2D>().bounds.center - FloorCollider.bounds.center;

            // 高さのしきい値（例: 自分より0.1以下の高さ or 水平方向に接近）
            bool isBelowOrSide = relativePos.y < -0.05f || Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);

            // 落下中 or 横または下から接近
            if (rb.velocity.y <= 0 || isBelowOrSide)
            {
                Physics2D.IgnoreCollision(FloorCollider, _IgnoreObj.GetComponent<Collider2D>(), true);
                ignoredSet.Add(_IgnoreObj.GetComponent<Collider2D>());
            }
        }
    }
     */


}
