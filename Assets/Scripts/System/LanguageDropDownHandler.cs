using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageDropDownHandler : MonoBehaviour
{
    [SerializeField] private LanguageManager languageManager;
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        languageManager = GameManager.instance.languageManager;
        SetDropdownToCurrentLanguage();
        languageDropdown.onValueChanged.AddListener(delegate {
            LanguageDropdownChanged(languageDropdown);
        });
    }

    // 현재 언어를 기준으로 드롭다운을 설정
    void SetDropdownToCurrentLanguage()
    {
        LanguageType currentLanguage = GameManager.instance.GetCurrentLanguage();
        languageDropdown.value = (int)currentLanguage;
        languageDropdown.RefreshShownValue(); // 드롭다운에 변경 사항을 즉시 반영
    }

    // 드롭다운에서 언어를 변경했을 때 호출
    void LanguageDropdownChanged(TMP_Dropdown dropdown)
    {
        LanguageType selectedLanguage = (LanguageType)dropdown.value;
        languageManager.ChangeLanguage(selectedLanguage);
    }
}
