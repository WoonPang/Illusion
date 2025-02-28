using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public GameObject HowToPlay;

    private void Start()
    {
        HowToPlay.SetActive(false);
    }

    public void OnPanel()
    {
        HowToPlay.SetActive(true);
        Debug.Log("게임방법 창을 띄웁니다.");
    }

    public void OffPanel()
    {
        HowToPlay.SetActive(false);
        Debug.Log("게임방법 창을 닫습니다.");
    }

    public void GameExit()
    {
        Application.Quit();
        Debug.Log("게임을 종료합니다.");
    }
}
