using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSFadeEffect : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiElement;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float blinkDuration; // 전체 블링크 지속 시간
    [SerializeField] private float minBlinkFrequency = 0.05f; // 최소 블링크 빈도 (가장 짧은 간격)
    [SerializeField] private float maxBlinkFrequency = 0.5f; // 최대 블링크 빈도 (가장 긴 간격)
    [SerializeField] private float lodingTime;
    [SerializeField] private GameObject lodingScreen;
    [SerializeField] private GameObject startScren;
    [SerializeField] private GameObject interactiveIcon;

    public void FadeBlink()
    {
        if (!lodingScreen.activeSelf)
        {
            return;
        }
        StartCoroutine(GradualBlinkEffect());
    }

    private IEnumerator GradualBlinkEffect()
    {
        float startTime = Time.time;
        blinkDuration = Random.Range(2f, 4f);

        // 블링크 빈도가 증가하는 부분
        while (Time.time - startTime < blinkDuration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / blinkDuration;

            float currentFrequency = Mathf.Lerp(maxBlinkFrequency, minBlinkFrequency, t);
            yield return BlinkOnce(currentFrequency);
        }

        DisableCanvasGroup();

        lodingTime = Random.Range(1f, 2f);

        lodingScreen.SetActive(false);
        startScren.SetActive(true);
        yield return new WaitForSeconds(lodingTime);
        startScren.SetActive(false);
        interactiveIcon.SetActive(true);
    }

    private IEnumerator BlinkOnce(float frequency)
    {
        yield return FadeOutEffect();
        yield return new WaitForSeconds(frequency / 2);
        yield return FadeInEffect();
        yield return new WaitForSeconds(frequency / 2);
    }

    private IEnumerator FadeInEffect()
    {
        uiElement.blocksRaycasts = true;
        while (uiElement.alpha < 1)
        {
            uiElement.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private IEnumerator FadeOutEffect()
    {
        uiElement.blocksRaycasts = false;
        while (uiElement.alpha > 0)
        {
            uiElement.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private void DisableCanvasGroup()
    {
        // CanvasGroup 비활성화 설정
        uiElement.blocksRaycasts = false;
        uiElement.interactable = false;
        uiElement.alpha = 0; // 완전히 투명하게 설정
    }
}
