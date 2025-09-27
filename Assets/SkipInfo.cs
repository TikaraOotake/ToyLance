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
        //イメージコンポ取得
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

        //タイマー更新
        DisplayTimer = Mathf.Max(0.0f, DisplayTimer - Time.deltaTime);

        //表示タイマーが切れたら徐々に透明化
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
