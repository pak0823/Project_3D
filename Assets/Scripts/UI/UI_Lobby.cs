using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public void OnBtnLobby()
    {
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_BATTLE);
    }
}
