using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Controller : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button endButton;

    private bool isPaused = false;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown("joystick button 7"))
        {
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            SetPause();
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1.0f;

        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        isPaused = false;
    }
    
    private void SetPause()
    {
        Time.timeScale = 0.0f;

        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        isPaused = true;
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
