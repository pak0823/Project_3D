using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 0.0f;
    public float AttackPower { get; set; } = 0.0f;
    public float MoveSpeed { get; set; } = 0.0f;
    public float SearchRange { get; set; } = 0.0f;

    private eMonsterState currentState;

    private Transform player; // 플레이어의 Transform

    protected void Start()
    {
        currentState = eMonsterState.CREATE;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case eMonsterState.CREATE:
                    Create();
                    break;
                case eMonsterState.IDLE:
                    yield return IdleRoutine();
                    break;
                case eMonsterState.MOVE:
                    yield return MoveRoutine();
                    break;
                case eMonsterState.CHASE:
                    yield return Chase(); // 추적 상태
                    break;
                case eMonsterState.ATTACK:
                    Attack();
                    break;
                case eMonsterState.HIT:
                    yield return HitRoutine();
                    break;
                case eMonsterState.DIE:
                    Die();
                    yield break; // 상태가 DIE일 경우 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    protected virtual void Create()
    {
        Debug.Log("Monster Created");
        ChangeState(eMonsterState.IDLE);
    }

    protected IEnumerator IdleRoutine()
    {
        Debug.Log("Monster is Idle");
        yield return new WaitForSeconds(1.0f);
        Search();
    }

    protected IEnumerator MoveRoutine()
    {
        Debug.Log("Monster is Moving");
        // 이동 로직 (여기서는 단순히 대기)
        yield return null;
    }

    public virtual void Attack()//공격
    {
        Debug.Log("Monster attacks with power: " + AttackPower);
        // 공격 후 HIT 상태로 전환
        ChangeState(eMonsterState.HIT);
    }

    protected virtual IEnumerator HitRoutine()//공격 피해
    {
        Debug.Log("Monster got hit!");
        // 피해를 받는 로직 (예: 체력 감소)
        Health -= 20f;

        if (Health <= 0)
        {
            ChangeState(eMonsterState.DIE);
        }
        else
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            ChangeState(eMonsterState.IDLE);
        }
    }

    protected virtual void Die()//사망
    {
        Debug.Log("Monster is Dead");
        // 죽음 처리 로직
        // 예: 게임 오브젝트 비활성화 또는 제거
        gameObject.SetActive(false);
    }

    protected void ChangeState(eMonsterState newState)//상태변경
    {
        currentState = newState;
    }

    public virtual void Move(Vector3 direction)//이동
    {
        // 이동 로직 구현
        Vector3 moveDirection = direction.normalized * Time.deltaTime;
        moveDirection.y = 0;
        transform.Translate(moveDirection);
    }

    protected virtual IEnumerator Chase()//추적
    {
        Debug.Log("Chase");
        while (currentState == eMonsterState.CHASE)
        {
            if (player != null)
            {
                // 플레이어를 향해 이동
                Vector3 direction = (player.position - transform.position).normalized;
                Move(direction); // Move 메서드 호출
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                ChangeState(eMonsterState.IDLE); // 플레이어가 없으면 대기 상태
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
                ChangeState(eMonsterState.CHASE);
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
