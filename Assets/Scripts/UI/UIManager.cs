using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Text_UI;
    [SerializeField]
    private GameObject HP_UI;

    [SerializeField]
    private GameObject Blind_UI;

    [SerializeField]
    private GameObject GameOver_UI;

    private void Awake()
    {
        
    }
    void Start()
    {
        Blind_UI = GameObject.Find("Blind");
        SetHP_UI(5);

        GameOver_UI = GameObject.Find("GameOver_UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetText_UI_String(string _string)
    {
        if (Text_UI)
        {
            Text text = Text_UI.GetComponent<Text>();
            if (text)
            {
                text.text = _string;
            }
        }
    }
    public void SetHP_UI(int _hp)
    {
        if (HP_UI)
        {
            Text text = HP_UI.GetComponent<Text>();
            if (text)
            {
                text.text = _hp.ToString();
            }
        }
    }
    public void SetHP_UI(float _HP, float _HP_Max)
    {
        if (HP_UI)
        {
            HP_Gauge_UI _HP_Gauge = HP_UI.GetComponent<HP_Gauge_UI>();
            if (_HP_Gauge != null)
            {
                _HP_Gauge.SetHP(_HP, _HP_Max);
            }
        }
    }

    public void SetBlindFade(bool _flag)
    {
        if(Blind_UI)
        {
            BlindFadeControl _blind= Blind_UI.GetComponent<BlindFadeControl>();
            if(_blind)
            {
                _blind.SetFadeFlag(_flag);
            }
        }
    }

    public void SetBlinking_UI(bool _flag)
    {
        if(Text_UI)
        {
            Blinking_UI blink = Text_UI.GetComponent<Blinking_UI>();
            if(blink)
            {
                blink.SetBlinking(_flag);
            }
        }
    }
    public void CallGameOver()
    {
        if (GameOver_UI != null)
        {
            GameOver_Controller gameover = GameOver_UI.GetComponent<GameOver_Controller>();
            if (gameover != null)
            {
                gameover.SetGameOver();
            }
        }
    }
}
