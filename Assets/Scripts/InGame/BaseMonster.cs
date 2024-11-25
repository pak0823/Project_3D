using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 0.0f;
    public float AttackPower { get; set; } = 0.0f;
    public float MoveSpeed { get; set; } = 0.0f;
    public float SearchRange { get; set; } = 0.0f;

    private eMonsterState currentState;

    private Transform player; // �÷��̾��� Transform

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
                    yield return Chase(); // ���� ����
                    break;
                case eMonsterState.ATTACK:
                    Attack();
                    break;
                case eMonsterState.HIT:
                    yield return HitRoutine();
                    break;
                case eMonsterState.DIE:
                    Die();
                    yield break; // ���°� DIE�� ��� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
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
        // �̵� ���� (���⼭�� �ܼ��� ���)
        yield return null;
    }

    public virtual void Attack()//����
    {
        Debug.Log("Monster attacks with power: " + AttackPower);
        // ���� �� HIT ���·� ��ȯ
        ChangeState(eMonsterState.HIT);
    }

    protected virtual IEnumerator HitRoutine()//���� ����
    {
        Debug.Log("Monster got hit!");
        // ���ظ� �޴� ���� (��: ü�� ����)
        Health -= 20f;

        if (Health <= 0)
        {
            ChangeState(eMonsterState.DIE);
        }
        else
        {
            yield return new WaitForSeconds(1f); // 1�� ���
            ChangeState(eMonsterState.IDLE);
        }
    }

    protected virtual void Die()//���
    {
        Debug.Log("Monster is Dead");
        // ���� ó�� ����
        // ��: ���� ������Ʈ ��Ȱ��ȭ �Ǵ� ����
        gameObject.SetActive(false);
    }

    protected void ChangeState(eMonsterState newState)//���º���
    {
        currentState = newState;
    }

    public virtual void Move(Vector3 direction)//�̵�
    {
        // �̵� ���� ����
        Vector3 moveDirection = direction.normalized * Time.deltaTime;
        moveDirection.y = 0;
        transform.Translate(moveDirection);
    }

    protected virtual IEnumerator Chase()//����
    {
        Debug.Log("Chase");
        while (currentState == eMonsterState.CHASE)
        {
            if (player != null)
            {
                // �÷��̾ ���� �̵�
                Vector3 direction = (player.position - transform.position).normalized;
                Move(direction); // Move �޼��� ȣ��
                yield return null; // ���� �����ӱ��� ���
            }
            else
            {
                ChangeState(eMonsterState.IDLE); // �÷��̾ ������ ��� ����
                yield break; // �ڷ�ƾ ����
            }
        }
    }

    protected virtual void Search()//Ž��
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
