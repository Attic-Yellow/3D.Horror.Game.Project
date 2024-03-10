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

    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchWalkSpeed;
    [SerializeField] private float crouchRunSpeed;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpForce;

    private float currentSpeed;
    private bool isRun = false;
    private bool isCrouch = false;
    private Vector2 inputValue;
    private Vector3 moveDir;
    public bool isMoving;
    private Player player;
    

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = State.Standing;
        animator.SetLayerWeight(1, 0);
    }
    
    private void Update()
    {
        if (!player.isOver && !player.cameraController.GetOverlayCamAtive())
        {
            Move();
        }
        else
        {
            GameManager.instance.settingsManager.StopMoveSFX();
        }
    }
    private void Move()
    {
        Ani();

        state = isCrouch ? State.Crouch : State.Standing;

        switch (state)
        {
            case State.Standing:
                isMoving = inputValue.magnitude > 0f;
                if (isMoving)
                {
                    if (inputValue.y < 0) //뒤로 가는키를 누르면 이속감소
                    {
                        currentSpeed = isRun ? sprintSpeed * 0.5f : moveSpeed * 0.7f;
                        GameManager.instance.settingsManager.ChangeSouncePitch(0.7f);
                    }
                    else
                    {
                        currentSpeed = isRun ? sprintSpeed : moveSpeed;
                        GameManager.instance.settingsManager.ResetPitch();
                    }
                    if (!isRun)
                    {
                        GameManager.instance.settingsManager.PlayMoveSFX(0);
                    }
                    else
                    {
                        GameManager.instance.settingsManager.PlayMoveSFX(1);
                    }
                }
                else
                {
                    GameManager.instance.settingsManager.StopMoveSFX();
                }
                break;
            case State.Crouch:

                bool isMove = inputValue.magnitude > 0f;
                if (isMove)
                {
                    if (inputValue.y < 0)
                    {
                        currentSpeed = isRun ? crouchRunSpeed * 0.5f : crouchWalkSpeed * 0.7f;
                        GameManager.instance.settingsManager.ChangeSouncePitch(0.5f);
                    }
                    else
                    {
                        currentSpeed = isRun ? crouchRunSpeed : crouchWalkSpeed;
                        GameManager.instance.settingsManager.ResetPitch();
                    }

                    if (!isRun)
                    {
                        GameManager.instance.settingsManager.ChangeSouncePitch(0.7f);
                        GameManager.instance.settingsManager.PlayMoveSFX(2);
                    }
                    else
                    {
                        GameManager.instance.settingsManager.PlayMoveSFX(2);
                    }
                }
                else
                {
                    GameManager.instance.settingsManager.StopMoveSFX();
                }
                
             
                break;
        }

        // 카메라 방향에 기반한 플레이어의 이동 방향 계산
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.Normalize();
        right.Normalize();

        moveDir = forward * inputValue.y + right * inputValue.x;  
        print(moveDir);
        /*Gravity();*/
        controller.Move(moveDir * Time.deltaTime *currentSpeed );   
    }
    private void Gravity()
    {
        if (!controller.isGrounded)
        {
            moveDir.y -= gravityScale;
        }
    
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
        animator.SetFloat("Velocity", new Vector3(moveDir.x, 0, moveDir.z).magnitude);
        animator.SetFloat("XDir", inputValue.x * currentSpeed);
        animator.SetFloat("ZDir", inputValue.y * currentSpeed);

        animator.SetBool("IsCrouch", isCrouch);
        animator.SetBool("IsRun", isRun);

    }
    public void TakeOutAni()
    {
        animator.SetLayerWeight(1, 1f); //애니메이터의 두번쨰 레이어의 weight를 1로
        animator.SetTrigger("TakeOut");
    }
    public void WeightZero() // 올리는 애니메이션 끝날때
    {
        animator.SetLayerWeight(1, 0f);
        //TODO : 펼쳐지는 애니메이션 및 카메라 무브.
    }
    public void OnJump()
    {
        if (!controller.isGrounded)
        {
            print("점프");
            moveDir.y = jumpForce;
        }
    }
}
