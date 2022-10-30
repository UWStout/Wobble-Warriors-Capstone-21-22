using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    public bool isPaused = false;
    private int playerWhoPaused;

    //controller support stuff
    [SerializeField] public GameObject firstButtonSelected;

    //public TextMeshProUGUI LifeCounter;
    public GameObject pauseScreen;
    [SerializeField] Image LivesCountImage;
    Director Director;

    private void Awake()
    {
        Director = GameObject.Find("Director").GetComponent<Director>();
        Director.setLivesCountImage(LivesCountImage);
    }
    private void Update()
    {
        if (isPaused == true)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void PauseGame(int playNum)
    {
        isPaused = !isPaused;
        playerWhoPaused = playNum;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }

    public void Resume() //used by pause canvas to resume game
    {
        
        isPaused = !isPaused;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        for (int c = 0; c < 4; c++)
        {
            ReadyUpPlayer.colors[c] = -1;
            ReadyUpPlayer.weapons[c] = 0;
        }
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        Destroy(GameObject.Find("PlayerManager"));
    }
}
