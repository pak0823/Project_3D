using System.Collections;
using UnityEngine;

public class Slime : BaseMonster
{
    private void Start()
    {
        base.Start(); // BaseMonster의 Start 메서드 호출
    }

    public override void Attack()
    {
        base.Attack(); // BaseMonster의 Attack 메서드 호출
        Debug.Log("Slime uses slime splash attack with power: " + AttackPower);
    }

    public override void Move(Vector3 direction)
    {
        base.Move(direction); // BaseMonster의 Move 메서드 호출

    }

    protected override IEnumerator HitRoutine()
    {
        Debug.Log("Slime got hit!");
        Health -= 10f;

        if (Health <= 0)
        {
            ChangeState(eMonsterState.DIE);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            ChangeState(eMonsterState.IDLE);
        }
    }
}
