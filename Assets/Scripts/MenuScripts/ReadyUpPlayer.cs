using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Video;

public class ReadyUpPlayer : MonoBehaviour
{
    //Player variables
    public PlayerInputManager playerManager;
    public Director director;
    public VideoPlayer vidPlayer;
    private PlayerInput playerInput;
    public static int playersIn = 0; // players that are in the game
    public static int playersReady = 0; // players that have readyd up
    public static bool lockedIn = false; // used for locking in the current players
    float countdownTime = 3.0f; // timer used once all players are ready
    float vidTime = 0.0f; // timer used for the video
    public static bool vidIsPlaying = false;
    bool sceneLoad = false;
    [SerializeField] private float videoLength =6.2f;
    //Graphics Variables
    public Image threeText;
    public Image twoText;
    public Image oneText;
    public Image fightText;
    //GameObject Variables
    public GameObject controlParent;
    //Sound Variables
    public AudioSource soundEffectSource;
    public AudioClip curtain;
    public AudioClip join;
    public AudioClip ready;
    public AudioClip cycleLeftAndUp;
    public AudioClip cycleRightAndDown;
    public AudioClip click;
    public AudioClip countDown;
    public int playCount = 0;
    public static UnityEvent newInput = new UnityEvent();
    public static UnityEvent<Color, int> updateUIHealth = new UnityEvent<Color, int>();
    public static int[] colors = {-1,-1,-1,-1}; //index = playerNumber - 1; each number represents what color they are
    public static int[] weapons = { 0, 0, 0, 0 }; //each element represents what player has what weapon
    [SerializeField]
    public GameObject[] weaponList;
    [SerializeField]
    public Color[] colorList;
    //Curtains
    [SerializeField] private RollTheCredits lCurtainDrop;
    [SerializeField] private RollTheCredits rCurtainDrop;
    private bool curtains = false;
    //FadeText
    [SerializeField] private FadeInOut[] fadeText = new FadeInOut[5];
    [SerializeField] private GameObject[] livesText = new GameObject[2];
    //change music
    [SerializeField] private ChangeMusic musicChanger;

    private void Start()
    {
        for (int i = 0; i < livesText.Length; i++)
            livesText[i].SetActive(false);
        director = GameObject.Find("Director").GetComponent<Director>();
        controlParent.SetActive(true);
        playerManager.EnableJoining();
        SceneTransition.numPlayersChanged.AddListener(OnPlayerJoined);
        threeText.gameObject.SetActive(false);
        twoText.gameObject.SetActive(false);
        oneText.gameObject.SetActive(false);
        fightText.gameObject.SetActive(false);
        lockedIn = false;
        soundEffectSource = GameObject.FindObjectOfType<AudioSource>();
        vidPlayer = GameObject.Find("Main Camera").GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (playersIn != 0 && playersIn == playersReady)
        {
            countdownTime -= Time.deltaTime;
        }
        else if (playersIn != playersReady && lockedIn == false)
        {
            countdownTime = 3.0f;
            threeText.gameObject.SetActive(false);
            twoText.gameObject.SetActive(false);
            oneText.gameObject.SetActive(false);
            fightText.gameObject.SetActive(false);
            playCount = 0;
        }

        if (countdownTime <= -1.0f && sceneLoad == false)
        {
            sceneLoad = true;
            playersIn = 0;
            playersReady = 0;
            countdownTime = 3.0f;

            vidPlayer.Play();
            vidIsPlaying = true;
            musicChanger.PlayVideoMusic();
        }
        else if (countdownTime <= 0.0f)
        {
            if (playCount == 3)
            {
                if (soundEffectSource)
                {
                    soundEffectSource.PlayOneShot(countDown);
                }
                playCount++;
            }
            oneText.gameObject.SetActive(false); //temp
            fightText.gameObject.SetActive(true);
            lockedIn = true;
            playerManager.DisableJoining();
            if (curtains == false)
            {
                curtains = true;
                StartCoroutine(CurtainMove());
            }
        }
        else if (countdownTime <= 1.0f)
        {
            twoText.gameObject.SetActive(false); //temp
            oneText.gameObject.SetActive(true);
            if (playCount == 2)
            {
                if (soundEffectSource)
                {
                    soundEffectSource.PlayOneShot(countDown);
                }
                playCount++;
            }
        }
        else if (countdownTime <= 2.0f)
        {
            threeText.gameObject.SetActive(false); //temp
            twoText.gameObject.SetActive(true);
            if (playCount == 1)
            {
                if (soundEffectSource)
                {
                    soundEffectSource.PlayOneShot(countDown);
                }
                playCount++;
            }
        }
        else if (playersIn != 0 && playersIn == playersReady)
        {
            threeText.gameObject.SetActive(true);
            if (playCount == 0)
            {
                if (soundEffectSource)
                {
                    soundEffectSource.PlayOneShot(countDown);
                }
                playCount++;
            }
        }

        if (vidIsPlaying)
        {
            vidTime += Time.deltaTime;
            if(vidTime >= videoLength)
            {
                PrepLevel();
            }
        }
    }

