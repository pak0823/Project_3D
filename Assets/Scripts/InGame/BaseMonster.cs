using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100f; // ü�� �ʱ�ȭ
    public float AttackPower { get; private set; } = 10f; // ���ݷ� �ʱ�ȭ

    private AIBase aiBase;

    public void Start()
    {
        aiBase = new AIBase(); // AIBase �ν��Ͻ� ����
        aiBase.Init(this); // ICharacter�� �ʱ�ȭ
        StartCoroutine(StateMachine()); // ���� ��� ����
    }

    private IEnumerator StateMachine()
    {
        while (true) // ���� ������ ���� ���¸� ���������� üũ
        {
            aiBase.UpdateState(); // AI ���� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }
    }

    public void Move(Vector3 direction)
    {
        // BaseMonster�� �̵� ����
        Debug.Log("BaseMonster is moving in direction: " + direction);
    }

    public void Attack()
    {
        // BaseMonster�� ���� ����
        Debug.Log("BaseMonster attacks with power: " + AttackPower);
    }
}
