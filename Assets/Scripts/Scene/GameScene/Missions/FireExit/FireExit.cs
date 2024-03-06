using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireExit : Mission
{
    [SerializeField] private TextMeshProUGUI missionSubjectText;
    public  List<DrawLineRenderer> drawLines = new();

    private void Start()
    {
        missionType = MissionType.FireExtinguisher;
        missionName = missionSubjectText.text;
        isCompleted = false;
    }

    //싸인이 이미되어있는 소화기는 drawlnerender를 컴포넌트안하면된다.
    public override void MissionCompleted()
    {
        foreach (var line in drawLines)
        {
            if (!line.isDraw)
                return;
        }
        base.MissionCompleted();
    }

    public void AddList(DrawLineRenderer line)
    {
        drawLines.Add(line);
    }
}
