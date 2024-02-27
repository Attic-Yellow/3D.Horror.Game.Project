using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManagedSystem : MonoBehaviour
{
    [SerializeField] private List<MissionType> missions;
    [SerializeField] private List<Mission> allMissions;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private MissionType currentMission;

    private void Start()
    {
        currentMission = MissionType.None;
    }

    private void Update()
    {
        CheckTime();
    }

    private void CheckTime()
    {
        string checktime = timeSystem.GetGameTime();

        switch (checktime)
        {
            case "21:00":
                SetNewMission();
                break;            
            case "22:00":
                SetNewMission();
                break;
            case "23:00":
                SetNewMission();
                break;
            case "24:00":
                SetNewMission();
                break;
            case "01:00":
                SetNewMission();
                break;
            case "02:00":
                SetNewMission();
                break;
            case "03:00":
                SetNewMission();
                break;
            case "04:00":
                SetNewMission();
                break;
            case "05:00":
                SetNewMission();
                break;
            case "06:00":
                SetNewMission();
                break;
        }
    }

    private void SetNewMission()
    {
        int randomMission = Random.Range(0, missions.Count);
        currentMission = missions[randomMission];
        ActivateMissionObject(currentMission);
    }

    private void ActivateMissionObject(MissionType missionType)
    {
        foreach (var mission in allMissions)
        {
            if (mission.missionType == missionType)
            {
                mission.gameObject.SetActive(true); // 해당 타입의 미션 활성화
            }
            else
            {
                mission.gameObject.SetActive(false); // 나머지 비활성화
            }
        }
    }
}
