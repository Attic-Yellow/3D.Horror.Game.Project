using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private enum State
    {
       Standing,
       Crouch
    }
    private State state;
    private CharacterController controller;
    private Animator animator;

    public float moveSpeed;
    public float sprintSpeed;


    
    float moveDirY;

    private float currentSpeed;
    private bool isRun = false;
    private bool isCrouch = false;
    private Vector2 inputValue;
    private Vector3 moveDir;
    public bool isMoving;
  
    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = State.Standing;
  
    }

    private void Update()
    {
        if (GameManager.instance.overlayManager.CheckOnOverlay())
        {
            return;
        }
        state = isCrouch ? State.Crouch : State.Standing;
        isMoving = inputValue.magnitude > 0f;
        switch (state)
        {
          case State.Standing:

                break;
          case State.Crouch:

                break;
        }
     

      

        if (inputValue.y < 0) //뒤로 가는키를 누르면 이속감소
        {
            currentSpeed = isRun ? sprintSpeed * 0.5f : moveSpeed * 0.7f;
        }
        else
        {
            currentSpeed = isRun ? sprintSpeed : moveSpeed;
        }

        // 카메라 방향에 기반한 플레이어의 이동 방향 계산
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; // 카메라의 상하 움직임은 무시
        right.y = 0; // 카메라의 상하 움직임은 무시
        forward.Normalize();
        right.Normalize();

        moveDir = (forward * inputValue.y + right * inputValue.x) * currentSpeed;
        moveDir.y = 0;

        controller.Move(moveDir * Time.deltaTime);


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
    public void OnCrouch()
    {
        isCrouch = !isCrouch;
    }
    private void Ani()
    {
        animator.SetFloat("Dir", moveDir.magnitude * Time.deltaTime);
        // animator.SetFloat("")
    }

  
}
