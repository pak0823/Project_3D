using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneMgr : MonoBehaviour
{

    private void Awake()
    {
        Shared.SceneMgr = this;
        //Shared ��ũ��Ʈ�� ����

        DontDestroyOnLoad(this);
        //���� ���� �������� ����
    }
}
