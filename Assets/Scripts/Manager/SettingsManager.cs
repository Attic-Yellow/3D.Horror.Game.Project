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
    [SerializeField] private AudioSource bgmAudioSource; // ��������� ���� AudioSource
    [SerializeField] private AudioSource sfxAudioSource; // ȿ������ ���� AudioSource
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

        // �����̴��� �ּڰ��� �ִ� ����
        if (lightIntensitySlider != null)
        {
            lightIntensitySlider.minValue = -2;
            lightIntensitySlider.maxValue = 0.5f;
        }

        LoadSettings();
    }

    void Start()
    {
        // Volume ������Ʈ���� Color Adjustments�� ����
        AdjustPostExposure(lightIntensitySlider.value);

    }

    // ������ �ε��ϰ� UI�� ������Ʈ
    public void LoadSettings()
    {
        float lightIntensityValue = GameManager.instance.GetLightIntensity();

        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f); // �⺻�� ����
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

    // ����� �ҷ� ����
    public void OnBGMVolumeChanged()
    {
        float bgmVolume = bgmVolumeSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        if (bgmAudioSource != null) bgmAudioSource.volume = bgmVolume; // ���� ����
        UpdateSettingsText();
    }

    // ȿ���� �ҷ� ����
    public void OnSFXVolumeChanged()
    {
        float sfxVolume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        // ��� SFX ����� �ҽ��� ���� ������ �����Ϸ���, sfxAudioSource ���
        // ��� ȿ���� ����� �ҽ��� �����ϰ� �����ؾ� ��
        // ���������� ���� sfxAudioSource�� ����
        if (sfxAudioSource != null) sfxAudioSource.volume = sfxVolume;
        UpdateSettingsText();
    }

    // ���� ���� ����
    public void OnLightIntensityChanged()
    {
        float newExposure = lightIntensitySlider.value;
        GameManager.instance.SetLightIntensity(newExposure);
        AdjustPostExposure(newExposure); // postExposure ���� �����ϴ� �޼��� ȣ��
        UpdateSettingsText();
    }

    // ���콺 ���� ����
    public void OnMouseSensitivityChanged()
    {
        float mouseSensitivity = mouseSensitivitySlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);

        var playerCameraView = FindObjectOfType<PlayerCameraView>(); // ���÷� FindObjectOfType ���
        if (playerCameraView != null)
        {
            playerCameraView.UpdateMouseSensitivity(mouseSensitivity);
        }
        UpdateSettingsText();
    }

    // ���� ���� ���� �Լ�
    public void AdjustPostExposure(float exposure)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = exposure;
        }
    }

    // ���� ���� ���� �ؽ�Ʈ�� ������Ʈ
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
            float displayLightIntensity = normalizedLightIntensity * 100; // 0 ~ 1 ������ 0 ~ 100���� �����ϸ�
            lightIntensityText.text = displayLightIntensity.ToString("0");
        }
    }
}
