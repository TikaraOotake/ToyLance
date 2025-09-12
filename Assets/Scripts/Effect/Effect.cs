using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private float deleteTimer;      //消えるまでの時間
    [SerializeField]
    private float angle;            //飛び散る方向(角度)
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float decaySpeed;       //飛び散った時の減衰速度(0.0f～1.0f)
    [SerializeField]
    private float MoveValue = 0.1f; //移動値
    [SerializeField]
    private float min = 0.0f;       //ブレ(最小値)
    [SerializeField]
    private float max = 360.0f;     //ブレ(最大値)

    SpriteRenderer _sr;
    private float Alpha = 1.0f;     //透明度
    private float shakingDirection; //ブレ
    float angleRadius;              //飛び散る方向(ベクトル)
    float x;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        shakingDirection = Random.Range(min, max);
        angle += shakingDirection;
        angleRadius = angle * Mathf.Deg2Rad;
        x = Mathf.Cos(angleRadius);
        y = Mathf.Sin(angleRadius);

        if (!_sr)
        {
            _sr = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionVector = new Vector2(x, y);

        //移動
        transform.Translate(new Vector2(directionVector.x * MoveValue, directionVector.y * MoveValue));

        //減衰
        transform.Translate(new Vector2(directionVector.x / decaySpeed, directionVector.y / decaySpeed));

        //タイマー更新
        deleteTimer = Mathf.Max(0.0f, deleteTimer - Time.deltaTime);

        //徐々に透明に
        Alpha -= Time.deltaTime;

        if (_sr)
        {
            _sr.color = new Color(1.0f, 1.0f, 1.0f, Alpha);
        }

        //自身の削除
        if (deleteTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSprite(Sprite _sprite)
    {
        if (!_sprite)
        {
            return;
        }

        _sr = GetComponent<SpriteRenderer>();

        if (_sr)
        {
            _sr.sprite = _sprite;
        }
    }
}
