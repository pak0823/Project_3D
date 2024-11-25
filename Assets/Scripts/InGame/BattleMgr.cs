using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public Transform Player;    //Player의 Transform

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
        // Resources 폴더에서 프리팹 로드
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Character/Monster/{Name}"); // name은 프리팹의 이름 (예: "Slime")

        if (prefab != null)
        {
            // 프리팹을 인스턴스화
            GameObject monsterObject = Instantiate(prefab, Position, Quaternion.identity);

            // BaseMonster 타입의 컴포넌트를 가져오기
            T monster = monsterObject.GetComponent<T>();

            if (monster != null)
            {
                monster.Health = Health; // 초기 체력 설정
                monster.MoveSpeed = MoveSpeed;  //초기 이동속도 설정
                monster.SearchRange = SearchRange;  //초기 탐색범위 설정
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
