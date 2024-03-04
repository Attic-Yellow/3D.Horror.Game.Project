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

    [SerializeField] private bool showColon = true; // 콜론을 표시할지 여부
    [SerializeField] private float colonTimer = 0f; // 콜론 깜빡임 타이머

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
        // 콜론 깜빡임 타이머 업데이트
        colonTimer += Time.deltaTime;

        if (colonTimer >= 1f) // 1초마다 콜론 상태 토글
        {
            showColon = !showColon;
            colonTimer = 0f; // 타이머 리셋
        }

        // 시간 문자열 포맷팅
        DateTime timeS = timeSystem.GetAnalogGameTime();

        string timeString = timeS.ToString("yyyy-MM-dd HH:mm:ss");

        if (!showColon)
        {
            // 콜론을 숨길 때
            timeString = timeString.Replace(":", " ");
        }

        // 텍스트 컴포넌트에 시간 표시
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
