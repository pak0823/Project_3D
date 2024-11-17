using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneMgr : MonoBehaviour
{

    private void Awake()
    {
        Shared.SceneMgr = this;
        //Shared 스크립트에 참조

        DontDestroyOnLoad(this);
        //현재 씬을 제거하지 않음
    }
}
