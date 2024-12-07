using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private Vector3 direction;
    private ePLAYERSTATE currentState;
    private BaseMonster basemonster;

    // Player settings
    public float moveSpeed = 6f;
    public float health = 100f;
    public float attackPower = 100f;
    public float jumpForce = 5f; // ���� ��
    public LayerMask groundLayer; // �� ���̾�

    private bool isAttacking = false;
    private bool isGrounded = true; // ���� ��� �ִ��� ����

    private Animator animator;
    private Rigidbody rigidBody;
    private BoxCollider weaponCollider; // ������ �ݶ��̴�

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        weaponCollider = GetComponentInChildren<BoxCollider>(); // ���Ⱑ �ڽ� ������Ʈ�� ���� ���

        currentState = ePLAYERSTATE.IDLE;
        StartCoroutine(StateMachine());
    }

    ///// ���� ���� //////





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
                        yield return AttackRoutine();
                        break;
                //case ePLAYERSTATE.DEFEND:
                //      yield return DefendRoutine();
                //      break;
                case ePLAYERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case ePLAYERSTATE.DIE:
                        yield return Die();
                        break;
            }
            yield return null;
        }
    }

    private void ChangeAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//���º���
    {
        currentState = newState;
    }

    private IEnumerator HitRoutine()
    {
        health -= basemonster.AttackPower;
        Debug.Log($"player_Health{health}");
        ChangeAnimationState(3);

        if(health <= 0)
            ChangeState(ePLAYERSTATE.DIE);
        else
            ChangeState(ePLAYERSTATE.IDLE);

        yield return WaitForSeconds(0.3f);
    }

    private IEnumerator Die()
    {
        ChangeAnimationState(10);
        yield return WaitForSeconds(2f);
        StopAllCoroutines();
    }

    private IEnumerator WaitForSeconds(float _seconds)
    {
        WaitForSeconds WaitForSeconds;
        WaitForSeconds = new WaitForSeconds(_seconds);

        yield return WaitForSeconds;
    }


    ///// �浹ó�� //////


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.CompareTag("Monster"))
        {
            basemonster = other.GetComponentInParent<BaseMonster>();
            ChangeState(ePLAYERSTATE.HIT);
        }
            
    }

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
