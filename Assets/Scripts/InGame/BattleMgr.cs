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
        BaseMonster monster = new BaseMonster();
        monster.Start(); // Start �޼��带 ȣ���Ͽ� �ʱ�ȭ
    }
}
