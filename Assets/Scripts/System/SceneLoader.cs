using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider; // 로딩 진행 상황을 표시할 UI 슬라이더
    [SerializeField] private TextMeshProUGUI loadingSubjectText; // 로딩 진행률을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI loadingCompleteText; // 로딩 진행률을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI loadingIntText; // 로딩 진행률을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI loadingStatusText; // 로딩 상태를 표시할 텍스트
    [SerializeField] private float delayBeforeLoading = 4f; // 씬 로딩 전 대기 시간
    [SerializeField] private Coroutine loadingStatusCoroutine;

    private string[] loadingDots = { ".", "..", "..." };

    private void Start()
    {
        // 초기 상태 설정
        loadingCompleteText.gameObject.SetActive(true); // 로딩 완료 텍스트 비활성화
        loadingSubjectText.gameObject.SetActive(true); // 로딩 제목 텍스트 활성화
    }

    // 씬 로딩을 시작하는 메서드
    public void LoadScene(string sceneName)
    {
        loadingCompleteText.gameObject.SetActive(false);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // 비동기적으로 씬을 로딩하는 코루틴
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingStatusCoroutine = StartCoroutine(UpdateLoadingStatus());
        yield return new WaitForSeconds(delayBeforeLoading);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // 씬 바로 활성화 방지

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;
            loadingIntText.text = $"{progress * 100:0}%";

            if (asyncLoad.progress >= 0.9f)
            {
                if (loadingStatusCoroutine != null)
                {
                    StopCoroutine(loadingStatusCoroutine);
                    loadingStatusCoroutine = null; // 참조 초기화
                }
                loadingStatusText.text = "!!!";
                loadingIntText.text = "100%";
                loadingCompleteText.gameObject.SetActive(true);
                loadingSubjectText.gameObject.SetActive(false);

                // 로딩 완료 표시를 유지하기 위해 추가 대기
                yield return new WaitForSeconds(0.5f);

                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // 로딩 상태 텍스트 업데이트 코루틴
    private IEnumerator UpdateLoadingStatus()
    {
        int dotIndex = 0;
        while (true)
        {
            loadingStatusText.text = loadingDots[dotIndex++ % loadingDots.Length];
            yield return new WaitForSeconds(0.25f); // 0.5초마다 텍스트 업데이트
        }
    }
}
