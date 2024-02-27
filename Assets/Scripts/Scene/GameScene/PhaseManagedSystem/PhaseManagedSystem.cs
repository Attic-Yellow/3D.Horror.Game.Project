using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManagedSystem : MonoBehaviour
{
    [SerializeField] private List<MissionType> missions;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private MissionType currentMission;

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
    }
}
