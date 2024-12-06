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
        CreateMonster(eMONSTERTYPE.Slime, new Vector3(0, 0, 0), 120f, 10f, 2f, 4f);  //���� enum, ��ġ��, HP, ���ݷ�, �̵��ӵ�, Ž�� ����
        CreateMonster(eMONSTERTYPE.Turtle, new Vector3(3, 0, 0), 150f, 15f, 1.5f, 6f);
        CreateMonster(eMONSTERTYPE.Mushroom, new Vector3(3, 0, 3), 130f, 10f, 3f, 4f);
        CreateMonster(eMONSTERTYPE.Cactus, new Vector3(7, 0, 3), 140f, 15f, 2.5f, 5f);
        //CreateMonster(eMONSTERTYPE.Golem, new Vector3(-5, 0, 5), 300f, 30f, 1f, 10f);
    }
    private void CreateMonster(eMONSTERTYPE monsterType, Vector3 Position, float Health, float AttackPower, float MoveSpeed, float SearchRange)
    {

        string prefabPath = $"Prefabs/Character/Monster/{monsterType}"; // enum�� ����Ͽ� ���ڿ� ����
        GameObject prefab = Resources.Load<GameObject>(prefabPath); // Resources �������� ������ �ε�

        if (prefab != null)
        {
            // �������� �ν��Ͻ�ȭ
            GameObject monsterObject = Instantiate(prefab, Position, Quaternion.identity);

            // BaseMonster Ÿ���� ������Ʈ�� ��������
            BaseMonster monster = monsterObject.GetComponent<BaseMonster>();

            if (monster != null)
            {
                monster.Health = Health; // �ʱ� ü�� ����
                monster.AttackPower = AttackPower;  //�ʱ� ���ݷ� ����
                monster.MoveSpeed = MoveSpeed;  //�ʱ� �̵��ӵ� ����
                monster.SearchRange = SearchRange;  //�ʱ� Ž������ ����
            }
            else
            {
                Debug.LogError($"The prefab does not have a component of type {typeof(BaseMonster)}.");
            }
        }
        else
        {
            Debug.LogError($"Prefab '{prefabPath}' not found in Resources folder.");
        }
    }
}
