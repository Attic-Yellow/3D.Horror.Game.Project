using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class OverlayManager : MonoBehaviour
{
    [Header("스타트 씬 오버레이")]
    [SerializeField] private GameObject LanguageSelectedOverlay;
    [SerializeField] private GameObject lightIntensityOverlay;
    [SerializeField] private GameObject chacterSettingOverlay;

    [Space(5)]
    [Header("클래스 참조")]
    [SerializeField] private StartScene startScene;
    [SerializeField] private GameStart gameStart;
    [SerializeField] private SceneLoader sceneLoader;
    [Space(5)]

    [Header("타이틀 씬 오버레이")]
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject loadingOverlay;

    [Header("게임 씬 오버레이")]
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
        sceneLoader.LoadScene("TitleScene");
    }

    // 옵션 오버레이 활성화 / 비활성화 시키는 함수
    public void OptionOverlayController()
    {
        if (optionsOverlay != null)
        {
            optionsOverlay.SetActive(!optionsOverlay.activeSelf);
        }
    }

    // 캐릭터 설정 오버레이 활성화 / 비활성화 시키는 함수
    public void ChacterSettingOverlayController()
    {
        if (chacterSettingOverlay != null)
        {
            chacterSettingOverlay.SetActive(!chacterSettingOverlay.activeSelf);
        }
    }

    // 게임 시작하는 버튼
    public void StartButton()
    {
        gameStart.StartGameFun();
        StartCoroutine(LoadingOverlayController());

    }

    // 컴퓨터 오버레이 활성화 / 비활성화 시키는 함수
    public void ComputerOverlayController()
    {
        if (computerOverlay != null)
        {
            computerOverlay.SetActive(!computerOverlay.activeSelf);
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

    public void WorkListOverlayController()
    {
        if (workListOverlay != null)
        {
            workListOverlay.SetActive(!workListOverlay.activeSelf);
        }
    }

    // 모든 오버레이 비활성화 시키는 함수
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
            loadingOverlay.SetActive(true);
            sceneLoader.LoadScene("GameScene");
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

    public bool CheckOnchacterSettingOverlay()
    {
        if (chacterSettingOverlay != null)
        {
            return chacterSettingOverlay.activeSelf;
        }
        return false;
    }
}