using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CCTVController : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;

    [SerializeField] private GameObject[] floorToSwitch;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI camNumberText;

    [SerializeField] private bool showColon = true; // �ݷ��� ǥ������ ����
    [SerializeField] private float colonTimer = 0f; // �ݷ� ������ Ÿ�̸�

    private void Start()
    {
        if (floorToSwitch != null)
        {
            floorToSwitch[0].SetActive(true);

            for (int i = 1; i < floorToSwitch.Length; i++)
            {
                floorToSwitch[i].SetActive(false);
            }
        }
    }

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
        DateTime timeS = timeSystem.GetAnalogGameTime();

        string timeString = timeS.ToString("yyyy-MM-dd HH:mm:ss");

        if (!showColon)
        {
            // �ݷ��� ���� ��
            timeString = timeString.Replace(":", " ");
        }

        // �ؽ�Ʈ ������Ʈ�� �ð� ǥ��
        timeText.text = timeString;
    }

    public void SwitchFloor(int FloorNum)
    {
        if (floorToSwitch != null)
        {
            floorToSwitch[FloorNum].SetActive(true);

            for (int i = 0; i < floorToSwitch.Length; i++)
            {
                if (i != FloorNum)
                {
                    floorToSwitch[i].SetActive(false);
                }
            }
        }
    }
}
