using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        WaitForSeconds WFS = new WaitForSeconds(3f);

        gameObject.SetActive(true);

        yield return WFS;

        gameObject.SetActive(false);
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}
