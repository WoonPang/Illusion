using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMainMenu : MonoBehaviour
{
    public void GoBack()
    {
        PlayerPrefs.DeleteKey("DeadCount");
        PlayerPrefs.DeleteKey("GameStarted");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Main_Menu");
        Debug.Log("����ȭ������ ���ư��ϴ�.");
    }
}
