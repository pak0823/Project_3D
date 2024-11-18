using System.Collections;
using UnityEngine;

public class AIMonster : AIBase
{
    public Transform Player;
    protected float SearchRadius = 10f; //������ Search����
    protected float patrolRadius = 15f; //������ ���� ����
    protected float WaitTime = 2f;  //��� �ð�
    protected float MoveSpeed = 1f;  //���� �̵� �ӵ�
    private Vector3 patrolPoint;

    protected override void Create()
    {
        //���� ���� �� �ʱ�ȭ �۾��� ����, ���� ������ ����
        base.Create();
        SetNewPatrolPoint();
        Debug.Log("Monster_Created");
    }

    protected override void Search()
    {
        //�÷��̾ Search���� �ȿ� ������ ���� ���·� ��ȯ�ϰ� ���� �������� �̵�
        if(Vector3.Distance(Character.transform.position, Player.position) <= SearchRadius)
        {
            //�÷��̾ �˻� ���� �ȿ� ���Դ��� Ȯ��

            AIState = eAI.eAI_MOVE; //���� ���·� ����
        }

        MoveTowards(patrolPoint);

        if(Vector3.Distance(Character.transform.position, patrolPoint) <= 1f)
        {
            //���� ������ �����ߴ��� Ȯ��
            //StartCoroutine(Patrol());
        }

        Debug.Log("Monster_Scarching");
    }

    protected override void Move()
    {
        //�÷��̾ ���� �̵�, �÷��̾ Search������ ����� �ٽ� Search ���·� ��ȯ
        MoveTowards(Player.position);

        if(Vector3.Distance(Character.transform.position,Player.position) > SearchRadius)
        {
            AIState = eAI.eAI_SEARCH;   //Search ���·� ����
            SetNewPatrolPoint();
        }

        Debug.Log("Monster_Move");
    }

    protected override void Reset()
    {
        //�ʱ�ȭ �۾��� ����
        AIState = eAI.eAI_SEARCH;
        SetNewPatrolPoint();
        Debug.Log("Monster_Reset");
    }

    private void SetNewPatrolPoint()
    {
        //���ο� ���������� �������� ����
        Vector3 randomPoint = Random.insideUnitSphere * SearchRadius;
        randomPoint += Character.transform.position;
        randomPoint.y = Character.transform.position.y;
    }

    private void MoveTowards(Vector3 _target)
    {
        Debug.Log(Character);
        //�־��� ��ǥ �������� ���Ͱ� �̵�
        Vector3 direction = (_target - Character.transform.position).normalized;    //���� ���
        Character.transform.position += direction * MoveSpeed * Time.deltaTime; //�̵�
    }

    private IEnumerator Patrol()
    {
        //���� �� ��� �� ���ο� ���� ������ ����
        WaitForSeconds WFS = new WaitForSeconds(WaitTime);
        yield return WFS;   //��� �ð�
        SetNewPatrolPoint();
    }
}
