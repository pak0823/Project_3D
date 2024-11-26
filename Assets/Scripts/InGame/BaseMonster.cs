using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 0.0f;
    public float AttackPower { get; set; } = 0.0f;
    public float MoveSpeed { get; set; } = 0.0f;
    public float SearchRange { get; set; } = 0.0f;

    private eMONSTERSTATE currentState;

    private Transform player; // 플레이어의 Transform
    private Animator animator;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        currentState = eMONSTERSTATE.CREATE;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case eMONSTERSTATE.CREATE:
                    Create();
                    break;
                case eMONSTERSTATE.IDLE:
                    yield return IdleRoutine();
                    break;
                case eMONSTERSTATE.CHASE:
                    yield return Chase(); // 추적 상태
                    break;
                case eMONSTERSTATE.ATTACK:
                    Attack();
                    break;
                case eMONSTERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case eMONSTERSTATE.DIE:
                    Die();
                    yield break; // 상태가 DIE일 경우 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    protected void ChangeState(eMONSTERSTATE newState)//상태변경
    {
        currentState = newState;
    }

    protected void SetAnimationState(int state)    //애니메이션 상태 세팅
    {
        animator.SetInteger("AnimationState", state);
    }



    protected virtual void Create()
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }
    public virtual void Attack()//공격
    {
        Debug.Log("Monster attacks with power: " + AttackPower);
        SetAnimationState(2);// 2번: Attack 애니메이션 실행

        ChangeState(eMONSTERSTATE.CHASE);
    }

    protected IEnumerator IdleRoutine()
    {
        SetAnimationState(0);// 0번: Idle 애니메이션 실행
        yield return new WaitForSeconds(0.5f);
        Search();
    }
    


    protected virtual IEnumerator HitRoutine()//공격 피해
    {
        Health -= 20f;

        if (Health <= 0)
        {
            ChangeState(eMONSTERSTATE.DIE);
        }
        else
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }

    protected virtual void Die()//사망
    {
        gameObject.SetActive(false);
    }

    public virtual void Move(Vector3 direction)//이동
    {
        // 이동 로직 구현

        Vector3 moveDirection = direction;
        moveDirection.y = 0;
        transform.Translate(moveDirection, Space.World);
    }

    protected virtual IEnumerator Chase()//추적
    {
        Debug.Log("Chase");
        while (currentState == eMONSTERSTATE.CHASE)
        {
            if (player != null)
            {
                // 플레이어를 향해 이동
                Vector3 direction = (player.position - transform.position).normalized;

                //몬스터가 플레이어를 바라보게 회전
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4f); // 부드러운 회전

                // 플레이어와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= 2f)
                {
                    ChangeState(eMONSTERSTATE.ATTACK); // 공격 상태로 전환
                    yield break; // 추적 코루틴 종료
                }

                SetAnimationState(1);// 1번: Move 애니메이션 실행
                Move(direction * MoveSpeed * Time.deltaTime); // Move 메서드 호출
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                ChangeState(eMONSTERSTATE.IDLE); // 플레이어가 없으면 대기 상태
                yield break; // 코루틴 종료
            }
        }
    }

    protected virtual void Search()//탐색
    {
        Debug.Log("Search");
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= SearchRange)
            {
                ChangeState(eMONSTERSTATE.CHASE);
            }
        }
        else
            Debug.Log("Search null");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
