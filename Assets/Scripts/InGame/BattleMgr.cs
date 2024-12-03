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
        CreateMonster(eMONSTERTYPE.Slime, new Vector3(0, 0, 0), 100f, 10f, 20f, 4f);  //몬스터 enum, 위치값, HP, 공격력, 이동속도, 탐색 범위
        CreateMonster(eMONSTERTYPE.Turtle, new Vector3(3, 0, 0), 120f, 15f, 15f, 5f);
    }
    private void CreateMonster(eMONSTERTYPE monsterType, Vector3 Position, float Health, float AttackPower, float MoveSpeed, float SearchRange)
    {

        string prefabPath = $"Prefabs/Character/Monster/{monsterType}"; // enum을 사용하여 문자열 생성
        GameObject prefab = Resources.Load<GameObject>(prefabPath); // Resources 폴더에서 프리팹 로드

        if (prefab != null)
        {
            // 프리팹을 인스턴스화
            GameObject monsterObject = Instantiate(prefab, Position, Quaternion.identity);

            // BaseMonster 타입의 컴포넌트를 가져오기
            BaseMonster monster = monsterObject.GetComponent<BaseMonster>();

            if (monster != null)
            {
                monster.Health = Health; // 초기 체력 설정
                monster.AttackPower = AttackPower;  //초기 공격력 설정
                monster.MoveSpeed = MoveSpeed;  //초기 이동속도 설정
                monster.SearchRange = SearchRange;  //초기 탐색범위 설정
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
