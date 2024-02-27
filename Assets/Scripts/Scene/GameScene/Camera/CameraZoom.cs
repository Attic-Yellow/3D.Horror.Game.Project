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
    [SerializeField] private bool isMonitorOn = false;
    [SerializeField] private bool isMission = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isZoomIn)
        {
            if (isMonitorOn)
            {
                isMonitorOn = !isMonitorOn;
                cameraController.SetOverlayCamAtive();
                GameManager.instance.overlayManager.ComputerOverlayController();
                monitorControl.OnAndOff();
            }
            else if (missions != null && isMission)
            {
                if (missions.gameObject.GetComponentInParent<Shield>() != null)
                {
                    print("Shield");
                    missions.gameObject.GetComponentInParent<Shield>().CloseShield();
                }
                else if(missions.gameObject.GetComponentInChildren<DrawLineRenderer>() != null)
                {
                    print("draw");
                  missions.gameObject.GetComponentInChildren<DrawLineRenderer>().DeleteLines();
                }
                MissionOverlayControl(missions);
                
            }

            LookAtZoomOut();
            cameraController.SetPointCamActive();
            missions = null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (missions != null && missions.gameObject.GetComponentInParent<Shield>() != null)
            {
                cameraController.SetOverlayCamAtive();
            }
        }
    }

    // 카메라 줌인, 줌아웃
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

        vCam.m_Lens.FieldOfView = targetFOV;
        vCam.transform.rotation = targetRotation;
    }

    // 카메라 줌인
    public void LookAtZoomIn(GameObject target)
    {
        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        if (transposer != null)
        {
            transposer.m_XDamping = 1;
            transposer.m_YDamping = 1;
            transposer.m_ZDamping = 1;
        }

        isZoomIn = true;
        initRotation = vCam.transform.rotation;
        targetRotation = target.transform.rotation;
        vCam.m_Follow = target.transform;
        StartCoroutine(ChangeFOV(26, targetRotation, 1.2f));
        CursorConfined();
    }

    // 카메라 줌아웃
    public void LookAtZoomOut()
    {
        isZoomIn = !isZoomIn;
        vCam.m_Follow = player.transform;
        StartCoroutine(ChangeFOV(70, initRotation, 1.2f));
        CuesorLoked();
        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        if (transposer != null)
        {
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;
        }
    }
    
    // 모니터 켜는 효과
    public void MonitorOn()
    {
        isMonitorOn = !isMonitorOn;
        cameraController.SetOverlayCamAtive();
        cameraController.SetPointCamActive();
        StartCoroutine(TurnOnEffect());
    }

    // 모니터 켜는 효과
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
            if (missions.gameObject.transform.Find("MissionRoot") != null)
            {
                cameraController.SetOverlayCamAtive();
                StartCoroutine(MissionEffect());
            }
        }
        else
        {
            if (missions.gameObject.transform.Find("MissionRoot") != null)
            {
                cameraController.SetOverlayCamAtive();
                missions.gameObject.transform.Find("MissionRoot").gameObject.SetActive(isMission);
            }
        }
    }

    private IEnumerator MissionEffect()
    {
        yield return new WaitForSeconds(1.25f);

        missions.gameObject.transform.Find("MissionRoot").gameObject.SetActive(isMission);
    }

    private void CuesorLoked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CursorConfined()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
