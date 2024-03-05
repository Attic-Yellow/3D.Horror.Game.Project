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
    [SerializeField] private GameObject computerOverlay;
    [SerializeField] private GameObject crtOverlay;
    [SerializeField] private GameObject workListOverlay;

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

        if (computerOverlay != null)
        {
            computerOverlay.SetActive(false);
        }

        if (crtOverlay != null)
        {
            crtOverlay.SetActive(false);
        }

        if (workListOverlay != null)
        {
            workListOverlay.SetActive(false);
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

    // ���� �����ϴ� ��ư
    public void StartButton()
    {
        gameStart.StartGameFun();
        StartCoroutine(LoadingOverlayController());

    }

    // ��ǻ�� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void ComputerOverlayController()
    {
        if (computerOverlay != null)
        {
            computerOverlay.SetActive(!computerOverlay.activeSelf);
        }
    }

    // CRT �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void CRTOverlayController()
    {
        if (crtOverlay != null)
        {
            crtOverlay.SetActive(!crtOverlay.activeSelf);
        }
    }

    public void WorkListOverlayController()
    {
        if (workListOverlay != null)
        {
            workListOverlay.SetActive(!workListOverlay.activeSelf);
        }
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

    // ���� ������ �Ѿ�� �Լ�
    private IEnumerator LoadingOverlayController()
    {
        if (loadingOverlay != null)
        {
            yield return new WaitForSeconds(2.8f);
            loadingOverlay.SetActive(true);
            sceneLoader.LoadScene("GameScene");
        }
    }

    // �������̰� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���ϴ� �Լ�
    public bool CheckOnOverlay()
    {
        if (optionsOverlay != null && computerOverlay != null)
        {
            return optionsOverlay.activeSelf || computerOverlay.activeSelf;
        }
        else if (optionsOverlay != null && computerOverlay == null)
        {
            return optionsOverlay.activeSelf;
        }
        return false;
    }

    public bool CheckOnchacterSettingOverlay()
    {
        if (chacterSettingOverlay != null)
        {
            return chacterSettingOverlay.activeSelf;
        }
        return false;
    }
}