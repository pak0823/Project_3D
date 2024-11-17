using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SceneMgr : MonoBehaviour
{
    public void SetPlayerPrefsIntKey(string _Key, int _Value)
    {
        PlayerPrefs.SetInt(_Key, _Value);
        PlayerPrefs.Save();
    }

    public int GetPlayerPrefsIntKey(string _Key)
    {
        return PlayerPrefs.GetInt(_Key);
    }

    public void SetPlayerPrefsStringKey(string _Key, string _Value)
    {
        PlayerPrefs.SetString(_Key, _Value);
        PlayerPrefs.Save();
    }

    public string GetPlayerPrefsStringKey(string _Key)
    {
        return PlayerPrefs.GetString(_Key);
    }
}
