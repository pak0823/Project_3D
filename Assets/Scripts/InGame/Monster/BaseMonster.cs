using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{

    //Monster Setting
    public float Health { get; set; } = 100.0f; // 기본 체력
    public float AttackPower { get; set; } = 10.0f; // 기본 공격력
    public float MoveSpeed { get; set; } = 2.0f; // 기본 이동 속도
    public float SearchRange { get; set; } = 1.0f; // 기본 탐색 범위

    //컴포넌트
    protected Player player;
    protected Animator animator;    
    private Rigidbody Rigidbody;



    //enum
    private eMONSTERSTATE currentState;


    //변수
    private float attackCooldown = 0;
    bool isHit = false;


    ///////////

    protected void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
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
                    CreateRoutine();
                    break;
                case eMONSTERSTATE.IDLE:
                    yield return IdleRoutine();
                    break;
                case eMONSTERSTATE.CHASE:
                    yield return ChaseRoutine();
                    break;
                case eMONSTERSTATE.ATTACK:
                    AttackRoutine();
                    break;
                case eMONSTERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case eMONSTERSTATE.DIE:
                    yield return DieRoutine();
                    break;
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    ///////////




    ///// 생성 & 초기동작 //////

    protected virtual void CreateRoutine() // 몬스터 생성 시 기본 상태
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }

    protected IEnumerator IdleRoutine()
    {
        ChangeAnimationState(0);// 0번: Idle 애니메이션 실행

        while (currentState == eMONSTERSTATE.IDLE)
        {
            Search();
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected virtual void Search()//탐색
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SearchRange, LayerMask.GetMask("Player")); // "Player" 레이어를 설정

        if (hitColliders.Length > 0)
        {
            // 플레이어가 범위 안에 들어온 경우
            player = hitColliders[0].GetComponent<Player>(); // 첫 번째 플레이어를 가져옴
            ChangeState(eMONSTERSTATE.CHASE); // Chase 상태로 전환
        }
        else
        {
            // 플레이어가 범위 밖에 있는 경우
            if (player != null) // 이전에 플레이어가 있었던 경우에만 상태 변경
            {
                player = null; // 플레이어를 null로 설정
                ChangeState(eMONSTERSTATE.IDLE); // Idle 상태로 전환
            }
        }
    }


    ///// 이동 //////

    protected virtual IEnumerator ChaseRoutine()// 플레이어 추적
    {
        float searchInterval = 1f; // 탐색 간격 설정
        float nextSearchTime = Time.time;

        while (currentState == eMONSTERSTATE.CHASE)
        {
            // 정해진 간격마다 Search 호출
            if (Time.time >= nextSearchTime)
            {
                Search();
                nextSearchTime = Time.time + searchInterval; // 다음 탐색 시간 업데이트
            }

            if (player == null)
            {
                ChangeState(eMONSTERSTATE.IDLE); // 플레이어가 없으면 대기 상태로 전환
                yield break; // 코루틴 종료
            }

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
            Move(direction); // Move 메서드 호출
            yield return null; // 다음 프레임까지 대기
        }
    }
    public virtual void Move(Vector3 direction)//이동
    {
        // 이동 로직 구현
        Vector3 moveDirection = direction.normalized * MoveSpeed;
        //moveDirection.y = Rigidbody.velocity.y; // 기존의 y 속도를 유지하여 중력 적용
        Rigidbody.velocity = moveDirection; // Rigidbody의 속도로 이동

    }

    ///// 공격 부분 //////

    public virtual void AttackRoutine()//공격
    {
        if(attackCooldown <= 0)
        {
            ChangeAnimationState(2);// 2번: Attack 애니메이션 전환
            attackCooldown = 2f;
            StartCoroutine(AttackCoolTime());
            Debug.Log("Attack");
        }
    }

    protected virtual bool IsInAttackRange(float distanceToPlayer)
    {
        float attackRange = 1.5f; // 공격 거리 설정
        return distanceToPlayer <= attackRange;
    }

    IEnumerator AttackCoolTime()
    {
        while (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    ///// Hit & Die //////

    protected IEnumerator HitRoutine()
    {
        if(!isHit)
        {
            isHit = true;
            //if(currentState != eMONSTERSTATE.ATTACK)
            //    ChangeAnimationState(3); // Hit 애니메이션 전환

            Health -= player.attackPower; // 체력 감소
            Debug.Log($"Current Health: {Health}");

            if (Health <= 0)
            {
                Debug.Log("Monster has died.");
                ChangeState(eMONSTERSTATE.DIE); // 사망 처리
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                isHit = false; // 맞는 상태 해제
                ChangeState(eMONSTERSTATE.IDLE); // Idle 상태로 전환
            }
        }
    }

    protected virtual IEnumerator DieRoutine()//사망
    {
        foreach (Transform child in transform)
        {
            child.gameObject.tag = "MonsterDie"; // 자식 오브젝트의 태그 변경
        }
        ChangeAnimationState(5); // Die 애니메이션 전환
        Rigidbody.isKinematic = true; // Rigidbody를 비활성화하여 움직이지 않도록 함
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    ///// 상태 전환 //////

    protected void ChangeState(eMONSTERSTATE newState)//기본 상태 전환
    {
        if (currentState != newState)   //중복되는 상태를 적용하지 않게 하기위해 추가
        {
            currentState = newState;
        }
    }

    protected void ChangeAnimationState(int state)    //애니메이션 상태 전환
    {
        animator.SetInteger("AnimationState", state);
    }


    ///// 충돌 처리 //////

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerWeapon"))
        {
            Player playerCompent = other.GetComponentInParent<Player>();
            if (playerCompent != null)
            {
                if (currentState != eMONSTERSTATE.HIT) // HIT 상태가 아닐 때만 반응
                {
                    ChangeState(eMONSTERSTATE.HIT);
                }
            }
            else
                Debug.LogError("Player component not found!");
        }
    }


    ///// 기타 //////

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SearchRange);
    }
}
