using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private CanvasGroup endButtonUI;
    [SerializeField]
    private CanvasGroup continueButtonUI;
    [SerializeField]
    private CanvasGroup GameUI;
    [SerializeField]
    private CanvasGroup OverUI;
    [SerializeField]
    private CanvasGroup LanceUI;
    [SerializeField]
    private CanvasGroup ContinueUI;
    [SerializeField]
    private Button endButton;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private RectTransform GameUIRect;
    [SerializeField]
    private RectTransform OverUIRect;
    [SerializeField]
    private RectTransform LanceUIRect;

    void Start()
    {
        GameOverUI.SetActive(true);
        endButtonUI.alpha = 0.0f;
        endButtonUI.interactable = false;
        endButtonUI.blocksRaycasts = false;
        continueButtonUI.alpha = 0.0f;
        continueButtonUI.interactable = false;
        continueButtonUI.blocksRaycasts= false;
        GameUI.alpha = 0.0f;
        OverUI.alpha = 0.0f;
        LanceUI.alpha = 0.0f;
        ContinueUI.alpha = 0.0f;

        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        MoveLanceToSelectedButton();
    }
    public void SetGameOver()
    {
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeDrop(OverUI,OverUIRect));

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeDrop(GameUI,GameUIRect));

        yield return new WaitForSeconds(0.5f);
        endButtonUI.alpha = 1.0f;
        endButtonUI.interactable = true;
        endButtonUI.blocksRaycasts = true;

        continueButtonUI.alpha = 1.0f;
        continueButtonUI.interactable = true;
        continueButtonUI.blocksRaycasts = true;

        LanceUI.alpha = 1.0f;

        yield return new WaitForSeconds(0.5f);
        ContinueUI.alpha = 1.0f;
    }

    IEnumerator FadeDrop(CanvasGroup cg, RectTransform rf)
    {
        float duration = 0.5f;
        float time = 0.0f;

        Vector2 startPos = rf.anchoredPosition + new Vector2(0, 600);
        Vector2 endPos = rf.anchoredPosition;

        rf.anchoredPosition = startPos;
        cg.alpha = 1.0f;

        while (time < duration) 
        {
            float t = time / duration;
            float easedT = Mathf.SmoothStep(0, 1, t);

            rf.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            time += Time.deltaTime;
            yield return null;
        }

        rf.anchoredPosition = endPos;
    }

    private void MoveLanceToSelectedButton()
    {
        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == continueButton.gameObject) 
        {
            LanceUIRect.anchoredPosition = new Vector2(30, -300);
        }

        if (selected == endButton.gameObject)
        {
            LanceUIRect.anchoredPosition = new Vector2(-680, -300);
        }
    }
    
    private void OnEndButtonPushed()
    {
        SceneManager.LoadScene("Title");
    }

    private void OnContinueButtonPushed()
    {
        GameManager_01.RespawnPlayer();

        endButtonUI.alpha = 0.0f;
        endButtonUI.interactable = false;
        endButtonUI.blocksRaycasts = false;
        continueButtonUI.alpha = 0.0f;
        continueButtonUI.interactable = false;
        continueButtonUI.blocksRaycasts = false;
        GameUI.alpha = 0.0f;
        OverUI.alpha = 0.0f;
        LanceUI.alpha = 0.0f;
        ContinueUI.alpha = 0.0f;
    }
}
