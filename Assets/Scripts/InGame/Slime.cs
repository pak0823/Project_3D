using UnityEngine;

public class Slime : MonoBehaviour, Character_Interface
{
    public float Health { get; set; } = 100f; // 체력 초기화
    public float AttackPower { get; private set; } = 10f; // 공격력 초기화

    public void Move(Vector3 direction)
    {
        // Slime의 이동 로직
        Debug.Log("Slime is moving in direction: " + direction);
    }

    public void Attack()
    {
        // Slime의 공격 로직
        Debug.Log("Slime attacks with power: " + AttackPower);
    }
}
