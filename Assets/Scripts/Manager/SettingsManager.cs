using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;


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
    [SerializeField] private AudioSource[] sfxAudioSources; // ȿ������ ���� AudioSource
    [SerializeField] private AudioSource playerMoveAudioSouce;
    [SerializeField] private AudioClip[] moveClips;
    [SerializeField] private AudioClip[] sfxClips;

    [Header("Diplaye Settings")]
    [SerializeField] private UniversalRenderPipelineAsset urpAsset;
    [SerializeField] private TMP_Dropdown displayMode;
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TMP_Dropdown anti_Aliasing;
    [SerializeField] private TMP_Dropdown renderScale;
    [SerializeField] private TMP_Dropdown quality;

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
            lightIntensitySlider.minValue = 0;
            lightIntensitySlider.maxValue = 2f;
        }

        LoadSettings();
    }

    void Start()
    {
        // Volume ������Ʈ���� Color Adjustments�� ����
        AdjustPostExposure(lightIntensitySlider.value);
        PopulateResolutionDropdown();

        // �� TMP ����ٿ��� OnValueChanged �̺�Ʈ�� ������ �߰�
        resolution.onValueChanged.AddListener(delegate { OnResolutionChanged(resolution.value); });
        displayMode.onValueChanged.AddListener(OnDisplayModeChanged);
        anti_Aliasing.onValueChanged.AddListener(OnAntiAliasingChanged);
        renderScale.onValueChanged.AddListener(OnRenderScaleChanged);
        quality.onValueChanged.AddListener(OnQualityChanged);
    }

    public void PlayClip(int soundNum)
    {
        for(int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (sfxAudioSources[i].isPlaying && sfxAudioSources[i].clip == sfxClips[soundNum])
            {
                return;
            }
        }
       for(int i = 0;i < sfxAudioSources.Length; i++)
        {
           if(!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = sfxClips[soundNum];
                sfxAudioSources[i].Play();
                return;
            }
        }
    }

    public void PlayClip(AudioClip clip)
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = clip;
                sfxAudioSources[i].Play();
                return;
            }

        }
    }

 

    public void PlayMoveSFX(int num)
    {
       if(playerMoveAudioSouce.isPlaying)
        {
            if (playerMoveAudioSouce.clip != moveClips[num])
            {
                StopMoveSFX();
                playerMoveAudioSouce.clip = moveClips[num];
                playerMoveAudioSouce.Play();
            }
            else
            {
                return;
            }
        }
        else
        {
            playerMoveAudioSouce.clip = moveClips[num];
            playerMoveAudioSouce.Play();
        }
    }
   


    public void StopMoveSFX()
    {
        playerMoveAudioSouce.Stop();
       ResetPitch();
    }
    public void StopThisSFX(int _num)
    {
       for(int i = 0; i < sfxAudioSources.Length;i++)
        {
          if(sfxAudioSources[i].isPlaying && sfxAudioSources[i].clip == sfxClips[_num])
            {
                sfxAudioSources[i].Stop();
            }
        }

    }
    public void ChangeSouncePitch(float _value)
    {
        playerMoveAudioSouce.pitch = _value;
    }
   public void ResetPitch()
    {
        playerMoveAudioSouce.pitch = 1f;
    }
    // ������ �ε��ϰ� UI�� ������Ʈ

    public AudioClip GetAudioClip(int _num)
    {
        return sfxClips[_num];
    }
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

        // ���÷��� ��� �ε�
        if (displayMode != null)
        {
            int displayModeValue = PlayerPrefs.GetInt("DisplayMode", 0); // �⺻������ 0 ����
            displayMode.value = displayModeValue;
        }

        // �ػ� �ε��� �ε�
        if (resolution != null)
        {
            PopulateResolutionDropdown();
        }

        // ��Ƽ �ٸ���� ��� �ε�
        if (anti_Aliasing != null)
        {
            int antiAliasingMode = PlayerPrefs.GetInt("AntiAliasingMode", 1); // �⺻������ 1 ����
            anti_Aliasing.value = antiAliasingMode; // �ε��� ����
        }

        // ���� ������ ��� �ε�
        if (renderScale != null)
        {
            int renderScaleMode = PlayerPrefs.GetInt("RenderScale", 4); // �⺻������ 4 ����
            renderScale.value = renderScaleMode;
        }

        // ����Ƽ ��� �ε�
        if (quality != null)
        {
            int qualityMode = PlayerPrefs.GetInt("QualityMode", 1); // �⺻������ 1 ����
            quality.value = qualityMode;
        }

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
        foreach (AudioSource audio in sfxAudioSources)
        {
            if (audio != null)
            {
                audio.volume = sfxVolume;
            }
        }
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

    // ȭ�� ��� ���� �Լ�
    public void OnDisplayModeChanged(int mode)
    {
        switch (mode)
        {
            case 0: // ��ü ȭ��
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // â ���
                Screen.fullScreen = false;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // â���(��� ����)
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", mode);
    }

    // �ػ� ����ٿ� ���� �Ҵ� �Լ�
    private void PopulateResolutionDropdown()
    {
        if (resolution != null)
        {
            resolution.ClearOptions();
            List<string> options = new List<string>();
            Resolution[] resolutions = Screen.resolutions;
            int currentResolutionIndex = 0; // ���� ���õ� �ػ� �ε��� �ʱ�ȭ

            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution current = resolutions[i];
                string option = $"{current.width}x{current.height}";
                options.Add(option);

                // ����� �ػ󵵿� ��ġ�ϴ� ���, �ε��� ����
                if (current.width == Screen.currentResolution.width && current.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolution.AddOptions(options);

            // ����� �ػ� �ε��� �ҷ�����, ������ ���� �ػ� �ε��� ���
            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
            resolution.value = savedResolutionIndex >= 0 && savedResolutionIndex < options.Count ? savedResolutionIndex : currentResolutionIndex;
            resolution.RefreshShownValue();
        }
    }

    // �ػ� ���� �Լ�
    public void OnResolutionChanged(int resolutionIndex)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
        else
        {
            Debug.LogError("Invalid resolution index.");
        }

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    // ��Ƽ �ٸ���� ���� �Լ�
    public void OnAntiAliasingChanged(int mode)
    {
        if (urpAsset != null)
        {
            switch (mode)
            {
                case 0: // Disable
                    urpAsset.msaaSampleCount = 1;
                    GraphicsSettings.renderPipelineAsset = urpAsset;
                    break;
                case 1: // 2x
                    urpAsset.msaaSampleCount = 2;
                    GraphicsSettings.renderPipelineAsset = urpAsset;
                    break;
                case 2: // 4x
                    urpAsset.msaaSampleCount = 4;
                    GraphicsSettings.renderPipelineAsset = urpAsset;
                    break;
                case 3: // 8x
                    urpAsset.msaaSampleCount = 8;
                    GraphicsSettings.renderPipelineAsset = urpAsset;
                    break;
            }

            PlayerPrefs.SetInt("AntiAliasingMode", mode);
        }
    }

    public void OnRenderScaleChanged(int value)
    {
        float renderScaleValue = 1f; // �⺻���� 1f�� ����

        switch (value)
        {
            case 0:
                renderScaleValue = 0.25f;
                break;
            case 1:
                renderScaleValue = 0.5f;
                break;
            case 2:
                renderScaleValue = 0.75f;
                break;
            case 3:
                renderScaleValue = 1f;
                break;
            case 4:
                renderScaleValue = 1.25f;
                break;
            case 5:
                renderScaleValue = 1.5f;
                break;
            case 6:
                renderScaleValue = 1.75f;
                break;
            case 7:
                renderScaleValue = 2f;
                break;
            default:
                renderScaleValue = 1f; 
                break;
        }

        if (urpAsset != null)
        {
            urpAsset.renderScale = renderScaleValue;
            GraphicsSettings.renderPipelineAsset = urpAsset;
            PlayerPrefs.SetInt("RenderScale", value);
        }
    }

    // ����Ƽ ���� �Լ�
    public void OnQualityChanged(int mode)
    {
        if (mode == 0)
        {
            urpAsset.colorGradingMode = ColorGradingMode.LowDynamicRange;
        }
        else if (mode == 1)
        {
            urpAsset.colorGradingMode = ColorGradingMode.HighDynamicRange;
        }

        PlayerPrefs.SetInt("QualityMode", mode);
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
            float normalizedLightIntensity = (lightIntensitySlider.value) / 2f;
            float displayLightIntensity = normalizedLightIntensity * 100; // 0 ~ 1 ������ 0 ~ 100���� �����ϸ�
            lightIntensityText.text = displayLightIntensity.ToString("0");
        }
    }

  
}
