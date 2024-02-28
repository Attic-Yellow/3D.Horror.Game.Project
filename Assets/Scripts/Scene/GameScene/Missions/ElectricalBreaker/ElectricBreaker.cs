using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricBreaker : Mission
{
    [SerializeField] private TextMeshProUGUI missionSubjectText;

    private void Start()
    {
        missionType = MissionType.ElectricShield;
        missionName = missionSubjectText.text;
        isCompleted = false;
    }

    protected override void MissionCompleted()
    {
        base.MissionCompleted();
    }
}
