using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public Zombi ZombiPrefab;   //Zombi 프리팹
    public Transform Player;    //Player의 Transform

    private void Awake()
    {
        Shared.BattleMgr = this;
    }

    private void Start()
    {
        Zombi newZombi = Instantiate(ZombiPrefab, transform.position, Quaternion.identity);
        AIMonster AIMonster = new AIMonster();
        AIMonster.Init(newZombi); // Zombi 인스턴스를 AIMonster에 전달
        newZombi.Init(AIMonster); // Zombi의 AI 속성을 초기화
        AIMonster.Player = Player;
    }


}
