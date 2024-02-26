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
    public float crouchWalkSpeed;
    public float crouchRunSpeed;
    public float gravityScale = 1f;


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
        animator.SetLayerWeight(1, 0);
    }

    private void Update()
    {
        Ani();
        if (GameManager.instance.overlayManager.CheckOnOverlay())
        {
            return;
        }
        state = isCrouch ? State.Crouch : State.Standing;
        isMoving = inputValue.magnitude > 0f;
        switch (state)
        {
            case State.Standing:

                if (inputValue.y < 0) //�ڷ� ����Ű�� ������ �̼Ӱ���
                {
                    currentSpeed = isRun ? sprintSpeed * 0.5f : moveSpeed * 0.7f;
                }
                else
                {
                    currentSpeed = isRun ? sprintSpeed : moveSpeed;
                }

                break;
            case State.Crouch:

                if (inputValue.y < 0)
                {
                    currentSpeed = isRun ? crouchRunSpeed * 0.5f : crouchWalkSpeed * 0.7f;
                }
                else
                {
                    currentSpeed = isRun ? crouchRunSpeed : crouchWalkSpeed;
                }

                break;
        }

        // ī�޶� ���⿡ ����� �÷��̾��� �̵� ���� ���
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; // ī�޶��� ���� �������� ����
        right.y = 0; // ī�޶��� ���� �������� ����
        forward.Normalize();
        right.Normalize();

        moveDir = (forward * inputValue.y + right * inputValue.x) * currentSpeed;
        Gravity();
        controller.Move(moveDir * Time.deltaTime);

    }

    private void Gravity()
    {
        if (!controller.isGrounded)
        {
            moveDir.y -= gravityScale;
        }
        else
        {
            moveDir.y = 0f;
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
        animator.SetLayerWeight(1, 1f); //�ִϸ������� �ι��� ���̾��� weight�� 1��
        animator.SetTrigger("TakeOut");
    }
    public void WeightZero() // �ø��� �ִϸ��̼� ������
    {
        animator.SetLayerWeight(1, 0f);
        //TODO : �������� �ִϸ��̼� �� ī�޶� ����.
    }
}