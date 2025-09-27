using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipInfo : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private float alpha = 0.0f;

    [SerializeField]
    private float DisplayTimer;
    void Start()
    {
        //�C���[�W�R���|�擾
        image = GetComponent<Image>();

        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            alpha = 1.0f;
            DisplayTimer = 3.0f;
        }
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        //�^�C�}�[�X�V
        DisplayTimer = Mathf.Max(0.0f, DisplayTimer - Time.deltaTime);

        //�\���^�C�}�[���؂ꂽ�珙�X�ɓ�����
        if (DisplayTimer <= 0.0f)
        {
            alpha = Mathf.Max(0.0f, alpha - Time.deltaTime);
        }


    }
    void OnAnyButtonClicked()
    {
        alpha = 1.0f;
        DisplayTimer = 3.0f;
    }
}
