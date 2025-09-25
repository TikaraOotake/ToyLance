using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Gauge_UI : MonoBehaviour
{
    [SerializeField] float HP = 1.0f;
    [SerializeField] float HP_Max = 1.0f;

    [SerializeField] Image _image;//イメージコンポーネント
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_image != null && HP_Max != 0.0f)
        {
            _image.fillAmount = HP / HP_Max;
        }
    }
    public void SetHP(float _HP,float _HP_Max)
    {
        HP = _HP;
        HP_Max = _HP_Max;
    }
}
