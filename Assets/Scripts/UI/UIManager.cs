using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject Text_UI;
    [SerializeField]
    GameObject HP_UI;

    [SerializeField]
    GameObject Blind_UI;
    void Start()
    {
        Blind_UI = GameObject.Find("Blind");
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
}
