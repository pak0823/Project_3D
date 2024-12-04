using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{

    //Monster Setting
    public float Health { get; set; } = 100.0f; // �⺻ ü��
    public float AttackPower { get; set; } = 10.0f; // �⺻ ���ݷ�
    public float MoveSpeed { get; set; } = 2.0f; // �⺻ �̵� �ӵ�
    public float SearchRange { get; set; } = 1.0f; // �⺻ Ž�� ����

    //������Ʈ
    protected Player player;
    protected Animator animator;    
    private Rigidbody Rigidbody;


    //enum
    private eMONSTERSTATE currentState;


    //����
    private float attackCooldown = 2.0f;
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
                    yield return AttackRoutine();
                    break;
                case eMONSTERSTATE.HIT:
                    yield return HitRoutine();
                    break;
                case eMONSTERSTATE.DIE:
                    yield return DieRoutine();
                    break;
            }
            yield return null; // ���� �����ӱ��� ���
        }
    }

    ///////////




    ///// ���� & �ʱ⵿�� //////

    protected virtual void CreateRoutine() // ���� ���� �� �⺻ ����
    {
        ChangeState(eMONSTERSTATE.IDLE);
    }

    protected IEnumerator IdleRoutine()
    {
        ChangeAnimationState(0);// 0��: Idle �ִϸ��̼� ����

        while (currentState == eMONSTERSTATE.IDLE)
        {
            Search();
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected virtual void Search()//Ž��
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SearchRange, LayerMask.GetMask("Player")); // "Player" ���̾ ����

        if (hitColliders.Length > 0)
        {
            // �÷��̾ ���� �ȿ� ���� ���
            player = hitColliders[0].GetComponent<Player>(); // ù ��° �÷��̾ ������
            ChangeState(eMONSTERSTATE.CHASE); // Chase ���·� ��ȯ
        }
        else
        {
            // �÷��̾ ���� �ۿ� �ִ� ���
            if (player != null) // ������ �÷��̾ �־��� ��쿡�� ���� ����
            {
                player = null; // �÷��̾ null�� ����
                ChangeState(eMONSTERSTATE.IDLE); // Idle ���·� ��ȯ
            }
        }
    }


    ///// �̵� //////

    protected virtual IEnumerator ChaseRoutine()// �÷��̾� ����
    {
        float searchInterval = 1f; // Ž�� ���� ����
        float nextSearchTime = Time.time;

        while (currentState == eMONSTERSTATE.CHASE)
        {
            // ������ ���ݸ��� Search ȣ��
            if (Time.time >= nextSearchTime)
            {
                Search();
                nextSearchTime = Time.time + searchInterval; // ���� Ž�� �ð� ������Ʈ
            }

            if (player == null)
            {
                ChangeState(eMONSTERSTATE.IDLE); // �÷��̾ ������ ��� ���·� ��ȯ
                yield break; // �ڷ�ƾ ����
            }
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
            Debug.Log("chaseing");
            yield return null; // ���� �����ӱ��� ���
        }
    }
    public virtual void Move(Vector3 direction)//�̵�
    {
        // �̵� ���� ����
        Vector3 moveDirection = direction.normalized * MoveSpeed;
        moveDirection.y = Rigidbody.velocity.y; // ������ y �ӵ��� �����Ͽ� �߷� ����
        Rigidbody.velocity = moveDirection; // Rigidbody�� �ӵ��� �̵�

    }

    ///// ���� �κ� //////

    public virtual IEnumerator AttackRoutine()//����
    {
        ChangeState(eMONSTERSTATE.ATTACK);
        ChangeAnimationState(2);// 2��: Attack �ִϸ��̼� ��ȯ
        yield return new WaitForSeconds(0.5f); // �ִϸ��̼� ��� (�ִϸ��̼��� ���� ������ ��ٸ�)
        ChangeAnimationState(0); //0��: �ִϸ��̼� IDLE�� ��ȯ

        ChangeState(player != null ? eMONSTERSTATE.CHASE : eMONSTERSTATE.IDLE);
        yield return new WaitForSeconds(attackCooldown);
    }

    protected virtual bool IsInAttackRange(float distanceToPlayer)
    {
        float attackRange = 1.5f; // ���� �Ÿ� ����
        return distanceToPlayer <= attackRange;
    }

    ///// Hit & Die //////

    protected IEnumerator HitRoutine()
    {
        if(!isHit)
        {
            isHit = true;
            ChangeAnimationState(3); // Hit �ִϸ��̼� ��ȯ
            Health -= player.AttackPower; // ü�� ����
            Debug.Log($"Current Health: {Health}");

            if (Health <= 0)
            {
                Debug.Log("Monster has died.");
                ChangeState(eMONSTERSTATE.DIE); // ��� ó��
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                ChangeState(eMONSTERSTATE.IDLE); // Idle ���·� ��ȯ
            }
        }

        isHit = false; // �´� ���� ����
    }

    protected virtual IEnumerator DieRoutine()//���
    {
        ChangeAnimationState(5); // Die �ִϸ��̼� ��ȯ
        Rigidbody.isKinematic = true; // Rigidbody�� ��Ȱ��ȭ�Ͽ� �������� �ʵ��� ��
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    ///// ���� ��ȯ //////

    protected void ChangeState(eMONSTERSTATE newState)//�⺻ ���� ��ȯ
    {
        if (currentState != newState)   //�ߺ��Ǵ� ���¸� �������� �ʰ� �ϱ����� �߰�
        {
            currentState = newState;
        }
    }

    protected void ChangeAnimationState(int state)    //�ִϸ��̼� ���� ��ȯ
    {
        animator.SetInteger("AnimationState", state);
    }


    ///// �浹 ó�� //////

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerWeapon"))
        {
            Player playerCompent = other.GetComponentInParent<Player>();
            if (playerCompent != null)
            {
                ChangeState(eMONSTERSTATE.HIT);
            }
            else
                Debug.LogError("Player component not found!");
        }
    }


    ///// ��Ÿ //////

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SearchRange);
    }
}
