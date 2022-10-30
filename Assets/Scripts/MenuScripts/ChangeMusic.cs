using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip levelMusic;
    public Director director;
    public bool playedBoss = false;
    public bool winPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindWithTag("MusicSource");
        if (temp != null && source == null)
            source = temp.GetComponent<AudioSource>();

        temp = GameObject.Find("Director");
        if ( temp != null && director == null)
            director = temp.GetComponent<Director>();

        //doesnt play if the scene is playing the same song again
        if (source != null && source.clip != music)
        {

            source.clip = music;
            source.Play();
        }
    }

    private void Update()
    {
        if (director != null && SceneManager.GetActiveScene().name != "ReadyUpMenu")
        {
            //plays boss music if players are in the boss room
            if (director.GetCurrentRoomInfo().isBossRoom && !playedBoss && source != null)
            {
                source.clip = bossMusic;
                source.Play();
                playedBoss = true;
            }
            //plays win music when the boss room is completed
            if(playedBoss && !winPlayed && director.GetCurrentRoomInfo().getRoomCleared() && source != null)
            {
                source.clip = winMusic;
                source.Play();
                winPlayed = true;
            }
        }
    }

    public void PlayVideoMusic()
    {
        source.clip = levelMusic;
        source.Play();
    }

}
