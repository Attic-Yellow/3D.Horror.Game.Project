using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float maxIntensity = 1f; // 최대 강도
    public float minIntensity = 0.2f; // 최소 강도
    public float BlinkSpeed = 3f;// 깜빡임 속도
    public float detectionRadius = 5f; // 플레이어 감지 반경
    public float duration = 10f; // 블링크 지속 시간


    private bool isBlinking = false;
    private bool isFrist = true;
    private UnityEngine.Light lightSource;


    void Start()
    {
        lightSource = GetComponent<UnityEngine.Light>();
    }

    void Update()
    {
        if (isFrist && CheckPlayerProximity()&& !isBlinking)
        {
            StartCoroutine(BlinkCoroutine());
        }

    }
    IEnumerator BlinkCoroutine()
    {
        isBlinking = true;

        float elapsedTime = 0f; // 경과 시간

        while (elapsedTime < duration)
        {
            float intensity = Mathf.PingPong(Time.time * BlinkSpeed, maxIntensity - minIntensity) + minIntensity;
            lightSource.intensity = intensity;

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null;
        }

        lightSource.intensity = 0;
        isFrist = false;
    }


    bool CheckPlayerProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, 1 << 9);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
