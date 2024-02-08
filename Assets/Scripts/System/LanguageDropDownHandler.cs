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

    // ���� �� �������� ��Ӵٿ��� ����
    void SetDropdownToCurrentLanguage()
    {
        LanguageType currentLanguage = GameManager.instance.GetCurrentLanguage();
        languageDropdown.value = (int)currentLanguage;
        languageDropdown.RefreshShownValue(); // ��Ӵٿ ���� ������ ��� �ݿ�
    }

    // ��Ӵٿ�� �� �������� �� ȣ��
    void LanguageDropdownChanged(TMP_Dropdown dropdown)
    {
        LanguageType selectedLanguage = (LanguageType)dropdown.value;
        languageManager.ChangeLanguage(selectedLanguage);
    }
}
