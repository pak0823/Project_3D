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
    public float JumpHeight = 5f; // ���� ����
    public float Gravity = -9.81f; // �߷�

    private bool isAttacking = false;
    private float verticalVelocity; // ���� �ӵ�

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
    private void SetAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//���º���
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
        else if (Input.anyKey) // Ű �Է��� �����Ͽ� MOVE ���·� ��ȯ
        {
            ChangeState(ePLAYERSTATE.MOVE);
        }
        yield return null;
    }

    private IEnumerator MoveRoutine()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0; // ���� �� ���� �ӵ� �ʱ�ȭ
            HandleMovement();
            HandleJump();
        }
        else
        {
            verticalVelocity += Gravity * Time.deltaTime; // �߷� ����
        }

        Dir.y = verticalVelocity; // ���� �ӵ��� ���� ���Ϳ� �߰�
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
        yield return new WaitForSeconds(0.65f); // �ִϸ��̼� ��� �ð�
        isAttacking = false;
        ChangeState(ePLAYERSTATE.IDLE);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ����
        {
            Debug.Log("Jump");
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); // ���� �ӵ� ���
        }
    }
}
