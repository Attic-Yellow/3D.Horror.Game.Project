using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManagedSystem : MonoBehaviour
{
    [SerializeField] private List<MissionType> missions;
    [SerializeField] private List<Mission> allMissions;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private MissionType currentMission;
    [SerializeField] private bool missionSuccess;
    [SerializeField] private bool check = true;
    private string checktime;
    private void Start()
    {
        missionSuccess = true;
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
                if (check)
                {
                    SetNewMission();
                    check = false;
                    StartCoroutine(ResetCheck()); 
                }
                break;
        }
    }

    private void SetNewMission()
    {
        print("ȣ��");
        if (missionSuccess)
        {
            missionSuccess = false;
            int randomMission = Random.Range(0, missions.Count);
            currentMission = missions[randomMission];
            ActivateMissionObject(currentMission);
            return;
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
    private IEnumerator ResetCheck()
    {
        yield return new WaitForSeconds(1f); 
        check = true; 
    }

}
