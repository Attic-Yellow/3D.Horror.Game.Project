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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CalcGravity();
        Ani();
        if (inputValue.y < 0) //뒤로 가는키를 누르면 이속감소
        {
            currentSpeed = isRun ? sprintSpeed * 0.5f : moveSpeed * 0.7f;
        }
        else
        {
            currentSpeed = isRun ? sprintSpeed : moveSpeed;
        }
        moveDir = transform.TransformDirection(new Vector3(inputValue.x, moveDirY, inputValue.y)) * currentSpeed;
        controller.Move(moveDir * Time.deltaTime);

    }

    void CalcGravity()
    {
        //중력 가속도 가산
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
        animator.SetFloat("Speed", inputValue.magnitude);
        animator.SetBool("Run", isRun);
    }
}
