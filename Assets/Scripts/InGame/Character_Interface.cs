using UnityEngine;

public interface Character_Interface
{
    void Move(Vector3 direction); // �̵� �޼���
    void Attack(); // ���� �޼���

    float Health { get; set; } // ü�� �Ӽ�
    float AttackPower { get; } // ���ݷ� �Ӽ� (�б� ����)
    float MoveSpeed { get; }    //�̵��ӵ� �Ӽ�
    float SearchRange { get; }  //Ž������ ����
}