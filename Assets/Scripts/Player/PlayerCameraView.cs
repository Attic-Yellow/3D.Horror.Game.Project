using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraView : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private float xRotation = 0f;

    private void Start()
    {
        /*Cursor.lockState = CursorLockMode.Locked;*/
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (vCam.m_Follow != playerBody || GameManager.instance.overlayManager.CheckOnOverlay())
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ĳ������ ���� ȸ���� ó��
        playerBody.Rotate(Vector3.up * mouseX);

        // ī�޶��� ���� ȸ���� ó��
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ī�޶��� ���� ȸ���� ���� ����
        transform.localEulerAngles = new Vector3(xRotation, playerBody.eulerAngles.y, 0f);
    }

    public void UpdateMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    public void CursorLockedToConfined(CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
    }
}
