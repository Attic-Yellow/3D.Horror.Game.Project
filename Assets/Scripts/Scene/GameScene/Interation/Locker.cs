using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Locker : MonoBehaviour
{
    public CinemachineVirtualCameraBase camera2;
    public PlayableDirector timeline;
    public bool isChanged = false;
    public bool timelinePlaying = false;
    public GameObject tf;
    public bool isIn;

    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (!isChanged)
        {
            timeline.paused += CameraChange;
            timeline.stopped += OutLocker;
        }
    }
   
    public void OnTimeline()
    {
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
        camera2.gameObject.SetActive(false);
    }
    public void PositionAndRotation(Transform _tf)
    {
        camera2.gameObject.SetActive(true);
        camera2.transform.position = _tf.position;
        camera2.transform.rotation = _tf.rotation;
        
    }

    public void CameraPriorityChange(int _num)
    {
        camera2.Priority = _num;
    }

    private IEnumerator PauseTimeline(float delay)
    {
        yield return new WaitForSeconds(delay);

        timeline.Pause();
        timelinePlaying = false;
    }
}
