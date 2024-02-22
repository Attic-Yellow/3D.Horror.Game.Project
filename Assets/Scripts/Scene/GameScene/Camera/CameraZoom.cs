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
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject missions;

    public bool isZoomIn = false;
    private bool isMonitorOn = false;
    private bool isMission = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isZoomIn)
        {
            if (isMonitorOn)
            {
                isMonitorOn = !isMonitorOn;
                GameManager.instance.overlayManager.ComputerOverlayController();
                monitorControl.OnAndOff();
            }
            else if (isMission)
            {
                MissionOverlayControl(missions);
            }
            
            LookAtZoomOut();
            cameraController.SetPointCamActive();
        }
    }

    // ī�޶� ����, �ܾƿ�
    private IEnumerator ChangeFOV(float targetFOV, Quaternion targetRotation, float duration)
    {
        float startTime = Time.time;
        float startFOV = vCam.m_Lens.FieldOfView;
        Quaternion initialRotation = vCam.transform.rotation; // �ʱ� ȸ�� ��

        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / duration; // ����� ���

            vCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            vCam.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            yield return null;
        }

        vCam.m_Lens.FieldOfView = targetFOV;
        vCam.transform.rotation = targetRotation;
    }

    // ī�޶� ����
    public void LookAtZoomIn(GameObject target)
    {
        isZoomIn = true;
        initRotation = vCam.transform.rotation;
        targetRotation = target.transform.rotation;
        vCam.m_Follow = target.transform;
        StartCoroutine(ChangeFOV(26, targetRotation, 1.2f));
    }

    // ī�޶� �ܾƿ�
    public void LookAtZoomOut()
    {
        isZoomIn = !isZoomIn;
        vCam.m_Follow = player.transform;
        StartCoroutine(ChangeFOV(70, initRotation, 1.2f));
    }
    
    // ����� �Ѵ� ȿ��
    public void MonitorOn()
    {
        isMonitorOn = !isMonitorOn;
        cameraController.SetPointCamActive();
        StartCoroutine(TurnOnEffect());
    }

    // ����� �Ѵ� ȿ��
    private IEnumerator TurnOnEffect()
    {
        yield return new WaitForSeconds(1.25f);

        GameManager.instance.overlayManager.ComputerOverlayController();
        oSFadeEffect.FadeBlink();
    }

    public void MissionOverlayControl(GameObject mission)
    {
        isMission = !isMission;
        missions = mission;
        if (isMission)
        {
            cameraController.SetPointCamActive();
            StartCoroutine(MissionEffect());
        }
        else
        {
            missions.gameObject.transform.Find("MissionRoot").gameObject.SetActive(isMission);
            missions = null;
        }
    }

    private IEnumerator MissionEffect()
    {
        yield return new WaitForSeconds(1.25f);

        missions.gameObject.transform.Find("MissionRoot").gameObject.SetActive(isMission);
    }
}
