using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHazards : MonoBehaviour
{
    // Start is called before the first frame update


    

    private Transform CurrentRoom;

    [SerializeField] private GameObject[] HazardParts;

    [SerializeField] private List<MonoBehaviour> HazardScripts;

    [SerializeField] private List<AudioSource> HazardAudioSFX;

    private string[] HazardTags;

    
    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void DisableHazard()
    {
        foreach (MonoBehaviour Hazard in HazardScripts)
        {
            Hazard.enabled = false;
        }
        foreach(AudioSource sound in HazardAudioSFX)
        {
            sound.enabled = false;
        }
    }

    public void EnableHazard()
    {
        foreach (MonoBehaviour Hazard in HazardScripts)
        {
            Hazard.enabled = true;
            
        }
        foreach (AudioSource sound in HazardAudioSFX)
        {
            sound.enabled = true;
        }
    }

    public List<MonoBehaviour> getDisabledObjects()
    {
        return HazardScripts;
    }

    public List<AudioSource> getDisabledSound()
    {
        return HazardAudioSFX;
    }
}
