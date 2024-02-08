using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Animator languageSelected;
    [SerializeField] private Animator lightIntensity;
    [SerializeField] private Animator characterSetting;
    [SerializeField] private Animator loadingOverlay;

    // ���� �������� ��� �������� ��ȯ�ϴ� �Լ�
    public void BackLanguage()
    {   
        languageSelected.SetTrigger("isLanguage");
        lightIntensity.SetTrigger("isLanguage");
        characterSetting.SetTrigger("isLanguage");
        loadingOverlay.SetTrigger("isLanguage");
    }

    // ĳ���� �������� ���� �������� ��ȯ�ϴ� �Լ�
    public void BackBright()
    {
        lightIntensity.SetTrigger("isBackBrightness");
        characterSetting.SetTrigger("isBackBrightness");
        loadingOverlay.SetTrigger("isBackBrightness");
    }

    // ��� �������� ���� �������� ��ȯ�ϴ� �Լ�
    public void NextBright()
    {
        languageSelected.SetTrigger("isBrightness");
        lightIntensity.SetTrigger("isBrightness");
        characterSetting.SetTrigger("isBrightness");
        loadingOverlay.SetTrigger("isBrightness");
    }

    // ���� �������� ĳ���� �������� ��ȯ�ϴ� �Լ�
    public void NextChacter()
    {
        lightIntensity.SetTrigger("isChacter");
        characterSetting.SetTrigger("isChacter");
        loadingOverlay.SetTrigger("isChacter");
    }

    // ĳ���� �������� �Ϸ� ��ư�� ������ �� �ε� �������̷� ��ȯ�ϴ� �Լ�
    public void Complete()
    {
        characterSetting.SetTrigger("isLoading");
        loadingOverlay.SetTrigger("isLoading");
    }
}
