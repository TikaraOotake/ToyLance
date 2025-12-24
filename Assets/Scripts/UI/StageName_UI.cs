using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageName_UI : MonoBehaviour
{
    private Image image;
    private Text text;
    private Outline outline;

    [SerializeField]
    private float alpha = 0.0f;

    [SerializeField]
    private float DisplayTimer = 6.0f;
    void Start()
    {
        //イメージコンポ取得
        image = GetComponent<Image>();
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();

        if (image != null) 
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        if(text != null || outline != null)
        {
            Color textcolor = text.color;
            textcolor.a = alpha;
            text.color = textcolor;

            Color outlinecolor = outline.effectColor;
            outlinecolor.a = alpha;
            outline.effectColor = outlinecolor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー更新
        DisplayTimer = Mathf.Max(0.0f, DisplayTimer - Time.deltaTime);

        if (image != null) 
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        if(text != null || outline != null)
        {
            Color textcolor = text.color;
            textcolor.a = alpha;
            text.color = textcolor;

            Color outlinecolor = outline.effectColor;
            outlinecolor.a = alpha;
            outline.effectColor = outlinecolor;
        }

        //表示タイマーが切れたら徐々に透明化
        if (DisplayTimer <= 0.0f)
        {
            alpha = Mathf.Max(0.0f, alpha - Time.deltaTime * 1.0f);
        }
        else if(DisplayTimer >= 4.0f)
        {
            alpha = Mathf.Min(1.0f, alpha + Time.deltaTime * 1.0f);
        }
    }
}
