using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider; // �ε� ���� ��Ȳ�� ǥ���� UI �����̴�
    [SerializeField] private TextMeshProUGUI loadingSubjectText; // �ε� ������� ǥ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI loadingCompleteText; // �ε� ������� ǥ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI loadingIntText; // �ε� ������� ǥ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI loadingStatusText; // �ε� ���¸� ǥ���� �ؽ�Ʈ
    [SerializeField] private float delayBeforeLoading = 4f; // �� �ε� �� ��� �ð�
    [SerializeField] private Coroutine loadingStatusCoroutine;

    private string[] loadingDots = { ".", "..", "..." };

    private void Start()
    {
        // �ʱ� ���� ����
        loadingCompleteText.gameObject.SetActive(true); // �ε� �Ϸ� �ؽ�Ʈ ��Ȱ��ȭ
        loadingSubjectText.gameObject.SetActive(true); // �ε� ���� �ؽ�Ʈ Ȱ��ȭ
    }

    // �� �ε��� �����ϴ� �޼���
    public void LoadScene(string sceneName)
    {
        loadingCompleteText.gameObject.SetActive(false);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // �񵿱������� ���� �ε��ϴ� �ڷ�ƾ
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingStatusCoroutine = StartCoroutine(UpdateLoadingStatus());
        yield return new WaitForSeconds(delayBeforeLoading);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // �� �ٷ� Ȱ��ȭ ����

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
                    loadingStatusCoroutine = null; // ���� �ʱ�ȭ
                }
                loadingStatusText.text = "!!!";
                loadingIntText.text = "100%";
                loadingCompleteText.gameObject.SetActive(true);
                loadingSubjectText.gameObject.SetActive(false);

                // �ε� �Ϸ� ǥ�ø� �����ϱ� ���� �߰� ���
                yield return new WaitForSeconds(0.5f);

                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // �ε� ���� �ؽ�Ʈ ������Ʈ �ڷ�ƾ
    private IEnumerator UpdateLoadingStatus()
    {
        int dotIndex = 0;
        while (true)
        {
            loadingStatusText.text = loadingDots[dotIndex++ % loadingDots.Length];
            yield return new WaitForSeconds(0.25f); // 0.5�ʸ��� �ؽ�Ʈ ������Ʈ
        }
    }
}
