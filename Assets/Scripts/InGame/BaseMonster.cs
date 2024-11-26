using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 0.0f;
    public float AttackPower { get; set; } = 0.0f;
    public float MoveSpeed { get; set; } = 0.0f;
    public float SearchRange { get; set; } = 0.0f;

    private eMONSTERSTATE currentState;

    private Transform player; // �÷��̾��� Transform
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
                    yield return Chase(); // ���� ����
                    break;
                case eMONSTERSTATE.ATTACK:
                    Attack();
                    break;
                case eMONSTERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case eMONSTERSTATE.DIE:
                    Die();
                    yield break; // ���°� DIE�� ��� �ڷ�ƾ ����
            }
            yield return null; // ���� �����ӱ��� ���
        }
    }

    protected void ChangeState(eMONSTERSTATE newState)//���º���
    {
        currentState = newState;
    }

    protected void SetAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        animator.SetInteger("AnimationState", state);
    }



    protected virtual void Create()
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }
    public virtual void Attack()//����
    {
        Debug.Log("Monster attacks with power: " + AttackPower);
        SetAnimationState(2);// 2��: Attack �ִϸ��̼� ����

        ChangeState(eMONSTERSTATE.CHASE);
    }

    protected IEnumerator IdleRoutine()
    {
        SetAnimationState(0);// 0��: Idle �ִϸ��̼� ����
        yield return new WaitForSeconds(0.5f);
        Search();
    }
    


    protected virtual IEnumerator HitRoutine()//���� ����
    {
        Health -= 20f;

        if (Health <= 0)
        {
            ChangeState(eMONSTERSTATE.DIE);
        }
        else
        {
            yield return new WaitForSeconds(1f); // 1�� ���
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }

    protected virtual void Die()//���
    {
        gameObject.SetActive(false);
    }

    public virtual void Move(Vector3 direction)//�̵�
    {
        // �̵� ���� ����

        Vector3 moveDirection = direction;
        moveDirection.y = 0;
        transform.Translate(moveDirection, Space.World);
    }

    protected virtual IEnumerator Chase()//����
    {
        Debug.Log("Chase");
        while (currentState == eMONSTERSTATE.CHASE)
        {
            if (player != null)
            {
                // �÷��̾ ���� �̵�
                Vector3 direction = (player.position - transform.position).normalized;

                //���Ͱ� �÷��̾ �ٶ󺸰� ȸ��
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4f); // �ε巯�� ȸ��

                // �÷��̾���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= 2f)
                {
                    ChangeState(eMONSTERSTATE.ATTACK); // ���� ���·� ��ȯ
                    yield break; // ���� �ڷ�ƾ ����
                }

                SetAnimationState(1);// 1��: Move �ִϸ��̼� ����
                Move(direction * MoveSpeed * Time.deltaTime); // Move �޼��� ȣ��
                yield return null; // ���� �����ӱ��� ���
            }
            else
            {
                ChangeState(eMONSTERSTATE.IDLE); // �÷��̾ ������ ��� ����
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
