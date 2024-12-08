using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private IEnumerator AttackRoutine()
    {
        if (IsAttack())
        {
            Debug.Log("Attack");
            ChangeAnimationState(2);
            weaponCollider.enabled = true;
            yield return WaitForSeconds(1f); // 애니메이션 대기 시간
            weaponCollider.enabled = false;
            yield return null;
        }
        else
        {
            ChangeState(ePLAYERSTATE.IDLE); // 대기 상태로 전환
        }
    }

    private IEnumerator DefendRoutine()
    {
        // 방어 중일 경우 중복 실행 방지
        if (IsDefend())
        {
            ChangeAnimationState(4); // 방어 애니메이션 실행
            yield return null; // 다음 프레임까지 대기

            // 방어 상태 유지
            while (IsDefend())
            {
                // 방어 중에 필요한 로직을 여기에 추가 (예: 방어 중일 때 공격을 차단하는 로직 등)
                yield return null; // 다음 프레임까지 대기
            }

            // 방어가 끝났을 때
            ChangeState(ePLAYERSTATE.IDLE); // 방어 후 대기 상태로 전환
        }
        else
        {
            // 방어 버튼이 눌리지 않은 경우
            ChangeState(ePLAYERSTATE.IDLE); // 대기 상태로 전환
        }

    }
    private bool IsAttack()//플레이어 공격 체크
    {
        return Input.GetMouseButton(0);
    }
    private bool IsDefend()//플레이어 방어 체크
    {
        return Input.GetMouseButton(1);
    }
}
