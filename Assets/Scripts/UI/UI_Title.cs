using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    public void Start()
    {
        //StartCoroutine("INextScene");
    }

    IEnumerator INextScene()
    {
        yield return new WaitForSeconds(3f);

        OnBtnTitle();
    }

    public void OnBtnTitle()
    {
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOGIN);
    }

    public void OnBtnSetting(int NUM)
    {
        Transform setting = transform.GetChild(5);
        if(NUM > 0)
            setting.gameObject.SetActive(true);
        else
            setting.gameObject.SetActive(false);
    }

    public void OnBtnEndGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //에디터에서의 실행을 종료할 경우
        //Application.Quit();
        //빌드에서 종료할 경우
    }
}
