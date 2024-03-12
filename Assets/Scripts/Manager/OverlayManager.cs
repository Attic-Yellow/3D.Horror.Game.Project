using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class OverlayManager : MonoBehaviour
{
    [Header("��ŸƮ ��������")]
    [SerializeField] private GameObject LanguageSelectedOverlay;
    [SerializeField] private GameObject lightIntensityOverlay;
    [SerializeField] private GameObject[] chacterSettingOverlay;
    [SerializeField] private GameObject[] startOverlayCanvas;

    [Space(5)]
    [Header("Ŭ���� ����")]
    [SerializeField] private StartScene startScene;
    [SerializeField] private GameStart gameStart;
    [SerializeField] private SceneLoader sceneLoaderToGameScene;
    [SerializeField] private SceneLoader sceneLoaderToTitle;

    [Space(5)]

    [Header("�� ���� ��������")]
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject[] loadingOverlay;
    [SerializeField] private GameObject[] checkArea;

    [Header("���� �� ��������")]
    [SerializeField] private GameObject computerOverlay;
    [SerializeField] private GameObject crtOverlay;
    [SerializeField] private GameObject workListOverlay;
    [SerializeField] private GameObject screenTextOverlay;
    [SerializeField] private GameObject gameOverOverlay;
    [SerializeField] private GameObject excelPopupOverlay;

    private void Start()
    {
        GameManager.instance.overlayManager = this;

        if (SceneManager.GetActiveScene().name == "TitleScene" && chacterSettingOverlay != null)
        {
            chacterSettingOverlay[0].SetActive(false);
        }

        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(false);
        }

        if (loadingOverlay != null)
        {
            loadingOverlay[0].SetActive(false);
        }

        if (checkArea != null)
        {
            foreach (GameObject area in checkArea)
            {
                area.SetActive(false);
            }
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

        if(screenTextOverlay != null)
        {
            screenTextOverlay.SetActive(false);
        }

        if (gameOverOverlay != null)
        {
            gameOverOverlay.SetActive(false);
        }
    }

    public void StartCanvasController()
    {
        if (LanguageSelectedOverlay != null)
        {  
            LanguageSelectedOverlay.SetActive(false); 
        }

        if (lightIntensityOverlay != null)
        {
            lightIntensityOverlay.SetActive(false);
        }

        if (chacterSettingOverlay[1] != null)
        {
            chacterSettingOverlay[1].SetActive(false);
        }

        if (loadingOverlay[1] != null)
        {
            loadingOverlay[1].SetActive(false);
        }

        if (startOverlayCanvas[0] != null)
        {
            foreach (GameObject canvas in startOverlayCanvas)
            {
                canvas.SetActive(false);
            }
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
        sceneLoaderToTitle.LoadScene("TitleScene");
    }

    // �ɼ� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void OptionOverlayController()
    {
        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(!optionsOverlay.activeSelf);
            optionsOverlay.GetComponent<OptionController>().OnClickCloseButton();

            if (checkArea[0].activeSelf)
            {
                foreach (GameObject area in checkArea)
                {
                    area.SetActive(false);
                }
            }
        }
    }

    // Ȯ�� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void CheckAreaOverlayController(int areaNum)
    {
        checkArea[0].SetActive(!checkArea[0].activeSelf);
        checkArea[areaNum].SetActive(!checkArea[areaNum].activeSelf);
    }

    public void ScrenTextOverlayController()
    {
        if(screenTextOverlay != null)
        {
            screenTextOverlay.SetActive(!screenTextOverlay.activeSelf);
        }
    }

    // ĳ���� ���� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void ChacterSettingOverlayController()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay[0].SetActive(!chacterSettingOverlay[0].activeSelf);
        }
    }

    // ���� �����ϴ� ��ư
    public void StartButton()
    {
        gameStart.StartGameFun();
        StartCoroutine(LoadingOverlayController());
    }

    // ���� ������ϴ� ��ư
    public void RestartButton()
    {
        StartCoroutine(LoadingOverlayControllerRoReset());
    }

    // Ÿ��Ʋ ������ �Ѿ�� ��ư
    public void TitleButton()
    {
        StartCoroutine(LoadingOverlayControllerToTitle());
    }

    // ��ǻ�� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void ComputerOverlayController()
    {
        if (computerOverlay != null)
        {
            computerOverlay.SetActive(!computerOverlay.activeSelf);
        }
    }
    public void ExcelPopUpOverlayController()
    {
        if(excelPopupOverlay != null)
        {
            excelPopupOverlay.SetActive(false);
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

    // ���� ��� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void WorkListOverlayController()
    {
        if (workListOverlay != null)
        {
            workListOverlay.SetActive(!workListOverlay.activeSelf);
        }
    }

    // ���� ���� �������� Ȱ��ȭ / ��Ȱ��ȭ ��Ű�� �Լ�
    public void GameOverOverlayController()
    {
        if (gameOverOverlay != null)
        {
            gameOverOverlay.SetActive(!gameOverOverlay.activeSelf);
        }
    }

    // ��� �������� ��Ȱ��ȭ ��Ű�� �Լ�
    public void CloseButton()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay[0].SetActive(false);
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
            loadingOverlay[0].SetActive(true);
            sceneLoaderToGameScene.LoadScene("GameScene");
        }
    }

    // ���� ������ �Ѿ�� �Լ�
    private IEnumerator LoadingOverlayControllerRoReset()
    {
        if (loadingOverlay != null)
        {
            loadingOverlay[0].SetActive(true);
            sceneLoaderToGameScene.LoadScene("GameScene");
            yield return null;
        }
    }

    // Ÿ��Ʋ ������ �Ѿ�� �Լ�
    private IEnumerator LoadingOverlayControllerToTitle()
    {
        if (loadingOverlay != null)
        {
            loadingOverlay[0].SetActive(true);
            sceneLoaderToTitle.LoadScene("TitleScene");
            yield return null;
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

    // ĳ���� ���� �������̰� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���ϴ� �Լ�
    public bool CheckOnchacterSettingOverlay()
    {
        if (chacterSettingOverlay != null)
        {
            return chacterSettingOverlay[0].activeSelf;
        }
        return false;
    }
}