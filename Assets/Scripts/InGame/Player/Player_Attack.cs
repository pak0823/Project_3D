using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private IEnumerator AttackRoutine()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        ChangeAnimationState(2);
        weaponCollider.enabled = true;
        yield return WaitForSeconds(0.65f); // �ִϸ��̼� ��� �ð�
        isAttacking = false;
        weaponCollider.enabled = false;
        ChangeState(ePLAYERSTATE.IDLE);
    }
}
