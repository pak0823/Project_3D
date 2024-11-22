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
        BaseMonster monster = new BaseMonster();
        monster.Start(); // Start 메서드를 호출하여 초기화
    }
}
