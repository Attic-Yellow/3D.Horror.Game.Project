using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    FireExtinguisher,
    ElectricShield,
    CountingObj,
    None
}

public class Mission : MonoBehaviour
{
    [SerializeField] protected string missionName;

    public MissionType missionType;
    public bool isCompleted;

    public virtual void MissionCompleted()
    {
        isCompleted = true;
    }
}
