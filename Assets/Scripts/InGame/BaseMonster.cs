using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100f; // 체력 초기화
    public float AttackPower { get; private set; } = 10f; // 공격력 초기화

    private AIBase aiBase;

    public void Start()
    {
        aiBase = new AIBase(); // AIBase 인스턴스 생성
        aiBase.Init(this); // ICharacter로 초기화
        StartCoroutine(StateMachine()); // 상태 기계 시작
    }

    private IEnumerator StateMachine()
    {
        while (true) // 무한 루프를 통해 상태를 지속적으로 체크
        {
            aiBase.UpdateState(); // AI 상태 업데이트
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void Move(Vector3 direction)
    {
        // BaseMonster의 이동 로직
        Debug.Log("BaseMonster is moving in direction: " + direction);
    }

    public void Attack()
    {
        // BaseMonster의 공격 로직
        Debug.Log("BaseMonster attacks with power: " + AttackPower);
    }
}
