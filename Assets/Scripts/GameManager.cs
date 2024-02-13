using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  
    public bool gameActive = true;
    [SerializeField] private GameObject winPanel, losePanel, pausePanel;
    public int levelNumber;
    public static GameManager instance;
    [SerializeField] private int enemies;
    private void Awake()
    {
        instance = this;
        foreach (AudioSource source in FindObjectsOfType(typeof(AudioSource)))
        {
            source.volume = PlayerPrefs.GetFloat("volume", 0.7f);
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Win()
    {
        if(gameActive)
        {
            Cursor.lockState = CursorLockMode.None;
            if (PlayerPrefs.GetInt("currentLevel", 0) < levelNumber)
                PlayerPrefs.SetInt("currentLevel", levelNumber);
            gameActive = false;
            winPanel.SetActive(true);
        }
    }
    public void Lose()
    {
        if (gameActive)
        {
            Cursor.lockState = CursorLockMode.None;
            gameActive = false;
            losePanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void EnemyDestroyed()
    {
        --enemies;
        if(enemies == 0)
            Win();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
        }
    }
    public void ContinueGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
