using System;
using UnityEngine;
using UnityEngine.UI;

public class Blinking_UI : MonoBehaviour
{
    private Graphic uiElement;
    [SerializeField]
    private float blinkSpeed;
    private float timer;
    private bool isBlinking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiElement = GetComponent<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiElement != null)
        {
            timer += Time.deltaTime * blinkSpeed;
            float alpha=Mathf.Abs(Mathf.Sin(timer));

            Color color=uiElement.color;
            color.a = alpha;
            uiElement.color = color;
        }
    }

    public void SetBlinking(bool enable)
    {
        isBlinking = enable;

        if(!enable&&uiElement != null)
        {
            Color color= uiElement.color;
            color.a = 1f;
            uiElement.color = color;
        }
    }
}
