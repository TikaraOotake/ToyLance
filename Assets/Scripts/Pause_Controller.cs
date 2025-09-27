using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Controller : MonoBehaviour
{
    private CanvasGroup _canvasGroup;       //親のCanvas

    [SerializeField]
    private Button continueButton;          //コンティニューボタン
    [SerializeField]
    private Button endButton;               //エンドボタン
    [SerializeField]
    private RectTransform LanceUIRect;      //槍の矢印のUIの位置

    private bool isPaused = false;          //ポーズ中フラグ

    void Start()
    {
        //親のCanvasを取得
        _canvasGroup = GetComponent<CanvasGroup>();
        //親のCanvasを非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //ボタンクリック時のイベントを登録
        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードPキーまたはコントローラーSTARTが押された時
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown("joystick button 7"))
        {
            //コンティニューボタンが選択されるように
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            //ポーズ状態
            TogglePause();
        }
        //槍の矢印のUIの位置
        MoveLanceToSelectedButton();
    }

    //ポーズ状態
    private void TogglePause()
    {
        //ポーズ状態なら
        if(isPaused)
        {
            //ゲームを再開
            ResumeGame();
        }
        //ポーズ状態じゃないなら
        else
        {
            //ポーズ
            SetPause();
        }
    }

    //ゲームを再開
    private void ResumeGame()
    {
        //通常時間
        Time.timeScale = 1.0f;

        //親のCanvasを非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //ポーズ状態じゃない
        isPaused = false;
    }

    //ポーズ
    private void SetPause()
    {
        //一時停止
        Time.timeScale = 0.0f;

        //親のCanvasを表示
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //ポーズ状態
        isPaused = true;
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
        //通常時間
        Time.timeScale = 1.0f;
        //シーン遷移
        SceneManager.LoadScene("Logo");
    }

    //コンティニューボタンクリック時のイベント
    private void OnContinueButtonPushed()
    {
        //通常時間
        Time.timeScale = 1.0f;

        //親のCanvasを非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
