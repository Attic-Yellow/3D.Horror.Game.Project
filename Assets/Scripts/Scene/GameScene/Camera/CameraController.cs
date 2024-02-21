using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera pointCam;
    [SerializeField] private Camera crtCam;
    [SerializeField] private Transform[] target;

    private void Start()
    {
        if (pointCam != null)
        {
            pointCam.gameObject.SetActive(true);
        }

        if (crtCam != null)
        {
            crtCam.gameObject.SetActive(false);
        }
    }

    public void SetPointCamActive()
    {
        if (pointCam != null)
        {
            pointCam.gameObject.SetActive(!pointCam.gameObject.activeSelf);
        }
    }

    public void SetCRTCamActive()
    {
        if (crtCam != null)
        {
            crtCam.gameObject.SetActive(!crtCam.gameObject.activeSelf);
        }
    }
}