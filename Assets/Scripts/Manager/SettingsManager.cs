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
    [SerializeField] private AudioSource bgmAudioSource; // 배경음악을 위한 AudioSource
    [SerializeField] private AudioSource[] sfxAudioSources; // 효과음을 위한 AudioSource
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

        // 슬라이더의 최솟값과 최댓값 설정
        if (lightIntensitySlider != null)
        {
            lightIntensitySlider.minValue = 0;
            lightIntensitySlider.maxValue = 2f;
        }

        LoadSettings();
    }

    void Start()
    {
        // Volume 컴포넌트에서 Color Adjustments를 참조
        AdjustPostExposure(lightIntensitySlider.value);
        PopulateResolutionDropdown();

        // 각 TMP 드랍다운의 OnValueChanged 이벤트에 리스너 추가
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
    // 설정을 로드하고 UI를 업데이트

    public AudioClip GetAudioClip(int _num)
    {
        return sfxClips[_num];
    }
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

        // 디스플레이 모드 로드
        if (displayMode != null)
        {
            int displayModeValue = PlayerPrefs.GetInt("DisplayMode", 0); // 기본값으로 0 설정
            displayMode.value = displayModeValue;
        }

        // 해상도 인덱스 로드
        if (resolution != null)
        {
            PopulateResolutionDropdown();
        }

        // 안티 앨리어싱 모드 로드
        if (anti_Aliasing != null)
        {
            int antiAliasingMode = PlayerPrefs.GetInt("AntiAliasingMode", 1); // 기본값으로 1 설정
            anti_Aliasing.value = antiAliasingMode; // 인덱스 조정
        }

        // 렌더 스케일 모드 로드
        if (renderScale != null)
        {
            int renderScaleMode = PlayerPrefs.GetInt("RenderScale", 4); // 기본값으로 4 설정
            renderScale.value = renderScaleMode;
        }

        // 퀄리티 모드 로드
        if (quality != null)
        {
            int qualityMode = PlayerPrefs.GetInt("QualityMode", 1); // 기본값으로 1 설정
            quality.value = qualityMode;
        }

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

    // 화면 모드 변경 함수
    public void OnDisplayModeChanged(int mode)
    {
        switch (mode)
        {
            case 0: // 전체 화면
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // 창 모드
                Screen.fullScreen = false;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // 창모드(경계 없음)
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", mode);
    }

    // 해상도 드랍다운 동적 할당 함수
    private void PopulateResolutionDropdown()
    {
        if (resolution != null)
        {
            resolution.ClearOptions();
            List<string> options = new List<string>();
            Resolution[] resolutions = Screen.resolutions;
            int currentResolutionIndex = 0; // 현재 선택된 해상도 인덱스 초기화

            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution current = resolutions[i];
                string option = $"{current.width}x{current.height}";
                options.Add(option);

                // 저장된 해상도와 일치하는 경우, 인덱스 저장
                if (current.width == Screen.currentResolution.width && current.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolution.AddOptions(options);

            // 저장된 해상도 인덱스 불러오기, 없으면 현재 해상도 인덱스 사용
            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
            resolution.value = savedResolutionIndex >= 0 && savedResolutionIndex < options.Count ? savedResolutionIndex : currentResolutionIndex;
            resolution.RefreshShownValue();
        }
    }

    // 해상도 변경 함수
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

    // 안티 앨리어싱 변경 함수
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
        float renderScaleValue = 1f; // 기본값을 1f로 설정

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

    // 퀄리티 변경 함수
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
            float normalizedLightIntensity = (lightIntensitySlider.value) / 2f;
            float displayLightIntensity = normalizedLightIntensity * 100; // 0 ~ 1 범위를 0 ~ 100으로 스케일링
            lightIntensityText.text = displayLightIntensity.ToString("0");
        }
    }

  
}
