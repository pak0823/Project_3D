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
        CreateMonster(eMONSTERTYPE.Slime, new Vector3(0, 0, 0), 100f, 2f, 6f);
        CreateMonster(eMONSTERTYPE.Turtle, new Vector3(3, 0, 0), 80f, 1f, 6f);
    }
    private void CreateMonster(eMONSTERTYPE monsterType, Vector3 Position, float Health, float MoveSpeed, float SearchRange)
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
