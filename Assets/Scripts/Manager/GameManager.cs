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
    public PoolingManager poolingManager;
    [SerializeField] private LanguageType languageType;
    [SerializeField] private float lightIntensity;
    [SerializeField] private bool isGameUsing = false;

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

    // 현재 활성화된 언어를 설정
    public void SetLanguageType(LanguageType SetLanguageType)
    {
        languageType = SetLanguageType;
        PlayerPrefs.SetInt("LanguageSetting", (int)languageType);
        PlayerPrefs.Save();
    }

    // 현재 활성화된 언어를 반환
    public LanguageType GetCurrentLanguage()
    {
        return languageType;
    }

    // 현재 광량을 저장 값으로 설정
    public void SetLightIntensity(float intensity)
    {
        lightIntensity = intensity;
        PlayerPrefs.SetFloat("LightIntensity", lightIntensity);
        PlayerPrefs.Save();
    }

    // 저장된 광량을 반환
    public float GetLightIntensity()
    {
        lightIntensity = PlayerPrefs.GetFloat("LightIntensity", 1f);
        return lightIntensity;
    }

    // 게임이 실행된 적이 있는지 확인
    private void SetGamePlayUsing()
    {
        isGameUsing = true;
        PlayerPrefs.SetInt("GameUsing", isGameUsing ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 게임이 실행된 적이 있는지 확인
    public bool GetGamePlayUsing()
    {
        isGameUsing = PlayerPrefs.GetInt("GameUsing", 0) == 1 ? true : false;
        return isGameUsing;
    }
}
