using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100.0f; // �⺻ ü��
    public float AttackPower { get; set; } = 10.0f; // �⺻ ���ݷ�
    public float MoveSpeed { get; set; } = 2.0f; // �⺻ �̵� �ӵ�
    public float SearchRange { get; set; } = 5.0f; // �⺻ Ž�� ����

    private eMONSTERSTATE currentState;

    protected Player player; // �÷��̾��� Transform
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
                    yield return Chase(); // ���� ����
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
            yield return null; // ���� �����ӱ��� ���
        }
    }

    protected void ChangeState(eMONSTERSTATE newState)//���º���
    {
        if (currentState != newState)   //�ߺ��Ǵ� ���¸� �������� �ʰ� �ϱ����� �߰�
        {
            currentState = newState;
        }
    }

    protected void ChangeAnimationState(int state)    //�ִϸ��̼� ���� ����
    {
        animator.SetInteger("AnimationState", state);
    }



    protected virtual void Create()
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }


    public virtual IEnumerator Attack()//����
    {
        //if (currentState == eMONSTERSTATE.ATTACK) // ���� �߿��� �ٸ� ���·� �������� ����
        //    yield break;

        ChangeState(eMONSTERSTATE.ATTACK);
        ChangeAnimationState(2);// 2��: Attack �ִϸ��̼� ��ȯ
        yield return new WaitForSeconds(0.5f); // �ִϸ��̼� ��� (�ִϸ��̼��� ���� ������ ��ٸ�)
        ChangeAnimationState(0); //0��: �ִϸ��̼� IDLE�� ��ȯ

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
        ChangeAnimationState(0);// 0��: Idle �ִϸ��̼� ����

        while(currentState == eMONSTERSTATE.IDLE)
        {
            Search();
            yield return new WaitForSeconds(0.5f);
        }
    }
    

    protected IEnumerator HitRoutine()//���� ����
    {
        if (!ishit)
        {
            ishit = true;
            ChangeAnimationState(3); // 3��: Hit �ִϸ��̼� ��ȯ
            Health -= player.AttackPower;
            Debug.Log($"Current Health: {Health}");

            if (Health <= 0)
            {
                Debug.Log("Monster has died.");
                ChangeState(eMONSTERSTATE.DIE);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); //���
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

    protected virtual IEnumerator Die()//���
    {
        ChangeAnimationState(10);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public virtual void Move(Vector3 direction)//�̵�
    {
        // �̵� ���� ����
        Vector3 moveDirection = direction;
        moveDirection.y = 0;
        //transform.Translate(moveDirection * MoveSpeed * Time.deltaTime, Space.World); // MoveSpeed�� ���Ͽ� �ӵ� �ݿ�
        characterController.Move(moveDirection * MoveSpeed * Time.deltaTime);

    }

    protected virtual void Search()//Ž��
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= SearchRange)//�÷��̾ ���� �ȿ� ������ Chase ���·� ��ȯ
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

    protected virtual IEnumerator Chase()//����
    {
        while (currentState == eMONSTERSTATE.CHASE)
        {
            if (player != null)
            {
                // �÷��̾ ���� �̵�
                Vector3 direction = (player.transform.position - transform.position).normalized;

                //���Ͱ� �÷��̾ �ٶ󺸰� ȸ��
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f); // �ε巯�� ȸ��

                // �÷��̾���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (IsInAttackRange(distanceToPlayer))
                {
                    ChangeState(eMONSTERSTATE.ATTACK); // ���� ���·� ��ȯ
                    yield break; // ���� �ڷ�ƾ ����
                }

                ChangeAnimationState(1);// 1��: Move �ִϸ��̼� ����
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

    protected virtual bool IsInAttackRange(float distanceToPlayer)
    {
        float attackRange = 1.5f; // ���� �Ÿ� ����
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
