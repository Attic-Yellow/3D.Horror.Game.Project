using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogTime : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;

    [SerializeField] private GameObject hourHand;
    [SerializeField] private GameObject minuteHand;

    private void Start()
    {
        hourHand.transform.localRotation = Quaternion.identity;
        minuteHand.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        UpdateClockHands();
    }

    private void UpdateClockHands()
    {
        DateTime gameTime = timeSystem.GetAnalogGameTime(); // TimeSystem���� ���� ���� �ð��� �����ɴϴ�.

        // ��ħ ȸ�� ���� ���
        float minuteRotationZ = gameTime.Minute * 6;
        // ��ħ ȸ�� ���� ��� (�ð��� ���� �⺻ ȸ�� + �п� ���� �߰� ȸ��)
        float hourRotationZ = (gameTime.Hour % 12) * 30 + gameTime.Minute * 0.5f;

        // ��ħ�� ��ħ�� ȸ���� ����
        hourHand.transform.localRotation = Quaternion.Euler(0, 0, hourRotationZ);
        minuteHand.transform.localRotation = Quaternion.Euler(0, 0, minuteRotationZ);
    }
}
