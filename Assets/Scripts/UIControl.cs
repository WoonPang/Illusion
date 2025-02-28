using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    public Text stageText;
    public Text deadText;

    private int deadCount;

    void Start()
    {
        pauseMenu.SetActive(false);

        stageText.text = SceneManager.GetActiveScene().name;

        if (!PlayerPrefs.HasKey("GameStarted"))
        {
            PlayerPrefs.SetInt("DeadCount", 3);
            PlayerPrefs.SetInt("GameStarted", 1);
            PlayerPrefs.Save();
        }

        deadCount = PlayerPrefs.GetInt("DeadCount", 3);
        UpdateDeadUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void GameExit()
    {
        Application.Quit();
        Debug.Log("게임을 종료합니다.");
    }

    public void AddDead()
    {
        deadCount--;
        PlayerPrefs.SetInt("DeadCount", deadCount);
        PlayerPrefs.Save();
        UpdateDeadUI();
    }

    void UpdateDeadUI()
    {
        deadText.text = "X " + deadCount;
    }
}