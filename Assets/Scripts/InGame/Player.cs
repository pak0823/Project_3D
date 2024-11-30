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

    //발소리 오디오 클릭
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
        if (CharacterController.isGrounded)//캐릭터가 지면에 있는 경우
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            Dir = new Vector3(h, 0, v) * MoveSpeed;

            if (Dir != Vector3.zero)
            {
                //진행 방향으로 캐릭터 회전
                //Mathf.Atan2(h, v): 두 개의 인자를 받아 아크탄젠트 값을 계산, 주어진 두 값의 비율을 기반으로 각도를 반환. 반환되는 각도는 라디안 단위
                //Mathf.Rad2Deg: 라디안을 도(degree)로 변환하는 상수
                //Quaternion.Euler(x, y, z): 주어진 각도를 사용하여 Quaternion을 생성. 각도들은 각각 x축, y축, z축을 기준으로 하는 회전을 나타냄.
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
                Animator.SetBool("IsMove", true);
                state = State.Move;
            }
            else
            {
                Animator.SetBool("IsMove", false);
                state = State.Idle;
            }


            if (Input.GetKeyDown(KeyCode.Space))// Spacebar 입력 시 점프
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
        //이벤트가 발생하면 발소리 사운드 재생
        //AudioSource.PlayClipAtPoint: 주어진 오디오 클립을 특정 위치에서 재생. 이 메서드는 새로운 AudioSource를 생성하여 해당 위치에 소리를 재생하고, 소리가 끝나면 자동으로 삭제됨.
        //FOOTSTEP 소리를 카메라의 위치에서 재생함.
        //AudioSource.PlayClipAtPoint(FOOTSTEP, Camera.main.transform.position);
    }

    void AnimHit()
    {
        ////공격 모션 중 특정 설정에 따라 프레임에서 발생한 이벤트
        //if (target == null || target.Health <= 0) return;
        //    target.GetDamage;
    }
}
