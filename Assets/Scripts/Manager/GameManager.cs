using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LanguageManager languageManager;
    public UIManager uiManager;
    public OverlayManager overlayManager;
    public SettingsManager settingsManager;

    [SerializeField] private LanguageType languageType;
    [SerializeField] private float lightIntensity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // PlayerPrefs.DeleteAll();
        languageType = (LanguageType)PlayerPrefs.GetInt("LanguageSetting", (int)LanguageType.English);
    }

    // ���� Ȱ��ȭ�� �� ����
    public void SetLanguageType(LanguageType SetLanguageType)
    {
        languageType = SetLanguageType;
        PlayerPrefs.SetInt("LanguageSetting", (int)languageType);
        PlayerPrefs.Save();
    }

    // ���� Ȱ��ȭ�� �� ��ȯ
    public LanguageType GetCurrentLanguage()
    {
        return languageType;
    }

    // ���� ������ ���� ������ ����
    public void SetLightIntensity(float intensity)
    {
        lightIntensity = intensity;
        PlayerPrefs.SetFloat("LightIntensity", lightIntensity);
        PlayerPrefs.Save();
    }

    // ����� ������ ��ȯ
    public float GetLightIntensity()
    {
        lightIntensity = PlayerPrefs.GetFloat("LightIntensity", 1f);
        return lightIntensity;
    }
}
