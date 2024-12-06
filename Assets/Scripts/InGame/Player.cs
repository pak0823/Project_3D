using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 Direction;
    private ePLAYERSTATE currentState;

    // Player settings
    public float MoveSpeed = 6f;
    public float Health = 100f;
    public float AttackPower = 100f;
    public float JumpForce = 5f; // 점프 힘
    public LayerMask groundLayer; // 땅 레이어

    private bool isAttacking = false;
    private bool isGrounded = true; // 땅에 닿아 있는지 상태

    private Animator animator;
    private Rigidbody Rigidbody;
    private BoxCollider weaponCollider; // 무기의 콜라이더

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponentInChildren<BoxCollider>(); // 무기가 자식 오브젝트로 있을 경우

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


    ///// 이동 조작 //////

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
        HandleMovement();
        HandleJump();
        //Defend();
        yield return null;
    }

    private void HandleMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        bool ShiftRun = Input.GetButton("LeftShift");

        Direction = new Vector3(Horizontal, 0, Vertical).normalized * MoveSpeed;

        if (Direction != Vector3.zero)
        {
            SetAnimationState(1);
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(Horizontal, Vertical) * Mathf.Rad2Deg, 0);
            Rigidbody.MovePosition(Rigidbody.position + Direction * Time.deltaTime);
        }
        else
        {
            ChangeState(ePLAYERSTATE.IDLE);
        }

        if (ShiftRun)
        {
            SetAnimationState(6);
            Direction = Direction * 0.3f;
        }
            
    }

    private void HandleJump()
    {
        // 땅에 닿아 있을 때만 점프 가능
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
        {
            Debug.Log("Jump");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse); // 점프 힘을 적용
        }
    }


    ///// 공격동작 및 방어동작 //////

    private IEnumerator Attack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        SetAnimationState(2);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(0.65f); // 애니메이션 대기 시간
        isAttacking = false;
        weaponCollider.enabled = false;
        ChangeState(ePLAYERSTATE.IDLE);
    }

    private void Defend()
    {
        if(Input.GetMouseButtonDown(1))
            SetAnimationState(4);
    }

    ///// 상태 세팅 //////

    private void SetAnimationState(int state)    //애니메이션 상태 세팅
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//상태변경
    {
        currentState = newState;
    }



    ///// 충돌처리 //////

    private void OnCollisionEnter(Collision collision)
    {
        //layerMask는 레이어를 비트로 판단하기 때문에 (1 << collision.gameObject.layer)와 같이 설정해야 한다.
        // 땅에 닿았는지 확인
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 땅에서 떨어졌을 때
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
