using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public Zombi ZombiPrefab;   //Zombi ������
    public Transform Player;    //Player�� Transform

    private void Awake()
    {
        Shared.BattleMgr = this;
    }

    private void Start()
    {
        Zombi newZombi = Instantiate(ZombiPrefab, transform.position, Quaternion.identity);
        AIMonster AIMonster = new AIMonster();
        AIMonster.Init(newZombi); // Zombi �ν��Ͻ��� AIMonster�� ����
        newZombi.Init(AIMonster); // Zombi�� AI �Ӽ��� �ʱ�ȭ
        AIMonster.Player = Player;
    }


}
