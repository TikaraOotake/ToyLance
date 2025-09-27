using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverUI;          //親のオブジェクト
    [SerializeField]
    private CanvasGroup endButtonUI;        //エンドのボタンのUI
    [SerializeField]
    private CanvasGroup continueButtonUI;   //コンティニューのボタンのUI
    [SerializeField]
    private CanvasGroup GameUI;             //Gameの文字のUI
    [SerializeField]
    private CanvasGroup OverUI;             //Overの文字のUI
    [SerializeField]
    private CanvasGroup LanceUI;            //槍の矢印のUI
    [SerializeField]
    private CanvasGroup ContinueUI;         //コンティニューの文字のUI
    [SerializeField]
    private Button endButton;               //エンドボタン
    [SerializeField]
    private Button continueButton;          //コンティニューボタン
    [SerializeField]
    private RectTransform GameUIRect;       //Gameの文字のUIの位置
    [SerializeField]
    private RectTransform OverUIRect;       //Overの文字のUIの位置
    [SerializeField]
    private RectTransform LanceUIRect;      //槍の矢印のUIの位置

    void Start()
    {
        //親のオブジェクトは表示　子供は全て非表示
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

        //ボタンクリック時のイベントを登録
        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        //槍の矢印のUIの位置
        MoveLanceToSelectedButton();
    }

    //ゲームオーバー時の処理
    public void SetGameOver()
    {
        //コンティニューボタンが選択されるように
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

        //UIを順番に表示するコルーチンを開始
        StartCoroutine(GameOverSequence());
    }

    //UIを順番に表示するコルーチン
    IEnumerator GameOverSequence()
    {
        //1.5秒待つ
        yield return new WaitForSeconds(1.5f);
        //UIを移動させるコルーチンを開始
        yield return StartCoroutine(FadeDrop(OverUI,OverUIRect));

        //ゲームオーバー音
        SEManager.instance.PlaySE("gameover");

        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);
        //UIを移動させるコルーチンを開始
        yield return StartCoroutine(FadeDrop(GameUI,GameUIRect));

        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);
        //UIを表示
        endButtonUI.alpha = 1.0f;
        endButtonUI.interactable = true;
        endButtonUI.blocksRaycasts = true;

        //UIを表示
        continueButtonUI.alpha = 1.0f;
        continueButtonUI.interactable = true;
        continueButtonUI.blocksRaycasts = true;

        //UIを表示
        LanceUI.alpha = 1.0f;

        //0.5秒待つ
        //yield return new WaitForSeconds(0.5f);
        //UIを表示
        ContinueUI.alpha = 1.0f;
    }

    //UIを移動させるコルーチン
    IEnumerator FadeDrop(CanvasGroup cg, RectTransform rf)
    {
        float duration = 0.5f;
        float time = 0.0f;

        Vector2 startPos = rf.anchoredPosition + new Vector2(0, 600);   //開始位置
        Vector2 endPos = rf.anchoredPosition;                           //終了位置

        //UIの位置を開始位置に
        rf.anchoredPosition = startPos;
        //UIを表示
        cg.alpha = 1.0f;

        while (time < duration) 
        {
            float t = time / duration;
            float easedT = Mathf.SmoothStep(0, 1, t);

            //開始位置から終了位置へ
            rf.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            time += Time.deltaTime;
            yield return null;
        }

        //UIの位置を終了位置に
        rf.anchoredPosition = endPos;
    }

    //槍の矢印のUIの位置
    private void MoveLanceToSelectedButton()
    {
        //現在選択されているボタン
        var selected = EventSystem.current.currentSelectedGameObject;

        //コンティニューボタン
        if (selected == continueButton.gameObject) 
        {
            //槍の矢印のUIの位置をコンティニューボタンの横に
            LanceUIRect.anchoredPosition = new Vector2(30, -300);
        }

        //エンドボタン
        else if (selected == endButton.gameObject)
        {
            //槍の矢印のUIの位置をエンドボタンの横に
            LanceUIRect.anchoredPosition = new Vector2(-680, -300);
        }
    }
    
    //エンドボタンクリック時のイベント
    private void OnEndButtonPushed()
    {
        //シーン遷移
        SceneManager.LoadScene("Logo");
    }

    //コンティニューボタンクリック時のイベント
    private void OnContinueButtonPushed()
    {
        //リスポーン
        GameManager_01.RespawnPlayer();

        //UIを全て非表示
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
