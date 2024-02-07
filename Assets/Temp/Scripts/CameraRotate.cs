using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotate : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    float camVerticalRotate = 0f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity; //좌우
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity; //위
        camVerticalRotate -= inputY;
        camVerticalRotate = Mathf.Clamp(camVerticalRotate, -50f, 50f);//고정값
        transform.localEulerAngles = Vector3.right * camVerticalRotate;

       player.Rotate(Vector3.up * inputX);

       
    }
}
