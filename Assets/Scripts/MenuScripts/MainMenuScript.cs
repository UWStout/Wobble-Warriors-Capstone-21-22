using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Olivia LaValley
// This script has the functions that hook up to the Main Menu buttons.
public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainFirstButton;

    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioClip click;

    [SerializeField] private RollTheCredits lCurtainDrop;
    [SerializeField] private RollTheCredits rCurtainDrop;
    private void Start()
    {
        soundEffectSource = GameObject.FindWithTag("SoundEffectSource").GetComponent<AudioSource>();
    }

    public void Play()
    {
        soundEffectSource.PlayOneShot(click);
        //SceneManager.LoadSceneAsync("ReadyUpMenu", LoadSceneMode.Single);
        StartCoroutine(CurtainCall("ReadyUpMenu"));
    }

    public void Menu()
    {
        if (soundEffectSource)
        {
            soundEffectSource.PlayOneShot(click);
        }
        for (int c = 0; c < 4; c++)
        {
            ReadyUpPlayer.colors[c] = -1;
            ReadyUpPlayer.weapons[c] = 0;
        }
        //SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        StartCoroutine(CurtainCall("MainMenu"));
    }

    public void Options()
    {
        soundEffectSource.PlayOneShot(click);
        //SceneManager.LoadSceneAsync("OptionsMenu", LoadSceneMode.Single);
        StartCoroutine(CurtainCall("OptionsMenu"));
    }

    public void Credits()
    {
        soundEffectSource.PlayOneShot(click);
        //SceneManager.LoadSceneAsync("CreditsScene", LoadSceneMode.Single);
        StartCoroutine(CurtainCall("CreditsScene"));
    }
    
    public void Quit()
    {
        soundEffectSource.PlayOneShot(click);
        Application.Quit();
    }

    IEnumerator CurtainCall(string sceneName)
    {
        //bring the curtains down
        lCurtainDrop.reverseMove();
        rCurtainDrop.reverseMove();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
