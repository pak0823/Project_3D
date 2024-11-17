using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    //������ ������� ���� ���

public partial class SceneMgr : MonoBehaviour
{
    string FilePath = "Login.txt";

    public bool IsFile(string _File)//��ο� ���� ���� ���� Ȯ��
    {
        if (File.Exists(Application.dataPath + _File))
            return true;
        else
            return false;
    }

    public void SaveFile()
    {
        if (!File.Exists(Application.dataPath + FilePath))
        {
            StreamWriter sw = File.CreateText(FilePath);

            sw.WriteLine("test");

            sw.Close();
        }
        else
        {
            StreamReader sr = File.OpenText(Application.dataPath + FilePath);

            AccountName = sr.ReadLine();

            sr.Close();
        }
    }

    public void SaveFile(string _File)
    {
        StreamWriter sw = File.CreateText(_File);

        sw.WriteLine(AccountName);

        sw.Close();
    }

    public void LoadFile(string _File)
    {
        StreamReader sr = File.OpenText(Application.dataPath + _File);

        AccountName = sr.ReadLine();

        sr.Close();
    }
}
