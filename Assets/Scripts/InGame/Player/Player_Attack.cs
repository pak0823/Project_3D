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
            yield return WaitForSeconds(1f); // �ִϸ��̼� ��� �ð�
            weaponCollider.enabled = false;
            yield return null;
        }
        else
        {
            ChangeState(ePLAYERSTATE.IDLE); // ��� ���·� ��ȯ
        }
    }

    private IEnumerator DefendRoutine()
    {
        // ��� ���� ��� �ߺ� ���� ����
        if (IsDefend())
        {
            ChangeAnimationState(4); // ��� �ִϸ��̼� ����
            yield return null; // ���� �����ӱ��� ���

            // ��� ���� ����
            while (IsDefend())
            {
                // ��� �߿� �ʿ��� ������ ���⿡ �߰� (��: ��� ���� �� ������ �����ϴ� ���� ��)
                yield return null; // ���� �����ӱ��� ���
            }

            // �� ������ ��
            ChangeState(ePLAYERSTATE.IDLE); // ��� �� ��� ���·� ��ȯ
        }
        else
        {
            // ��� ��ư�� ������ ���� ���
            ChangeState(ePLAYERSTATE.IDLE); // ��� ���·� ��ȯ
        }

    }
    private bool IsAttack()//�÷��̾� ���� üũ
    {
        return Input.GetMouseButton(0);
    }
    private bool IsDefend()//�÷��̾� ��� üũ
    {
        return Input.GetMouseButton(1);
    }
}
