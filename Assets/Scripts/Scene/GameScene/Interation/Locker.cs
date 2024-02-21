using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Locker : TimelineSystem
{
    public GameObject tf;
    public bool isIn;

    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!isChanged)
        {
            timeline.paused += CameraChange;
            timeline.stopped += OutLocker;
        }
    }
   
    public override void OnTimeline()
    {
        isChanged = false;
        PositionAndRotation(tf.transform);
        isIn = true;
        timeline.Play();
        timelinePlaying = true;
        StartCoroutine(PauseTimeline(375/60f));
        
    }

    public void ReverseTimeline()
    {
        isChanged = false;
        timeline.Play();
        timelinePlaying = true;
    }

    private void CameraChange(PlayableDirector director)
    {
        isChanged = true;
        CameraPriorityChange(11);
    }

    private void OutLocker(PlayableDirector director)
    {
        CameraPriorityChange(9);
        isIn = false;
        isChanged = true;
        timelinePlaying = false;
    }
}
