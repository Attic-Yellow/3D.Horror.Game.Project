using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSystem : MonoBehaviour
{
    public CinemachineVirtualCameraBase camera2;
    public PlayableDirector timeline;
    public bool isChanged = false;
    public bool timelinePlaying = false;
    protected void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
    }

    public void PositionAndRotation(Transform _tf)
    {
      camera2.transform.position =   _tf.position ;
      camera2.transform.rotation  = _tf.rotation;
    }

    public void CameraPriorityChange(int _num)
    {
        camera2.Priority = _num;
    }

    protected IEnumerator PauseTimeline(float delay)
    {
        yield return new WaitForSeconds(delay);

        timeline.Pause();
        timelinePlaying = false;
    }

    public virtual void OnTimeline()
    {
        timeline.Play();
    }
}
