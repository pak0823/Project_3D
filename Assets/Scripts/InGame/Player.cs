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
    public float JumpHeight = 5f; // ���� ����
    public float Gravity = -9.81f; // �߷�
    private float verticalVelocity; // ���� �ӵ�

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
    private void SetAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        Animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//���º���
    {
        currentState = newState;
    }

    IEnumerator IdleRoutine()
    {
        SetAnimationState(0);
        ChangeState(ePLAYERSTATE.MOVE);

        if (Input.GetMouseButtonDown(0) && !isAttacking) // ���� Ŭ�� �� ���� ���·� ��ȯ
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }

        yield return null;
    }

    IEnumerator MoveRoutine()
    {
        if (CharacterController.isGrounded)//ĳ���Ͱ� ���鿡 �ִ� ���
        {
            verticalVelocity = 0; // ���� �� ���� �ӵ� �ʱ�ȭ
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            Jump();

            Dir = new Vector3(h, 0, v).normalized * MoveSpeed;

            if (Dir != Vector3.zero)
            {
                SetAnimationState(1);
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
                //���� �������� ĳ���� ȸ��
                //Mathf.Atan2(h, v): �� ���� ���ڸ� �޾� ��ũź��Ʈ ���� ���, �־��� �� ���� ������ ������� ������ ��ȯ. ��ȯ�Ǵ� ������ ���� ����
                //Mathf.Rad2Deg: ������ ��(degree)�� ��ȯ�ϴ� ���
                //Quaternion.Euler(x, y, z): �־��� ������ ����Ͽ� Quaternion�� ����. �������� ���� x��, y��, z���� �������� �ϴ� ȸ���� ��Ÿ��.
            }
            else
            {
                ChangeState(ePLAYERSTATE.IDLE);
            }
        }
        else
        {
            verticalVelocity += Gravity * Time.deltaTime; // ���߿��� �߷� ����
        }
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }

        Dir.y = verticalVelocity; // ���� �ӵ��� ���� ���Ϳ� �߰�
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
        yield return new WaitForSeconds(0.65f); //�ִϸ��̼� ��� �ð�
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ����
        {
            Debug.Log("Jump");
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); // ���� �ӵ� ���
            Debug.Log(verticalVelocity);
        }
    }
}
