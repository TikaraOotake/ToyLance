using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageName_UI : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private float alpha = 0.0f;

    [SerializeField]
    private float DisplayTimer = 6.0f;
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
        //�^�C�}�[�X�V
        DisplayTimer = Mathf.Max(0.0f, DisplayTimer - Time.deltaTime);

        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        //�\���^�C�}�[���؂ꂽ�珙�X�ɓ�����
        if (DisplayTimer <= 0.0f)
        {
            alpha = Mathf.Max(0.0f, alpha - Time.deltaTime * 0.5f);
        }
        else if(DisplayTimer >= 4.0f)
        {
            alpha = Mathf.Min(1.0f, alpha + Time.deltaTime * 0.5f);
        }
    }
}
