using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 Dir;
    State state;
    BaseMonster target;
    WaitForSeconds wfs = new WaitForSeconds(0.1f);

    public float MoveSpeed = 8f;
    public float HP;
    public float AttackDelay;
    public float AttackPower = 5f;
    float remainAttackTime = 1f;

    Animator Animator;
    CharacterController CharacterController;

    //�߼Ҹ� ����� Ŭ��
    public AudioClip FOOTSTEP;

    public enum State
    {
        Idle,
        Move,
        Attack
    }

    private void Awake()
    {
        Shared.player = this;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();

        state = State.Idle;
        HP = 100f;
        remainAttackTime = 0f;
        //StartCoroutine(CorUpdate());
    }

    private void Update()
    {
        Move();
        Attack();
    }

    IEnumerator CorUpdate()
    {
        while (true)
        {
            switch (state)
            {
                case State.Idle:
                    {
                        Debug.Log("Idle");
                        break;
                    }
                     
                case State.Move:
                    {
                        Debug.Log("Move");
                        Move();
                        break;
                    }
                    //case State.Attack: Attack(); break;
            }
            yield return wfs;
        }
    }

    private void Move()
    {
        if (CharacterController.isGrounded)//ĳ���Ͱ� ���鿡 �ִ� ���
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            Dir = new Vector3(h, 0, v) * MoveSpeed;

            if (Dir != Vector3.zero)
            {
                //���� �������� ĳ���� ȸ��
                //Mathf.Atan2(h, v): �� ���� ���ڸ� �޾� ��ũź��Ʈ ���� ���, �־��� �� ���� ������ ������� ������ ��ȯ. ��ȯ�Ǵ� ������ ���� ����
                //Mathf.Rad2Deg: ������ ��(degree)�� ��ȯ�ϴ� ���
                //Quaternion.Euler(x, y, z): �־��� ������ ����Ͽ� Quaternion�� ����. �������� ���� x��, y��, z���� �������� �ϴ� ȸ���� ��Ÿ��.
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
                Animator.SetBool("IsMove", true);
                state = State.Move;
            }
            else
            {
                Animator.SetBool("IsMove", false);
                state = State.Idle;
            }


            if (Input.GetKeyDown(KeyCode.Space))// Spacebar �Է� �� ����
                Dir.y = 7.5f;
        }

        Dir.y += Physics.gravity.y * Time.deltaTime;
        CharacterController.Move(Dir * Time.deltaTime);
    }

    void Attack()
    {
        bool inputAttack = Input.GetMouseButtonDown(0);
        if (inputAttack && remainAttackTime <= 0f)
        {
            remainAttackTime = AttackDelay;
            Animator.Play("Attack02", -1, 0);
            Animator.SetBool("IsMove", false);
        }
        remainAttackTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.GetComponent<BaseMonster>();
        if (target != null)
            transform.LookAt(target.transform);
    }

    public void GetDamage(float _Dmg)
    {
        HP -= _Dmg;
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") == false)
            Animator.Play("GetHit", -1, 0);
    }

    void FootStep()
    {
        //�̺�Ʈ�� �߻��ϸ� �߼Ҹ� ���� ���
        //AudioSource.PlayClipAtPoint: �־��� ����� Ŭ���� Ư�� ��ġ���� ���. �� �޼���� ���ο� AudioSource�� �����Ͽ� �ش� ��ġ�� �Ҹ��� ����ϰ�, �Ҹ��� ������ �ڵ����� ������.
        //FOOTSTEP �Ҹ��� ī�޶��� ��ġ���� �����.
        //AudioSource.PlayClipAtPoint(FOOTSTEP, Camera.main.transform.position);
    }

    void AnimHit()
    {
        ////���� ��� �� Ư�� ������ ���� �����ӿ��� �߻��� �̺�Ʈ
        //if (target == null || target.Health <= 0) return;
        //    target.GetDamage;
    }
}
