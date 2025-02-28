using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {

    }

    private void NewGame()
    {
        lives = 3;

        LoadLevel(1, 1);
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel()
    {
        lives--;

        FindObjectOfType<UIControl>().AddDead();

        if (lives > -10000)
            LoadLevel(world, stage);
        else
            GameOver();
    }

    private IEnumerator ResetLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        LoadLevel(world, stage);
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
