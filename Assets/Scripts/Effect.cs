using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private float deleteTimer;      //������܂ł̎���
    [SerializeField]
    private float angle;            //��юU�����(�p�x)
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float decaySpeed;       //��юU�������̌������x(0.0f�`1.0f)
    [SerializeField]
    private float MoveValue = 0.1f; //�ړ��l
    [SerializeField]
    private float min = 0.0f;       //�u��(�ŏ��l)
    [SerializeField]
    private float max = 360.0f;     //�u��(�ő�l)

    SpriteRenderer _sr;
    private float Alpha = 1.0f;     //�����x
    private float shakingDirection; //�u��
    float angleRadius;              //��юU�����(�x�N�g��)
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

        //�ړ�
        transform.Translate(new Vector2(directionVector.x * MoveValue, directionVector.y * MoveValue));

        //����
        transform.Translate(new Vector2(directionVector.x / decaySpeed, directionVector.y / decaySpeed));

        //�^�C�}�[�X�V
        deleteTimer = Mathf.Max(0.0f, deleteTimer - Time.deltaTime);

        //���X�ɓ�����
        Alpha -= Time.deltaTime;

        if (_sr)
        {
            _sr.color = new Color(1.0f, 1.0f, 1.0f, Alpha);
        }

        //���g�̍폜
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
