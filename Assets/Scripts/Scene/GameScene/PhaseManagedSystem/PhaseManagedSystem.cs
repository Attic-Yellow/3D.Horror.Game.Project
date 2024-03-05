using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManagedSystem : MonoBehaviour
{
    [SerializeField] private List<MissionType> missions;
    [SerializeField] private List<Mission> allMissions;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private MissionType currentMission;
    public bool missionSuccess = true;
    private string checktime;
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
        checktime = timeSystem.GetGameTime();

        switch (checktime)
        {
            case "21:00":
            case "23:00":
            case "01:00":
            case "03:00":
            case "05:00":
            case "07:00":
                SetNewMission();
                break;
        }
    }

    private void SetNewMission()
    {
        if (missionSuccess)
        {
            missionSuccess = false;
            int randomMission = Random.Range(0, missions.Count);
            currentMission = missions[randomMission];
            ActivateMissionObject(currentMission);
        }
        else
        {
            Player player = FindObjectOfType<Player>();
            if (player!= null)
            {
                player.SetIsOver(true);
                //�ٽý��� UI
            }

        }
    }

    private void ActivateMissionObject(MissionType missionType)
    {
        foreach (var mission in allMissions)
        {
            if (mission.missionType == missionType)
            {
                mission.gameObject.SetActive(true); // �ش� Ÿ���� �̼� Ȱ��ȭ
            }
            else
            {
                mission.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
            }
        }
    }

    public void SetMissionSuccess(bool isbool)
    {
      missionSuccess = isbool;
    }
  

}
