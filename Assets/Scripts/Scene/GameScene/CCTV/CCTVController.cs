using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Cinemachine;

public class CCTVController : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private CinemachineVirtualCamera vCam;

    [SerializeField] private GameObject[] floorToSwitch; // 보고자 하는 CCTV 캠들의 층 배열
    [SerializeField] private GameObject[] camToSwitchFloor1; // 보고자 하는 CCTV 캠 배열
    [SerializeField] private GameObject[] camToSwitchFloor2; // 보고자 하는 CCTV 캠 배열
    [SerializeField] private GameObject[] camToSwitchFloor3; // 보고자 하는 CCTV 캠 배열
    [SerializeField] private GameObject[] camToSwitchStairs; // 보고자 하는 CCTV 캠 배열
    [SerializeField] private GameObject[][] targetPos = new GameObject[5][];
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI camNumberText;

    [SerializeField] private bool showColon = true; // 콜론을 표시할지 여부
    [SerializeField] private float colonTimer = 0f; // 콜론 깜빡임 타이머
    [SerializeField] private int FloorNum = 3; // 현재 층

    private void Start()
    {
        if (floorToSwitch != null)
        {
            floorToSwitch[3].SetActive(true);

            for (int i = 0; i < floorToSwitch.Length - 1; i++)
            {
                floorToSwitch[i].SetActive(false);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            targetPos[i] = new GameObject[5];

            if (i == 3)
            {
                for (int j = 0; j < camToSwitchFloor3.Length; j++)
                {
                    targetPos[3][j] = camToSwitchFloor3[j];
                }
            }
            else if (i == 2)
            {
                for (int j = 0; j < camToSwitchFloor2.Length; j++)
                {
                    targetPos[2][j] = camToSwitchFloor2[j];
                }
            }
            else if (i == 1)
            {
                for (int j = 0; j < camToSwitchFloor1.Length; j++)
                {
                    targetPos[1][j] = camToSwitchFloor1[j];
                }
            }
            else if (i == 0)
            {
                for (int j = 0; j < camToSwitchStairs.Length; j++)
                {
                    targetPos[0][j] = camToSwitchStairs[j];
                }
            }
        }

        SwitchCam(0);
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
            this.FloorNum = FloorNum;
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

    public void SwitchCam(int CamNum)
    {
        if (camNumberText != null)
        {
            if (FloorNum == 0)
            {
                camNumberText.text = ($"Cam S-{CamNum + 1}");
            }
            else
            {
                camNumberText.text = ($"Cam {FloorNum}-{CamNum + 1}");
            }
            
        }

        if (targetPos != null)
        {
            vCam.transform.rotation = targetPos[FloorNum][CamNum].gameObject.transform.Find("CRT Target").transform.rotation;
            vCam.m_Follow = targetPos[FloorNum][CamNum].gameObject.transform.Find("CRT Target").transform;
        }
    }
}
