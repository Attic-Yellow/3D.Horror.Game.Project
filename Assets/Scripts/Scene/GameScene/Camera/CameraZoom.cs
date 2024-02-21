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

    public bool isZoomIn = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isZoomIn)
        {
            GameManager.instance.overlayManager.ComputerOverlayController();
            monitorControl.OnAndOff();
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
        isZoomIn = false;
        vCam.m_Follow = player.transform;
        StartCoroutine(ChangeFOV(70, initRotation, 1.2f));
    }
    
    // ����� �Ѵ� ȿ��
    public void MonitorOn()
    {
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
}