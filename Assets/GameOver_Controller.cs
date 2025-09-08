using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Button endButton;
    [SerializeField]
    private Button continueButton;

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
            SetGameOver();
            Time.timeScale = 0.0f;
        }
    }
    public void SetGameOver()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void OnEndButtonPushed()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1.0f;
    }

    private void OnContinueButtonPushed()
    {
        GameManager_01.RespawnPlayer();

        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        Time.timeScale = 1.0f;
    }
}
