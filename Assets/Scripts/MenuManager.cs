using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject levelPanel, ayarlarPanel, hakkindaPanel, nasilOynanirPanel;
    [SerializeField] private Slider volumeSlider;
    private GameObject currentPanel;
    private void Awake()
    {
        foreach (AudioSource source in FindObjectsOfType(typeof(AudioSource)))
        {
            source.volume = PlayerPrefs.GetFloat("volume", 0.7f);
        }
    }
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.7f);
        currentPanel = levelPanel;
        for(int i = 0; i < PlayerPrefs.GetInt("currentLevel", 0); i++)
        {
            levels[i].transform.GetChild(1).gameObject.SetActive(true);
            if(levels.Length > i+1)
                levels[i+1].GetComponent<Button>().interactable = true;
        }
    }
    public void OynaButton()
    {
        currentPanel.SetActive(false);
        levelPanel.SetActive(true);
        currentPanel = levelPanel;
    }
    public void AyarlarButton()
    {
        currentPanel.SetActive(false);
        ayarlarPanel.SetActive(true);
        currentPanel = ayarlarPanel;
    }
    public void HakkindaButton()
    {
        currentPanel.SetActive(false);
        hakkindaPanel.SetActive(true);
        currentPanel = hakkindaPanel;
    }
    public void NasilOynanirButton()
    {
        currentPanel.SetActive(false);
        nasilOynanirPanel.SetActive(true);
        currentPanel = nasilOynanirPanel;
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void LevelPlay(int i)
    {
        SceneManager.LoadScene(i);
    }
    public void VolumeChanged()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        foreach (AudioSource source in FindObjectsOfType(typeof(AudioSource)))
        {
            source.volume = PlayerPrefs.GetFloat("volume", 0.7f);
        }
    }

    public void QualityChanged(int i)
    {
        QualitySettings.SetQualityLevel(i,true);
    }
}
