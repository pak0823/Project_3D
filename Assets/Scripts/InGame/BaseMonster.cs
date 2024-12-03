using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100.0f; // 기본 체력
    public float AttackPower { get; set; } = 10.0f; // 기본 공격력
    public float MoveSpeed { get; set; } = 2.0f; // 기본 이동 속도
    public float SearchRange { get; set; } = 5.0f; // 기본 탐색 범위

    private eMONSTERSTATE currentState;

    protected Player player; // 플레이어의 Transform
    protected Animator animator;
    CharacterController characterController;


    private float attackCooldown = 2.0f;
    bool ishit = false;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        currentState = eMONSTERSTATE.CREATE;
        StartCoroutine(StateMachine());
        //Debug.Log($"Hp:{Health.ToString()}, Power:{AttackPower.ToString()}, Speed:{MoveSpeed.ToString()}, Search{SearchRange.ToString()}");
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
                    yield return Attack();
                    break;
                case eMONSTERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case eMONSTERSTATE.DIE:
                    yield return Die();
                    break;
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    protected void ChangeState(eMONSTERSTATE newState)//상태변경
    {
        if (currentState != newState)   //중복되는 상태를 적용하지 않게 하기위해 추가
        {
            currentState = newState;
        }
    }

    protected void ChangeAnimationState(int state)    //애니메이션 상태 세팅
    {
        animator.SetInteger("AnimationState", state);
    }



    protected virtual void Create()
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }


    public virtual IEnumerator Attack()//공격
    {
        //if (currentState == eMONSTERSTATE.ATTACK) // 공격 중에는 다른 상태로 변경하지 않음
        //    yield break;

        ChangeState(eMONSTERSTATE.ATTACK);
        ChangeAnimationState(2);// 2번: Attack 애니메이션 전환
        yield return new WaitForSeconds(0.5f); // 애니메이션 대기 (애니메이션이 끝날 때까지 기다림)
        ChangeAnimationState(0); //0번: 애니메이션 IDLE로 전환

        if (player != null)
        {
            ChangeState(eMONSTERSTATE.CHASE);
        }
        else
            ChangeState(eMONSTERSTATE.IDLE);

        yield return new WaitForSeconds(attackCooldown);
    }

    protected IEnumerator IdleRoutine()
    {
        ChangeAnimationState(0);// 0번: Idle 애니메이션 실행

        while(currentState == eMONSTERSTATE.IDLE)
        {
            Search();
            yield return new WaitForSeconds(0.5f);
        }
    }
    

    protected IEnumerator HitRoutine()//공격 피해
    {
        if (!ishit)
        {
            ishit = true;
            ChangeAnimationState(3); // 3번: Hit 애니메이션 전환
            Health -= player.AttackPower;
            Debug.Log($"Current Health: {Health}");

            if (Health <= 0)
            {
                Debug.Log("Monster has died.");
                ChangeState(eMONSTERSTATE.DIE);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); //대기
                ChangeState(eMONSTERSTATE.IDLE);
            }

            ishit = false;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }

    protected virtual IEnumerator Die()//사망
    {
        ChangeAnimationState(10);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public virtual void Move(Vector3 direction)//이동
    {
        // 이동 로직 구현
        Vector3 moveDirection = direction;
        moveDirection.y = 0;
        //transform.Translate(moveDirection * MoveSpeed * Time.deltaTime, Space.World); // MoveSpeed를 곱하여 속도 반영
        characterController.Move(moveDirection * MoveSpeed * Time.deltaTime);

    }

    protected virtual void Search()//탐색
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= SearchRange)//플레이어가 범위 안에 들어오면 Chase 상태로 전환
            {
                ChangeState(eMONSTERSTATE.CHASE);
            }
        }
        else
        {
            Debug.Log("No Player detected.");
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }

    protected virtual IEnumerator Chase()//추적
    {
        while (currentState == eMONSTERSTATE.CHASE)
        {
            if (player != null)
            {
                // 플레이어를 향해 이동
                Vector3 direction = (player.transform.position - transform.position).normalized;

                //몬스터가 플레이어를 바라보게 회전
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f); // 부드러운 회전

                // 플레이어와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (IsInAttackRange(distanceToPlayer))
                {
                    ChangeState(eMONSTERSTATE.ATTACK); // 공격 상태로 전환
                    yield break; // 추적 코루틴 종료
                }

                ChangeAnimationState(1);// 1번: Move 애니메이션 실행
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

    protected virtual bool IsInAttackRange(float distanceToPlayer)
    {
        float attackRange = 1.5f; // 공격 거리 설정
        return distanceToPlayer <= attackRange;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            Debug.Log("player in");
        }
        else if(other.CompareTag("PlayerWeapon"))
        {
            Player playerCompent = other.GetComponentInParent<Player>();
            if (playerCompent != null)
            {
                Debug.Log("playerWeapon in");
                ChangeState(eMONSTERSTATE.HIT);
            }
            else
                Debug.LogError("Player component not found!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player out");
            player = null;
        }
    }
}
