using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
    public Text TEXTINPUT;

    private void Start()
    {
        string name = Shared.SceneMgr.GetPlayerPrefsStringKey("Name");

        if("" == Shared.SceneMgr.AccountName)
        {
        }
        else
        {
            Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY, true);
            //로그인 성공 후 로비씬으로 이동
        }
    }

    public void OnBtnLogin()
    {
        if (TEXTINPUT.text.Length == 0 || TEXTINPUT.text == null)
        {
            return;
            //TEXTINPUT에 아무런 입력값이 없으면 리턴
        }

        Shared.SceneMgr.AccountName = TEXTINPUT.text;

        Shared.SceneMgr.SetPlayerPrefsStringKey("Name", TEXTINPUT.text);

        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOADING, true);

    }
}
