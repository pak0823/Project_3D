using System.Collections;
using UnityEngine;

public class Monster : BaseMonster
{
    public override IEnumerator Attack()
    {
        // BaseMonster�� Attack �޼��� ȣ��
        yield return base.Attack();
        Debug.Log("Attack");
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
