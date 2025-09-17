using UnityEngine;
using UnityEngine.UI;

public class BlindFadeControl : MonoBehaviour
{
    [SerializeField]
    private bool FadeFlag;//true:�Ó]�@false:���]

    [SerializeField]
    private float FadeSpeed = 1.0f;

    private float AlphaValue;//�s�����x

    private Image _image;


    void Start()
    {
        _image = GetComponent<Image>();

        if (FadeFlag)
        {
            AlphaValue = 0.0f;//����
        }
        else
        {
            AlphaValue = 1.0f;//�^����
        }

        //�F����
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

        //��������𒴂��Ȃ��悤�ɕ␳
        if (AlphaValue >= 1.0f)
        {
            AlphaValue = 1.0f;
        }
        else if (AlphaValue <= 0.0f)
        {
            AlphaValue = 0.0f;
        }

        //�F����
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
