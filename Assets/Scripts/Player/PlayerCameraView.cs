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

        // 캐릭터의 수평 회전을 처리
        playerBody.Rotate(Vector3.up * mouseX);
        // 카메라의 상하 회전을 처리
        xRotation -= mouseY;   
        xRotation = Mathf.Clamp(xRotation, -65f, 65f);

        if (player.currentItem != null)
        {
           /* Player.ItemPos.rotation = transform.rotation;*/
           Quaternion cameraRotation = Quaternion.Euler(transform.localEulerAngles);

            // 아이템의 새로운 회전값을 계산합니다.
            Quaternion newItemRotation = cameraRotation * player.currentItem.originalRotate;

            // 새로운 회전값을 아이템에 적용합니다.
            player.currentItem.transform.rotation = newItemRotation;
        }
        // 카메라의 로컬 회전을 직접 조정
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
