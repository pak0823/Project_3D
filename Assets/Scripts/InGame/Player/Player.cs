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
    public float jumpForce = 5f; // 점프 힘
    public LayerMask groundLayer; // 땅 레이어

    private bool isAttacking = false;
    private bool isGrounded = true; // 땅에 닿아 있는지 상태

    private Animator animator;
    private Rigidbody rigidBody;
    private BoxCollider weaponCollider; // 무기의 콜라이더

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        weaponCollider = GetComponentInChildren<BoxCollider>(); // 무기가 자식 오브젝트로 있을 경우

        currentState = ePLAYERSTATE.IDLE;
        StartCoroutine(StateMachine());
    }

    ///// 상태 세팅 //////





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

    private void ChangeAnimationState(int state)    //애니메이션 상태 세팅
    {
        animator.SetInteger("AnimationState", state);
    }
    private void ChangeState(ePLAYERSTATE newState)//상태변경
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


    ///// 충돌처리 //////


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
