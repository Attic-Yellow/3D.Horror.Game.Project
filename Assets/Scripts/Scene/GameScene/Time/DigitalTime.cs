using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalTime : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private bool showColon = true; // �ݷ��� ǥ������ ����
    [SerializeField] private float colonTimer = 0f; // �ݷ� ������ Ÿ�̸�

    private void Update()
    {
        // �ݷ� ������ Ÿ�̸� ������Ʈ
        colonTimer += Time.deltaTime;

        if (colonTimer >= 1f) // 1�ʸ��� �ݷ� ���� ���
        {
            showColon = !showColon;
            colonTimer = 0f; // Ÿ�̸� ����
        }

        // �ð� ���ڿ� ������
        string timeString = timeSystem.GetDigitalTimeString();
        if (!showColon)
        {
            // �ݷ��� ���� ��
            timeString = timeString.Replace(":", " ");
        }

        // �ؽ�Ʈ ������Ʈ�� �ð� ǥ��
        timeText.text = timeString;
    }
}
