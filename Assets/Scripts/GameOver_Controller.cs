using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverUI;          //�e�̃I�u�W�F�N�g
    [SerializeField]
    private CanvasGroup endButtonUI;        //�G���h�̃{�^����UI
    [SerializeField]
    private CanvasGroup continueButtonUI;   //�R���e�B�j���[�̃{�^����UI
    [SerializeField]
    private CanvasGroup GameUI;             //Game�̕�����UI
    [SerializeField]
    private CanvasGroup OverUI;             //Over�̕�����UI
    [SerializeField]
    private CanvasGroup LanceUI;            //���̖���UI
    [SerializeField]
    private CanvasGroup ContinueUI;         //�R���e�B�j���[�̕�����UI
    [SerializeField]
    private Button endButton;               //�G���h�{�^��
    [SerializeField]
    private Button continueButton;          //�R���e�B�j���[�{�^��
    [SerializeField]
    private RectTransform GameUIRect;       //Game�̕�����UI�̈ʒu
    [SerializeField]
    private RectTransform OverUIRect;       //Over�̕�����UI�̈ʒu
    [SerializeField]
    private RectTransform LanceUIRect;      //���̖���UI�̈ʒu

    void Start()
    {
        //�e�̃I�u�W�F�N�g�͕\���@�q���͑S�Ĕ�\��
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

        //�{�^���N���b�N���̃C�x���g��o�^
        endButton.onClick.AddListener(OnEndButtonPushed);
        continueButton.onClick.AddListener(OnContinueButtonPushed);
    }

    // Update is called once per frame
    void Update()
    {
        //���̖���UI�̈ʒu
        MoveLanceToSelectedButton();
    }

    //�Q�[���I�[�o�[���̏���
    public void SetGameOver()
    {
        //�R���e�B�j���[�{�^�����I�������悤��
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

        //UI�����Ԃɕ\������R���[�`�����J�n
        StartCoroutine(GameOverSequence());
    }

    //UI�����Ԃɕ\������R���[�`��
    IEnumerator GameOverSequence()
    {
        //1.5�b�҂�
        yield return new WaitForSeconds(1.5f);
        //UI���ړ�������R���[�`�����J�n
        yield return StartCoroutine(FadeDrop(OverUI,OverUIRect));

        //�Q�[���I�[�o�[��
        SEManager.instance.PlaySE("gameover");

        //0.5�b�҂�
        yield return new WaitForSeconds(0.5f);
        //UI���ړ�������R���[�`�����J�n
        yield return StartCoroutine(FadeDrop(GameUI,GameUIRect));

        //0.5�b�҂�
        yield return new WaitForSeconds(0.5f);
        //UI��\��
        endButtonUI.alpha = 1.0f;
        endButtonUI.interactable = true;
        endButtonUI.blocksRaycasts = true;

        //UI��\��
        continueButtonUI.alpha = 1.0f;
        continueButtonUI.interactable = true;
        continueButtonUI.blocksRaycasts = true;

        //UI��\��
        LanceUI.alpha = 1.0f;

        //0.5�b�҂�
        //yield return new WaitForSeconds(0.5f);
        //UI��\��
        ContinueUI.alpha = 1.0f;
    }

    //UI���ړ�������R���[�`��
    IEnumerator FadeDrop(CanvasGroup cg, RectTransform rf)
    {
        float duration = 0.5f;
        float time = 0.0f;

        Vector2 startPos = rf.anchoredPosition + new Vector2(0, 600);   //�J�n�ʒu
        Vector2 endPos = rf.anchoredPosition;                           //�I���ʒu

        //UI�̈ʒu���J�n�ʒu��
        rf.anchoredPosition = startPos;
        //UI��\��
        cg.alpha = 1.0f;

        while (time < duration) 
        {
            float t = time / duration;
            float easedT = Mathf.SmoothStep(0, 1, t);

            //�J�n�ʒu����I���ʒu��
            rf.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            time += Time.deltaTime;
            yield return null;
        }

        //UI�̈ʒu���I���ʒu��
        rf.anchoredPosition = endPos;
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
        //�V�[���J��
        SceneManager.LoadScene("Logo");
    }

    //�R���e�B�j���[�{�^���N���b�N���̃C�x���g
    private void OnContinueButtonPushed()
    {
        //���X�|�[��
        GameManager_01.RespawnPlayer();

        //UI��S�Ĕ�\��
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
