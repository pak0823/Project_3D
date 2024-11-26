using System.Collections;
using UnityEngine;

public class Monster : BaseMonster
{

    public override void Attack()
    {
        base.Attack(); // BaseMonster�� Attack �޼��� ȣ��
        Debug.Log("Slime uses slime splash attack with power: " + AttackPower);
    }

    public override void Move(Vector3 direction)
    {
        base.Move(direction); // BaseMonster�� Move �޼��� ȣ��

    }

    protected override IEnumerator HitRoutine()
    {
        Debug.Log("Slime got hit!");
        Health -= 10f;

        if (Health <= 0)
        {
            ChangeState(eMONSTERSTATE.DIE);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            ChangeState(eMONSTERSTATE.IDLE);
        }
    }
}
