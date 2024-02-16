using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class OverlayManager : MonoBehaviour
{
    [Header("��ŸƮ �� ��������")]
    [SerializeField] private GameObject LanguageSelectedOverlay;
    [SerializeField] private GameObject lightIntensityOverlay;
    [SerializeField] private GameObject chacterSettingOverlay;
   
    [Space(5)]
    [Header("Ŭ���� ����")]
    [SerializeField] private StartScene startScene;
    [SerializeField] private GameStart gameStart;
    [SerializeField] private SceneLoader sceneLoader;
    [Space(5)]

    [Header("Ÿ��Ʋ �� ��������")]
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject loadingOverlay;

    [Header("���� �� ��������")]
    [SerializeField] private GameObject interactiveOverlay;

    private void Start()
    {
        GameManager.instance.overlayManager = this;

        if (SceneManager.GetActiveScene().name == "TitleScene" && chacterSettingOverlay != null)
        {
            chacterSettingOverlay.SetActive(false);
        }

        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(false);
        }

        if (loadingOverlay != null)
        {
            loadingOverlay.SetActive(false);
        }

        if (interactiveOverlay != null)
        {
            interactiveOverlay.SetActive(false);
        }
    }

    // ��� ���� �������̿��� ���� ���� �������̷� ��ȯ �ϴ� �Լ�
    public void GoToBright()
    {
        startScene.NextBright();
    }

    // ���� ���� �������̿��� ĳ���� ���� �������̷� ��ȯ �ϴ� �Լ�
    public void GoToChacter()
    {
        startScene.NextChacter();
    }

    // ���� ���� �������̿��� ��� ���� �������̷� ��ȯ �ϴ� �Լ�
    public void BackToLanguage()
    {
        startScene.BackLanguage();
    }

    // ĳ���� ���� �������̿��� ���� ���� �������̷� ��ȯ �ϴ� �Լ�
    public void BackToBright()
    {
        startScene.BackBright();
    }

    // Ÿ��Ʋ ������ �Ѿ�� �Լ�
    public void Complete()
    {
        startScene.Complete();
        sceneLoader.LoadScene("TitleScene");
    }

    // �ɼ� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void OptionOverlayController()
    {
        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(!optionsOverlay.activeSelf);
        }
    }

    // ĳ���� ���� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void ChacterSettingOverlayController()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay.SetActive(!chacterSettingOverlay.activeSelf);
        }
    }

    public void InteractiveOverlayController()
    {
        if (interactiveOverlay != null)
        {
            interactiveOverlay.SetActive(!interactiveOverlay.activeSelf);
        }
    }

    // ���� �����ϴ� ��ư
    public void StartButton()
    {
        gameStart.StartGameFun();
        StartCoroutine(LoadingOverlayController());
        
    }

    // ��� �������� ��Ȱ��ȭ ��Ű�� �Լ�
    public void CloseButton()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay.SetActive(false);
        }

        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(false);
        }
    }

    // ���� ����
    public void QuitButton()
    {
        Application.Quit();
    }

    private IEnumerator LoadingOverlayController()
    {
        if (loadingOverlay != null)
        {
            yield return new WaitForSeconds(2.8f);
            loadingOverlay.SetActive(true);
            sceneLoader.LoadScene("GameScene");
        }
    }
}
