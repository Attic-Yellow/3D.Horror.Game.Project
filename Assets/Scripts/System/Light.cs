using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float maxIntensity = 1f; // �ִ� ����
    public float minIntensity = 0.2f; // �ּ� ����
    public float BlinkSpeed = 3f;// ������ �ӵ�
    public float detectionRadius = 5f; // �÷��̾� ���� �ݰ�
    public float duration = 10f; // ��ũ ���� �ð�


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

        float elapsedTime = 0f; // ��� �ð�

        while (elapsedTime < duration)
        {
            float intensity = Mathf.PingPong(Time.time * BlinkSpeed, maxIntensity - minIntensity) + minIntensity;
            lightSource.intensity = intensity;

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
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
