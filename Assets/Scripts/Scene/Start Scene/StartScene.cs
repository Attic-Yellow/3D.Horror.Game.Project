using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Animator languageSelected;
    [SerializeField] private Animator lightIntensity;
    [SerializeField] private Animator characterSetting;
    [SerializeField] private Animator loadingOverlay;

    // 광량 설정에서 언어 설정으로 전환하는 함수
    public void BackLanguage()
    {   
        languageSelected.SetTrigger("isLanguage");
        lightIntensity.SetTrigger("isLanguage");
        characterSetting.SetTrigger("isLanguage");
        loadingOverlay.SetTrigger("isLanguage");
    }

    // 캐릭터 설정에서 광량 설정으로 전환하는 함수
    public void BackBright()
    {
        lightIntensity.SetTrigger("isBackBrightness");
        characterSetting.SetTrigger("isBackBrightness");
        loadingOverlay.SetTrigger("isBackBrightness");
    }

    // 언어 설정에서 광량 설정으로 전환하는 함수
    public void NextBright()
    {
        languageSelected.SetTrigger("isBrightness");
        lightIntensity.SetTrigger("isBrightness");
        characterSetting.SetTrigger("isBrightness");
        loadingOverlay.SetTrigger("isBrightness");
    }

    // 광량 설정에서 캐릭터 설정으로 전환하는 함수
    public void NextChacter()
    {
        lightIntensity.SetTrigger("isChacter");
        characterSetting.SetTrigger("isChacter");
        loadingOverlay.SetTrigger("isChacter");
    }

    // 캐릭터 설정에서 완료 버튼을 눌렀을 때 로딩 오버레이로 전환하는 함수
    public void Complete()
    {
        characterSetting.SetTrigger("isLoading");
        loadingOverlay.SetTrigger("isLoading");
    }
}
