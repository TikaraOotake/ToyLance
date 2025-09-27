using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Controller : MonoBehaviour
{
    private CanvasGroup _canvasGroup;       //�e��Canvas

    [SerializeField]
    private Button continueButton;          //�R���e�B�j���[�{�^��
    [SerializeField]
    private Button endButton;               //�G���h�{�^��
    [SerializeField]
    private RectTransform LanceUIRect;      //���̖���UI�̈ʒu

    private bool isPaused = false;          //�|�[�Y���t���O

    void Start()
    {
        //�e��Canvas���擾
        _canvasGroup = GetComponent<CanvasGroup>();
        //�e��Canvas���\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //�{�^���N���b�N���̃C�x���g��o�^
        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[�{�[�hP�L�[�܂��̓R���g���[���[START�������ꂽ��
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown("joystick button 7"))
        {
            //�R���e�B�j���[�{�^�����I�������悤��
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            //�|�[�Y���
            TogglePause();
        }
        //���̖���UI�̈ʒu
        MoveLanceToSelectedButton();
    }

    //�|�[�Y���
    private void TogglePause()
    {
        //�|�[�Y��ԂȂ�
        if(isPaused)
        {
            //�Q�[�����ĊJ
            ResumeGame();
        }
        //�|�[�Y��Ԃ���Ȃ��Ȃ�
        else
        {
            //�|�[�Y
            SetPause();
        }
    }

    //�Q�[�����ĊJ
    private void ResumeGame()
    {
        //�ʏ펞��
        Time.timeScale = 1.0f;

        //�e��Canvas���\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //�|�[�Y��Ԃ���Ȃ�
        isPaused = false;
    }

    //�|�[�Y
    private void SetPause()
    {
        //�ꎞ��~
        Time.timeScale = 0.0f;

        //�e��Canvas��\��
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //�|�[�Y���
        isPaused = true;
    }

    //���̖���UI�̈ʒu
    private void MoveLanceToSelectedButton()
    {
        //���ݑI������Ă���{�^��
        var selected = EventSystem.current.currentSelectedGameObject;

        //�R���e�B�j���[�{�^��
        if (selected == continueButton.gameObject)
        {
            //���̖���UI�̈ʒu���R���e�B�j���[�{�^���̉���
            LanceUIRect.anchoredPosition = new Vector2(30, -300);
        }

        //�G���h�{�^��
        else if (selected == endButton.gameObject)
        {
            //���̖���UI�̈ʒu���G���h�{�^���̉���
            LanceUIRect.anchoredPosition = new Vector2(-680, -300);
        }
    }

    //�G���h�{�^���N���b�N���̃C�x���g
    private void OnEndButtonPushed()
    {
        //�ʏ펞��
        Time.timeScale = 1.0f;
        //�V�[���J��
        SceneManager.LoadScene("Logo");
    }

    //�R���e�B�j���[�{�^���N���b�N���̃C�x���g
    private void OnContinueButtonPushed()
    {
        //�ʏ펞��
        Time.timeScale = 1.0f;

        //�e��Canvas���\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
