using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 0.0f;
    public float AttackPower { get; set; } = 0.0f;
    public float MoveSpeed { get; set; } = 0.0f;
    public float SearchRange { get; set; } = 0.0f;

    private eMONSTERSTATE currentState;

    protected Transform player; // �÷��̾��� Transform
    protected Animator animator;

    private float attackCooldown = 2.0f;
    private float lastAttackTime = 0.0f;

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
                    yield return Attack();
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
    public virtual IEnumerator Attack()//����
    {
        SetAnimationState(2);// 2��: Attack �ִϸ��̼� ��ȯ
        Debug.Log("Monster attacks with power: " + AttackPower);
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.5f); // �ִϸ��̼� ��� (�ִϸ��̼��� ���� ������ ��ٸ�)

        SetAnimationState(0); //0��: �ִϸ��̼� IDLE�� ��ȯ

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
        SetAnimationState(0);// 0��: Idle �ִϸ��̼� ����
        Debug.Log("Idle");
        yield return new WaitForSeconds(0.5f);
        Search();
    }
    

    protected virtual IEnumerator HitRoutine()//���� ����
    {
        Health -= AttackPower;
        SetAnimationState(3); // 3��: Hit �ִϸ��̼� ��ȯ
        Debug.Log("Hit Start");

        if (Health <= 0)
        {
            ChangeState(eMONSTERSTATE.DIE);
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // 1�� ���
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

    protected virtual void Search()//Ž��
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= SearchRange)
            {
                ChangeState(eMONSTERSTATE.CHASE);
            }
        }
        else
        {
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }

    protected virtual IEnumerator Chase()//����
    {
        while (currentState == eMONSTERSTATE.CHASE)
        {
            if (player != null)
            {
                // �÷��̾ ���� �̵�
                Vector3 direction = (player.position - transform.position).normalized;

                //���Ͱ� �÷��̾ �ٶ󺸰� ȸ��
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f); // �ε巯�� ȸ��

                // �÷��̾���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= 1.5f)
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



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
        if(other.CompareTag("PlayerWeapon"))
        {
            Debug.Log("change Hit");
            ChangeState(eMONSTERSTATE.HIT); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
