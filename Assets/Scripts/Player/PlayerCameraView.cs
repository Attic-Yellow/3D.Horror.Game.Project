using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 캐릭터의 수평 회전을 처리
        playerBody.Rotate(Vector3.up * mouseX);

        // 카메라의 상하 회전을 처리
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 카메라의 로컬 회전을 직접 조정
        transform.localEulerAngles = new Vector3(xRotation, playerBody.eulerAngles.y, 0f);
    }
}
