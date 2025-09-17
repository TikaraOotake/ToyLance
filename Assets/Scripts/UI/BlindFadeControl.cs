using UnityEngine;
using UnityEngine.UI;

public class BlindFadeControl : MonoBehaviour
{
    [SerializeField]
    private bool FadeFlag;//true:暗転　false:明転

    [SerializeField]
    private float FadeSpeed = 1.0f;

    private float AlphaValue;//不透明度

    private Image _image;


    void Start()
    {
        _image = GetComponent<Image>();

        if (FadeFlag)
        {
            AlphaValue = 0.0f;//透明
        }
        else
        {
            AlphaValue = 1.0f;//真っ暗
        }

        //色を代入
        Color color = _image.color;
        _image.color = new Color(0.0f, 0.0f, 0.0f, AlphaValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeFlag)
        {
            AlphaValue += FadeSpeed * Time.deltaTime;
        }
        else
        {
            AlphaValue -= FadeSpeed * Time.deltaTime;
        }

        //上限下限を超えないように補正
        if (AlphaValue >= 1.0f)
        {
            AlphaValue = 1.0f;
        }
        else if (AlphaValue <= 0.0f)
        {
            AlphaValue = 0.0f;
        }

        //色を代入
        if(_image)
        {
            Color color = _image.color;
            _image.color = new Color(0.0f, 0.0f, 0.0f, AlphaValue);
        }
    }

    public void SetFadeFlag(bool _flag)
    {
        FadeFlag = _flag;
    }
    
}
