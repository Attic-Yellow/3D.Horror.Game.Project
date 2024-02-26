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
        DateTime gameTime = timeSystem.GetAnalogGameTime(); // TimeSystem에서 현재 게임 시간을 가져옵니다.

        // 분침 회전 각도 계산
        float minuteRotationZ = gameTime.Minute * 6;
        // 시침 회전 각도 계산 (시간에 따른 기본 회전 + 분에 따른 추가 회전)
        float hourRotationZ = (gameTime.Hour % 12) * 30 + gameTime.Minute * 0.5f;

        // 시침과 분침의 회전을 적용
        hourHand.transform.localRotation = Quaternion.Euler(0, 0, hourRotationZ);
        minuteHand.transform.localRotation = Quaternion.Euler(0, 0, minuteRotationZ);
    }
}
