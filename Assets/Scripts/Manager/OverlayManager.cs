using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class OverlayManager : MonoBehaviour
{
    [Header("스타트 오버레이")]
    [SerializeField] private GameObject LanguageSelectedOverlay;
    [SerializeField] private GameObject lightIntensityOverlay;
    [SerializeField] private GameObject[] chacterSettingOverlay;
    [SerializeField] private GameObject[] startOverlayCanvas;

    [Space(5)]
    [Header("클래스 참조")]
    [SerializeField] private StartScene startScene;
    [SerializeField] private GameStart gameStart;
    [SerializeField] private SceneLoader sceneLoaderToGameScene;
    [SerializeField] private SceneLoader sceneLoaderToTitle;

    [Space(5)]

    [Header("씬 공용 오버레이")]
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject[] loadingOverlay;
    [SerializeField] private GameObject[] checkArea;

    [Header("게임 씬 오버레이")]
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

    // 언어 설정 오버레이에서 광량 설정 오버레이로 전환 하는 함수
    public void GoToBright()
    {
        startScene.NextBright();
    }

    // 광량 설정 오버레이에서 캐릭터 설정 오버레이로 전환 하는 함수
    public void GoToChacter()
    {
        startScene.NextChacter();
    }

    // 광량 설정 오버레이에서 언어 설정 오버레이로 전환 하는 함수
    public void BackToLanguage()
    {
        startScene.BackLanguage();
    }

    // 캐릭터 설정 오버레이에서 광량 설정 오버레이로 전환 하는 함수
    public void BackToBright()
    {
        startScene.BackBright();
    }

    // 타이틀 씬으로 넘어가는 함수
    public void Complete()
    {
        startScene.Complete();
        sceneLoaderToTitle.LoadScene("TitleScene");
    }

    // 옵션 오버레이 활성화 / 비활성화 시키는 함수
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

    // 확인 오버레이 활성화 / 비활성화 시키는 함수
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

    // 캐릭터 설정 오버레이 활성화 / 비활성화 시키는 함수
    public void ChacterSettingOverlayController()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay[0].SetActive(!chacterSettingOverlay[0].activeSelf);
        }
    }

    // 게임 시작하는 버튼
    public void StartButton()
    {
        gameStart.StartGameFun();
        StartCoroutine(LoadingOverlayController());
    }

    // 게임 재시작하는 버튼
    public void RestartButton()
    {
        StartCoroutine(LoadingOverlayControllerRoReset());
    }

    // 타이틀 씬으로 넘어가는 버튼
    public void TitleButton()
    {
        StartCoroutine(LoadingOverlayControllerToTitle());
    }

    // 컴퓨터 오버레이 활성화 / 비활성화 시키는 함수
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
    // CRT 오버레이 활성화 / 비활성화 시키는 함수
    public void CRTOverlayController()
    {
        if (crtOverlay != null)
        {
            crtOverlay.SetActive(!crtOverlay.activeSelf);
        }
    }

    // 업무 목록 오버레이 활성화 / 비활성화 시키는 함수
    public void WorkListOverlayController()
    {
        if (workListOverlay != null)
        {
            workListOverlay.SetActive(!workListOverlay.activeSelf);
        }
    }

    // 게임 오버 오버레이 활성화 / 비활성화 시키는 함수
    public void GameOverOverlayController()
    {
        if (gameOverOverlay != null)
        {
            gameOverOverlay.SetActive(!gameOverOverlay.activeSelf);
        }
    }

    // 모든 오버레이 비활성화 시키는 함수
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

    // 게임 종료
    public void QuitButton()
    {
        Application.Quit();
    }

    // 게임 씬으로 넘어가는 함수
    private IEnumerator LoadingOverlayController()
    {
        if (loadingOverlay != null)
        {
            yield return new WaitForSeconds(2.8f);
            loadingOverlay[0].SetActive(true);
            sceneLoaderToGameScene.LoadScene("GameScene");
        }
    }

    // 게임 씬으로 넘어가는 함수
    private IEnumerator LoadingOverlayControllerRoReset()
    {
        if (loadingOverlay != null)
        {
            loadingOverlay[0].SetActive(true);
            sceneLoaderToGameScene.LoadScene("GameScene");
            yield return null;
        }
    }

    // 타이틀 씬으로 넘어가는 함수
    private IEnumerator LoadingOverlayControllerToTitle()
    {
        if (loadingOverlay != null)
        {
            loadingOverlay[0].SetActive(true);
            sceneLoaderToTitle.LoadScene("TitleScene");
            yield return null;
        }
    }

    // 오버레이가 활성화되어 있는지 확인하는 함수
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

    // 캐릭터 설정 오버레이가 활성화되어 있는지 확인하는 함수
    public bool CheckOnchacterSettingOverlay()
    {
        if (chacterSettingOverlay != null)
        {
            return chacterSettingOverlay[0].activeSelf;
        }
        return false;
    }
}