using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FierExit : Mission
{
    private void Start()
    {
        missionType = MissionType.FireExtinguisher;
        missionName = "Fire Extinguisher";
    }

    protected override void MissionCompleted()
    {
        base.MissionCompleted();
    }
}
