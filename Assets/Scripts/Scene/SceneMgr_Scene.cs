using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneMgr : MonoBehaviour
{
    public eSCENE Scene = eSCENE.eSCENE_TITLE;

    public void ChangeScene(eSCENE _e, bool _Loading = false)
    {
        if (Scene == _e)
            return;

        switch(_e)
        {
            case eSCENE.eSCENE_TITLE:
                SceneManager.LoadScene("Title");
                break;
            case eSCENE.eSCENE_LOGIN:
                SceneManager.LoadScene("Login");
                break;
            case eSCENE.eSCENE_LOBBY:
                SceneManager.LoadScene("Lobby");
                break;
            case eSCENE.eSCENE_LOADING:
                SceneManager.LoadScene("Loading");
                break;
        }
    }
}
