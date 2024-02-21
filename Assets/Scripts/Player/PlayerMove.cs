using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    public float moveSpeed;
    public float sprintSpeed;
    public float gravityScale;
    public float jumpPower;
    float moveDirY;

    private float currentSpeed;
    private bool isRun = false;
    private Vector2 inputValue;
    private Vector3 moveDir;
    public bool isMoving;

    public Transform cameraTransform; // ī�޶��� Transform

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.overlayManager.CheckOnOverlay())
        {
            return;
        }

        CalcGravity();
        Ani();
        isMoving = inputValue.magnitude > 0f;

        if (inputValue.y < 0) //�ڷ� ����Ű�� ������ �̼Ӱ���
        {
            currentSpeed = isRun ? sprintSpeed * 0.5f : moveSpeed * 0.7f;
        }
        else
        {
            currentSpeed = isRun ? sprintSpeed : moveSpeed;
        }

        // ī�޶� ���⿡ ����� �÷��̾��� �̵� ���� ���
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; // ī�޶��� ���� �������� ����
        right.y = 0; // ī�޶��� ���� �������� ����
        forward.Normalize();
        right.Normalize();

        moveDir = (forward * inputValue.y + right * inputValue.x) * currentSpeed;
        moveDirY += Physics.gravity.y * gravityScale * Time.deltaTime; // �߷� ���ӵ� �߰�
        moveDir.y = moveDirY;

        controller.Move(moveDir * Time.deltaTime);

    }

    void CalcGravity()
    {
        //�߷� ���ӵ� ����
        moveDirY += Physics.gravity.y * gravityScale * Time.deltaTime;
    }

    public void OnMove(InputValue _value)
    {
        inputValue = _value.Get<Vector2>();
    }

   public void OnRun()
    {
        isRun = true;
     
    }

    public void OnRunCancle()
    {
        isRun = false;
      
    }

    void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            moveDirY = jumpPower;
        }
    }

   void Ani()
    {
        // animator.SetFloat("Speed", inputValue.magnitude);
        // animator.SetBool("Run", isRun);
    }
}
