using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public Transform Player;    //Player�� Transform

    private void Awake()
    {
        Shared.BattleMgr = this;
    }

    private void Start()
    {
        CreateMonster<Slime>("Slime", new Vector3(0, 0, 0), 100f, 3f, 5f);
        //CreateMonster<Goblin>("Goblin", new Vector3(2, 0, 0), 80f);
    }
    private void CreateMonster<T>(string Name, Vector3 Position, float Health, float MoveSpeed, float SearchRange) where T : BaseMonster
    {
        // Resources �������� ������ �ε�
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Character/Monster/{Name}"); // name�� �������� �̸� (��: "Slime")

        if (prefab != null)
        {
            // �������� �ν��Ͻ�ȭ
            GameObject monsterObject = Instantiate(prefab, Position, Quaternion.identity);

            // BaseMonster Ÿ���� ������Ʈ�� ��������
            T monster = monsterObject.GetComponent<T>();

            if (monster != null)
            {
                monster.Health = Health; // �ʱ� ü�� ����
                monster.MoveSpeed = MoveSpeed;  //�ʱ� �̵��ӵ� ����
                monster.SearchRange = SearchRange;  //�ʱ� Ž������ ����
            }
            else
            {
                Debug.LogError($"The prefab does not have a component of type {typeof(T)}.");
            }
        }
        else
        {
            Debug.LogError($"Prefab '{name}' not found in Resources folder.");
        }
    }
}
