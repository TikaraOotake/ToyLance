using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Button endButton;
    [SerializeField]
    private Button continueButton;

    private GameObject Camera;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);

        Camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGameOver()
    {
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

        if (Camera)
        {
            UIManager _uiMng = Camera.GetComponent<UIManager>();
            if (_uiMng)
            {
                _uiMng.SetBlindFade(true);
            }
        }

        Invoke(nameof(DelayMethod), 1.5f);
    }

    private void DelayMethod()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void OnEndButtonPushed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }

    private void OnContinueButtonPushed()
    {
        Time.timeScale = 1.0f;

        GameManager_01.RespawnPlayer();

        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
