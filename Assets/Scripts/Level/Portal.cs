using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public static bool nowLevel2 = false;
    public Director director;

    [SerializeField] private RollTheCredits lCurtainDrop = null;
    [SerializeField] private RollTheCredits rCurtainDrop = null;
    [SerializeField] private RollTheCredits tCurtainDrop = null;

    // Start is called before the first frame update
    void Awake()
    {
        director = FindObjectOfType<Director>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter boi = other.GetComponent<PlayerCharacter>();
        if (boi!=null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if(sceneName == "Level_T")
            {
                nowLevel2 = true;
                //SceneManager.LoadSceneAsync("Level_2", LoadSceneMode.Single);
                StartCoroutine(CurtainCall("Level_X"));
            }
            else if (sceneName == "Level_X")
            {
                //SceneManager.LoadSceneAsync("WinScreen", LoadSceneMode.Single);
                StartCoroutine(CurtainCall("WinScreen"));
            }
            else
            {
                Debug.Log("ERROR: COULD NOT FIND SCENE NAME");
                //SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
                StartCoroutine(CurtainCall("MainMenu"));
            }
        }
    }

    public void CloseCurtains()
    {
        Debug.Log("! Inside of Portal's Close Curtains!");
        StartCoroutine(CurtainCall("LoseScreen"));
    }

    IEnumerator CurtainCall(string sceneName)
    {
        //bring the curtains down
        if (lCurtainDrop != null)
            lCurtainDrop.reverseMove();
        if (rCurtainDrop != null)
            rCurtainDrop.reverseMove();
        if (lCurtainDrop != null)
            tCurtainDrop.reverseMove();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
