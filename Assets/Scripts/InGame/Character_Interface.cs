using System.Collections;
using UnityEngine;

public interface Character_Interface
{
    void Move(Vector3 direction); // �̵� �޼���
    IEnumerator AttackRoutine(); // ���� �޼���

    float Health { get; set; } // ü�� �Ӽ�
    float AttackPower { get; } // ���ݷ� �Ӽ�
    float MoveSpeed { get; }    //�̵��ӵ� �Ӽ�
    float SearchRange { get; }  //Ž������ ����
}