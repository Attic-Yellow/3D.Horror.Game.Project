using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider lightIntensitySlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI lightIntensityText;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;
    [SerializeField] private AudioSource bgmAudioSource; // 배경음악을 위한 AudioSource
    [SerializeField] private List<AudioSource> sfxAudioSources = new(); // 효과음을 위한 AudioSource
    [SerializeField] private Volume globalVolume;
    [SerializeField] private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        GameManager.instance.settingsManager = this;
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.minValue = 1;
            mouseSensitivitySlider.maxValue = 300;
        }

        // 슬라이더의 최솟값과 최댓값 설정
        if (lightIntensitySlider != null)
        {
            lightIntensitySlider.minValue = -2;
            lightIntensitySlider.maxValue = 0.5f;
        }

        LoadSettings();
    }

    void Start()
    {
        // Volume 컴포넌트에서 Color Adjustments를 참조
        AdjustPostExposure(lightIntensitySlider.value);

    }

    // 설정을 로드하고 UI를 업데이트
    public void LoadSettings()
    {
        float lightIntensityValue = GameManager.instance.GetLightIntensity();

        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f); // 기본값 설정
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        }

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 100);
        }

        lightIntensitySlider.value = lightIntensityValue;
        UpdateSettingsText();
    }

    // 배경음 불륨 조절
    public void OnBGMVolumeChanged()
    {
        float bgmVolume = bgmVolumeSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        if (bgmAudioSource != null) bgmAudioSource.volume = bgmVolume; // 볼륨 조절
        UpdateSettingsText();
    }

    // 효과음 불륨 조절
    public void OnSFXVolumeChanged()
    {
        float sfxVolume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        // 모든 SFX 오디오 소스에 대해 볼륨을 조절하려면, sfxAudioSource 대신
        // 모든 효과음 오디오 소스를 참조하고 조절해야 함
        // 예제에서는 단일 sfxAudioSource만 조절
        foreach (AudioSource audio in sfxAudioSources)
        {
            if (audio != null)
            {
                audio.volume = sfxVolume;
            }
        }
        UpdateSettingsText();
    }

    // 조명 강도 조절
    public void OnLightIntensityChanged()
    {
        float newExposure = lightIntensitySlider.value;
        GameManager.instance.SetLightIntensity(newExposure);
        AdjustPostExposure(newExposure); // postExposure 값을 조절하는 메서드 호출
        UpdateSettingsText();
    }

    // 마우스 감도 조절
    public void OnMouseSensitivityChanged()
    {
        float mouseSensitivity = mouseSensitivitySlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);

        var playerCameraView = FindObjectOfType<PlayerCameraView>(); // 예시로 FindObjectOfType 사용
        if (playerCameraView != null)
        {
            playerCameraView.UpdateMouseSensitivity(mouseSensitivity);
        }
        UpdateSettingsText();
    }

    // 조명 강도 변경 함수
    public void AdjustPostExposure(float exposure)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = exposure;
        }
    }

    // 설정 값에 따라 텍스트를 업데이트
    private void UpdateSettingsText()
    {
        if (bgmVolumeText != null)
        {
            bgmVolumeText.text = (bgmVolumeSlider.value * 100).ToString("0");
        }

        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = (sfxVolumeSlider.value * 100).ToString("0");
        }

        if (mouseSensitivityText != null)
        {
            mouseSensitivityText.text = mouseSensitivitySlider.value.ToString("0");
        }

        if (lightIntensityText != null)
        {
            float normalizedLightIntensity = (lightIntensitySlider.value + 2) / 2.5f;
            float displayLightIntensity = normalizedLightIntensity * 100; // 0 ~ 1 범위를 0 ~ 100으로 스케일링
            lightIntensityText.text = displayLightIntensity.ToString("0");
        }
    }

    public void ChangeAudioClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void StopAudioSource(AudioSource source)
    {
        source.Stop();
    }

    public void AddSfxSourceList(AudioSource source)
    {
        sfxAudioSources.Add(source);
    }
}
