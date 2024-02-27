using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireExit : Mission
{
    [SerializeField] private TextMeshProUGUI missionSubjectText;

    private void Start()
    {
        missionType = MissionType.FireExtinguisher;
        missionName = missionSubjectText.text;
        isCompleted = false;
    }

    protected override void MissionCompleted()
    {
        base.MissionCompleted();
    }
}
