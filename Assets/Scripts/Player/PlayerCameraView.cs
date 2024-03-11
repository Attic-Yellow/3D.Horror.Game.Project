using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraView : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private CameraController camController;
    [SerializeField] private Player player;
    private float xRotation = 0f;

    private void Awake()
    {
        player = playerBody.gameObject.GetComponent<Player>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (vCam.m_Follow != playerBody || camController.GetOverlayCamAtive() || player.isOver || player.LiveCamCam3())
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ĳ������ ���� ȸ���� ó��
        playerBody.Rotate(Vector3.up * mouseX);
        // ī�޶��� ���� ȸ���� ó��
        xRotation -= mouseY;   
        xRotation = Mathf.Clamp(xRotation, -65f, 65f);

        if (player.currentItem != null)
        {
           /* Player.ItemPos.rotation = transform.rotation;*/
           Quaternion cameraRotation = Quaternion.Euler(transform.localEulerAngles);

            // �������� ���ο� ȸ������ ����մϴ�.
            Quaternion newItemRotation = cameraRotation * player.currentItem.originalRotate;

            // ���ο� ȸ������ �����ۿ� �����մϴ�.
            player.currentItem.transform.rotation = newItemRotation;
        }
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
