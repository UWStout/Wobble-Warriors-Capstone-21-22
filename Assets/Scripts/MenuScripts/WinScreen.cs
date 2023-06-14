using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private float delayTime = 3.0f;
    private bool canTrigger = false;

    [SerializeField] private RollTheCredits lCurtainDrop;
    [SerializeField] private RollTheCredits rCurtainDrop;

    // Start is called before the first frame update
    void Start()
    {
        //storing the audio source component in the scene
        soundEffectSource = GameObject.FindWithTag("SoundEffectSource").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(delayTime >= 0.0f)
        {
            delayTime -= Time.deltaTime;
        }
        else
        {
            canTrigger = true;
        }
    }
    //Function called when retry button is pressed
    public void Retry()
    {
        if (canTrigger)
        {
            //Make button noise
            if (soundEffectSource)
            {
                soundEffectSource.PlayOneShot(click);
            }
            //Check whether the players lost in level 1 or level 2 and load whichever scene it was
            if (Portal.nowLevel2 == false)
            {
                StartCoroutine(CurtainCall("Level_T"));
                //SceneManager.LoadScene("Level_1");
            }
            else
            {
                StartCoroutine(CurtainCall("Level_X"));
                //SceneManager.LoadScene("Level_2");
            }
        }
    }
    //Function Called when quit button is pressed
    public void QuitGame()
    {
        if (canTrigger)
        {
            //Make button noise
            if (soundEffectSource)
            {
                soundEffectSource.PlayOneShot(click);
            }
            //Exit program
            Application.Quit();
        }
    }
    //Function called when main menu button is pressed
    public void MainMenu()
    {
        if (canTrigger)
        {
            //Make button noise
            if (soundEffectSource)
            {
                soundEffectSource.PlayOneShot(click);
            }
            //Destroy the current players
            Destroy(GameObject.Find("PlayerManager"));
            //reset player weapon and color choices
            for (int c = 0; c < 4; c++)
            {
                ReadyUpPlayer.colors[c] = -1;
                ReadyUpPlayer.weapons[c] = 0;
            }
            //Load the main menu
            //SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            StartCoroutine(CurtainCall("MainMenu"));
        }
    }

    IEnumerator CurtainCall(string sceneName)
    {
        //bring the curtains down
        lCurtainDrop.reverseMove();
        rCurtainDrop.reverseMove();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
