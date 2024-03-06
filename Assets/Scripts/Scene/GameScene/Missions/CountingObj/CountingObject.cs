using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingObject : Mission
{
    [SerializeField] private string missionText;
    public List<ExcelRow> rows = new();
    void Start()
    {
        missionName = missionText;
        missionType = MissionType.CountingObj;
    }

    private void Update()
    {
        if(!isCompleted)
        {
            MissionCompleted();
        }
    }

    public override void  MissionCompleted()
    {
        bool allTrue = true;
        foreach (ExcelRow row in rows)
        {
            if (!row.isTrue)
            {
                allTrue = false;
                break;
            }
        }

        if (allTrue)
        {
            base.MissionCompleted();
        }
    }
}
