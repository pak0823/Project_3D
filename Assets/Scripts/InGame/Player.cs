using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 Dir;
    private ePLAYERSTATE currentState;

    // Player settings
    public float MoveSpeed = 8f;
    public float Health = 100f;
    public float AttackPower = 20f;
    public float JumpHeight = 5f; // 점프 높이
    public float Gravity = -9.81f; // 중력

    private bool isAttacking = false;
    private float verticalVelocity; // 수직 속도

    private Animator animator;
    private CharacterController characterController;

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        currentState = ePLAYERSTATE.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case ePLAYERSTATE.IDLE:
                        yield return IdleRoutine();
                        break;
                     
                case ePLAYERSTATE.MOVE:
                        yield return MoveRoutine();
                        break;
                case ePLAYERSTATE.ATTACK:
                        yield return Attack();
                        break;
            }
            yield return null;
        }
    }
    private void SetAnimationState(int state)    //애니메이션 상태 세팅
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//상태변경
    {
        currentState = newState;
    }

    private IEnumerator IdleRoutine()
    {
        SetAnimationState(0);
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }
        else if (Input.anyKey) // 키 입력을 감지하여 MOVE 상태로 전환
        {
            ChangeState(ePLAYERSTATE.MOVE);
        }
        yield return null;
    }

    private IEnumerator MoveRoutine()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0; // 착지 시 수직 속도 초기화
            HandleMovement();
            HandleJump();
        }
        else
        {
            verticalVelocity += Gravity * Time.deltaTime; // 중력 적용
        }

        Dir.y = verticalVelocity; // 수직 속도를 방향 벡터에 추가
        characterController.Move(Dir * Time.deltaTime);
        yield return null;
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Dir = new Vector3(h, 0, v).normalized * MoveSpeed;

        if (Dir != Vector3.zero)
        {
            SetAnimationState(1);
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        }
        else
        {
            ChangeState(ePLAYERSTATE.IDLE);
        }
    }


    private IEnumerator Attack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        SetAnimationState(2);
        yield return new WaitForSeconds(0.65f); // 애니메이션 대기 시간
        isAttacking = false;
        ChangeState(ePLAYERSTATE.IDLE);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
        {
            Debug.Log("Jump");
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); // 점프 속도 계산
        }
    }
}
