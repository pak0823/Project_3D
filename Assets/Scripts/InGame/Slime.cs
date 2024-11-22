using UnityEngine;

public class Slime : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100f; // ü�� �ʱ�ȭ
    public float AttackPower { get; private set; } = 10f; // ���ݷ� �ʱ�ȭ

    public void Move(Vector3 direction)
    {
        // Slime�� �̵� ����
        Debug.Log("Slime is moving in direction: " + direction);
    }

    public void Attack()
    {
        // Slime�� ���� ����
        Debug.Log("Slime attacks with power: " + AttackPower);
    }
}
