using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    FireExtinguisher,
    ElectricShield
}

public class Mission : MonoBehaviour
{


    [SerializeField] protected string missionName;

    public MissionType missionType;
    public bool isCompleted;

    protected virtual void MissionCompleted()
    {
        isCompleted = true;
    }
}
