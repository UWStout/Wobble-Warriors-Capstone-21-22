using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    public GameObject firstSource;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.tag == "MusicSource")
        {
            firstSource = GameObject.FindWithTag("MusicSource");
            DestroyDuplicates();
        }
        else if (gameObject.tag == "SoundEffectSource")
        {
            firstSource = GameObject.FindWithTag("SoundEffectSource");
            DestroyDuplicates();
        }

    }

    public void DestroyDuplicates()
    {
        if (firstSource != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);            
        }
    } 
}
