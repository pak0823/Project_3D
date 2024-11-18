using System.Collections;
using UnityEngine;

public class AIMonster : AIBase
{
    public Transform Player;
    protected float SearchRadius = 10f; //몬스터의 Search범위
    protected float patrolRadius = 15f; //몬스터의 추적 범위
    protected float WaitTime = 2f;  //대기 시간
    protected float MoveSpeed = 1f;  //몬스터 이동 속도
    private Vector3 patrolPoint;

    protected override void Create()
    {
        //몬스터 생성 시 초기화 작업을 실행, 순찰 지점을 설정
        base.Create();
        SetNewPatrolPoint();
        Debug.Log("Monster_Created");
    }

    protected override void Search()
    {
        //플레이어가 Search범위 안에 들어오면 추적 상태로 전환하고 순찰 지점으로 이동
        if(Vector3.Distance(Character.transform.position, Player.position) <= SearchRadius)
        {
            //플레이어가 검색 범위 안에 들어왔는지 확인

            AIState = eAI.eAI_MOVE; //추적 상태로 변경
        }

        MoveTowards(patrolPoint);

        if(Vector3.Distance(Character.transform.position, patrolPoint) <= 1f)
        {
            //순찰 지점에 도착했는지 확인
            //StartCoroutine(Patrol());
        }

        Debug.Log("Monster_Scarching");
    }

    protected override void Move()
    {
        //플레이어를 향해 이동, 플레이어가 Search범위를 벗어나면 다시 Search 상태로 전환
        MoveTowards(Player.position);

        if(Vector3.Distance(Character.transform.position,Player.position) > SearchRadius)
        {
            AIState = eAI.eAI_SEARCH;   //Search 상태로 변경
            SetNewPatrolPoint();
        }

        Debug.Log("Monster_Move");
    }

    protected override void Reset()
    {
        //초기화 작업을 수행
        AIState = eAI.eAI_SEARCH;
        SetNewPatrolPoint();
        Debug.Log("Monster_Reset");
    }

    private void SetNewPatrolPoint()
    {
        //새로운 순찰지점을 랜덤으로 설정
        Vector3 randomPoint = Random.insideUnitSphere * SearchRadius;
        randomPoint += Character.transform.position;
        randomPoint.y = Character.transform.position.y;
    }

    private void MoveTowards(Vector3 _target)
    {
        Debug.Log(Character);
        //주어진 목표 방향으로 몬스터가 이동
        Vector3 direction = (_target - Character.transform.position).normalized;    //방향 계산
        Character.transform.position += direction * MoveSpeed * Time.deltaTime; //이동
    }

    private IEnumerator Patrol()
    {
        //순찰 중 대기 후 새로운 순찰 지점을 설정
        WaitForSeconds WFS = new WaitForSeconds(WaitTime);
        yield return WFS;   //대기 시간
        SetNewPatrolPoint();
    }
}
