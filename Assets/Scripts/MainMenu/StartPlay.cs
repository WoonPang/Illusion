using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlay : MonoBehaviour
{
    public void PlayStart()
    {
        PlayerPrefs.SetInt("DeadCount", 3);
        PlayerPrefs.Save();

        PlayerPrefs.DeleteKey("SavedStage");
        PlayerPrefs.Save();

        SceneManager.LoadScene("1-1");
        Debug.Log("게임을 시작합니다.");
    }
}