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

    //������ �̵̹Ǿ��ִ� ��ȭ��� drawlnerender�� ������Ʈ���ϸ�ȴ�.
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
