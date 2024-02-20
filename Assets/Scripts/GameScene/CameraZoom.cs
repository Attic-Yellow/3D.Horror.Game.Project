using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private GameObject player;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private Quaternion initRotation;
    [SerializeField] private MonitorControl monitorControl;
    [SerializeField] private OSFadeEffect oSFadeEffect;
    public bool isZoomIn = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isZoomIn)
        {
            GameManager.instance.overlayManager.ComputerOverlayController();
            monitorControl.OnAndOff();
            LookAtZoomOut();
        }
    }

    private IEnumerator ChangeFOV(float targetFOV, Quaternion targetRotation, float duration)
    {
        float startTime = Time.time;
        float startFOV = vCam.m_Lens.FieldOfView;
        Quaternion initialRotation = vCam.transform.rotation; // 초기 회전 값

        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / duration; // 진행률 계산

            vCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            vCam.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            yield return null;
        }
    }

    public void LookAtZoomIn(GameObject target)
    {
        isZoomIn = true;
        initRotation = vCam.transform.rotation;
        targetRotation = target.transform.rotation;
        vCam.m_Follow = target.transform;
        StartCoroutine(ChangeFOV(26, targetRotation, 1.2f));
    }

    public void LookAtZoomOut()
    {
        isZoomIn = false;
        vCam.m_Follow = player.transform;
        StartCoroutine(ChangeFOV(70, initRotation, 1.2f));
    }

    public void MonitorOn()
    {
        StartCoroutine(TurnOnEffect());
    }

    private IEnumerator TurnOnEffect()
    {
        yield return new WaitForSeconds(1.25f);

        GameManager.instance.overlayManager.ComputerOverlayController();
        oSFadeEffect.FadeBlink();
    }
}