    public void PrepLevel()
    {
        vidIsPlaying = false;
        vidTime = 0.0f;
        Portal.nowLevel2 = false;

        List<PlayerCharacter> currentPlayers = FindObjectOfType<SceneTransition>().players;
        Color[] allColors = FindObjectOfType<SceneTransition>().colors;
        Color playerColor = new Vector4(0f, 0f, 0f, 1f);
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayers[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            //assign colors
            Debug.Log(colors[i]);
            playerColor = colorList[Mathf.Clamp(colors[i], 0, 3)];
            currentPlayers[i].SetColor(playerColor);
            updateUIHealth.Invoke(playerColor, i + 1);
            //assign weapons
            GameObject choice = weaponList[weapons[i]];
            Debug.Log("Weapon choice: " + choice.name);
            WeaponSwap switcher = currentPlayers[i].GetComponent<WeaponSwap>();
            switcher.SetPickup(choice);
            switcher.SetInMenu(true);
            //switcher.SwapWeapons();
        }
        director.SetPlayerWeaponChoice(weapons);
        //SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Single);
        StartCoroutine(CurtainCall("Level_1"));
    }

    public void backButton()
    {
        if(lockedIn == false)
        {
            if (soundEffectSource)
            {
                soundEffectSource.PlayOneShot(click);
            }

            Destroy(playerManager.gameObject);
            playersIn = 0;
            playersReady = 0;
            countdownTime = 3.0f;
            for(int i = 0; i < 4; i++)
            {
                colors[i] = -1;
                weapons[i] = 0;
            }
            //SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            StartCoroutine(CurtainCall("MainMenu"));
        }
    }

    void OnPlayerJoined()
    {
        playersIn++;
        //Show option for lives
        if (!livesText[0].activeInHierarchy)
        {
            for (int i = 0; i < livesText.Length; i++)
                livesText[i].SetActive(true);
        }

        Debug.Log("Players In: " + playersIn);
        //playerInput = playerManager.GetComponent<PlayerInput>();
        //playerInput.SwitchCurrentActionMap("UI");
        //playerInput.currentActionMap = playerInput.actions.FindActionMap("UI");
        //Debug.Log("Current Action Map: " + playerInput.currentActionMap.name);
        newInput.Invoke();
        if (soundEffectSource)
        {
            soundEffectSource.PlayOneShot(join);
        }
    }

    void OnPlayerLeft()
    {
        playersIn--;
        Debug.Log("Players In: " + playersIn);
        newInput.Invoke();
    }

    public void showControls()
    {
        if (soundEffectSource)
        {
            soundEffectSource.PlayOneShot(click);
            soundEffectSource.PlayOneShot(curtain);
        }
        if (!controlParent.activeInHierarchy)
            controlParent.SetActive(true);
        else
            controlParent.SetActive(false);
    }

    IEnumerator CurtainCall(string sceneName)
    {
        for (int i = 0; i < fadeText.Length; i++)
            fadeText[i].gameObject.SetActive(false);
        for (int i = 0; i < livesText.Length; i++)
            livesText[i].gameObject.SetActive(false);

        //bring the curtains down
        lCurtainDrop.reverseMove();
        rCurtainDrop.reverseMove();
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    IEnumerator CurtainMove()
    {
        for(int i = 0; i < fadeText.Length; i++)
            fadeText[i].gameObject.SetActive(false);
        for (int i = 0; i < livesText.Length; i++)
            livesText[i].gameObject.SetActive(false);
        //bring the curtains down
        lCurtainDrop.reverseMove();
        rCurtainDrop.reverseMove();
        yield return new WaitForSeconds(.8f);
    }

}


