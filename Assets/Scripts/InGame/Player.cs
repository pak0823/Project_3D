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
    public float JumpForce = 5f; // ���� ��
    public LayerMask groundLayer; // �� ���̾�

    private bool isAttacking = false;
    private bool isGrounded = true; // ���� ��� �ִ��� ����

    private Animator animator;
    private Rigidbody Rigidbody;
    private BoxCollider weaponCollider; // ������ �ݶ��̴�

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponentInChildren<BoxCollider>(); // ���Ⱑ �ڽ� ������Ʈ�� ���� ���

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


    ///// �̵� ���� //////

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
        // ���� ��� ���� ���� ���� ����
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ����
        {
            Debug.Log("Jump");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse); // ���� ���� ����
        }
    }


    ///// ���ݵ��� �� ���� //////

    private IEnumerator Attack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        SetAnimationState(2);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(0.65f); // �ִϸ��̼� ��� �ð�
        isAttacking = false;
        weaponCollider.enabled = false;
        ChangeState(ePLAYERSTATE.IDLE);
    }

    private void Defend()
    {
        if(Input.GetMouseButtonDown(1))
            SetAnimationState(4);
    }

    ///// ���� ���� //////

    private void SetAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//���º���
    {
        currentState = newState;
    }



    ///// �浹ó�� //////

    private void OnCollisionEnter(Collision collision)
    {
        //layerMask�� ���̾ ��Ʈ�� �Ǵ��ϱ� ������ (1 << collision.gameObject.layer)�� ���� �����ؾ� �Ѵ�.
        // ���� ��Ҵ��� Ȯ��
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ������ �������� ��
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
