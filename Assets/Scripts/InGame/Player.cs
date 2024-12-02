using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 Dir;
    ePLAYERSTATE currentState;

    public float MoveSpeed = 8f;
    public float Health = 100f;
    public float AttackPower = 5f;
    bool isAttacking = false;
    public float JumpHeight = 5f; // 점프 높이
    public float Gravity = -9.81f; // 중력
    private float verticalVelocity; // 수직 속도

    Animator Animator;
    CharacterController CharacterController;

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();

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
        Animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//상태변경
    {
        currentState = newState;
    }

    IEnumerator IdleRoutine()
    {
        SetAnimationState(0);
        ChangeState(ePLAYERSTATE.MOVE);

        if (Input.GetMouseButtonDown(0) && !isAttacking) // 왼쪽 클릭 시 공격 상태로 전환
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }

        yield return null;
    }

    IEnumerator MoveRoutine()
    {
        if (CharacterController.isGrounded)//캐릭터가 지면에 있는 경우
        {
            verticalVelocity = 0; // 착지 시 수직 속도 초기화
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            Jump();

            Dir = new Vector3(h, 0, v).normalized * MoveSpeed;

            if (Dir != Vector3.zero)
            {
                SetAnimationState(1);
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
                //진행 방향으로 캐릭터 회전
                //Mathf.Atan2(h, v): 두 개의 인자를 받아 아크탄젠트 값을 계산, 주어진 두 값의 비율을 기반으로 각도를 반환. 반환되는 각도는 라디안 단위
                //Mathf.Rad2Deg: 라디안을 도(degree)로 변환하는 상수
                //Quaternion.Euler(x, y, z): 주어진 각도를 사용하여 Quaternion을 생성. 각도들은 각각 x축, y축, z축을 기준으로 하는 회전을 나타냄.
            }
            else
            {
                ChangeState(ePLAYERSTATE.IDLE);
            }
        }
        else
        {
            verticalVelocity += Gravity * Time.deltaTime; // 공중에서 중력 적용
        }
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }

        Dir.y = verticalVelocity; // 수직 속도를 방향 벡터에 추가
        CharacterController.Move(Dir * Time.deltaTime);
        yield return null;
    }
    IEnumerator Attack()
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        SetAnimationState(2);
        isAttacking = false;
        ChangeState(ePLAYERSTATE.IDLE);
        yield return new WaitForSeconds(0.65f); //애니메이션 대기 시간
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
        {
            Debug.Log("Jump");
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); // 점프 속도 계산
            Debug.Log(verticalVelocity);
        }
    }
}
