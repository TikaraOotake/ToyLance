using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeExtinction : MonoBehaviour
{
    [SerializeField]
    private float DeleteTime = 1.0f;//���Ŏ���
    private float DeleteTimer;

    SpriteRenderer _sr;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        DeleteTimer = DeleteTime;//�^�C�}�[�Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�X�V
        DeleteTimer = Mathf.Max(0.0f, DeleteTimer - Time.deltaTime);

        if (DeleteTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }

        if (_sr)
        {
            Color tempColor = _sr.color;
            if (DeleteTimer <= 1.0f)
            {
                if ((int)(DeleteTimer * 10.0f) % 2 == 0)
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
    }
}
