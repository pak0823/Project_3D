using System.Collections;
using UnityEngine;

public interface Character_Interface
{
    void Move(Vector3 direction); // 이동 메서드
    //IEnumerator AttackRoutine(); // 공격 메서드

    void AttackRoutine(); // 공격 메서드

    float Health { get; set; } // 체력 속성
    float AttackPower { get; } // 공격력 속성
    float MoveSpeed { get; }    //이동속도 속성
    float SearchRange { get; }  //탐색범위 속정
}