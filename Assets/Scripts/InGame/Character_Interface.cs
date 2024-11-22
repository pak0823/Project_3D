using UnityEngine;

public interface Character_Interface
{
    void Move(Vector3 direction); // 이동 메서드
    void Attack(); // 공격 메서드

    float Health { get; set; } // 체력 속성
    float AttackPower { get; } // 공격력 속성 (읽기 전용)
}