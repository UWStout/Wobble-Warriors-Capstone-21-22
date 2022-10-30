using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSounds : MonoBehaviour
{
    [SerializeField] AudioSource source;

    [SerializeField] AudioClip[] deathSound;
    [SerializeField] AudioClip[] damageSound;
    [SerializeField] AudioClip[] rawrSound;
    [SerializeField] AudioClip[] reviveSound;
    [SerializeField] AudioClip[] oofSound;
    [SerializeField] AudioClip[] dashSound;
    [SerializeField] AudioClip[] groundpoundSound;

    public AudioClip[] getDeathSounds()
    {
        return deathSound;
    }

    public AudioClip[] getDamageSounds()
    {
        return damageSound;
    }

    public AudioClip[] getRawrSounds()
    {
        return rawrSound;
    }

    public AudioClip[] getReviveSounds()
    {
        return reviveSound;
    }

    public AudioClip[] getOofSounds()
    {
        return oofSound;
    }

    public AudioClip[] getDashSound()
    {
        return dashSound;
    }

    public AudioClip[] GetGroundpoundSound()
    {
        return groundpoundSound;
    }

    public void playRandomSound(AudioClip[] soundArray, float probability, float volume, float pitchVariance)
    {
        float val = Random.Range(0.0f, 1.0f);

        if(val <= probability)
        {
            int index = Random.Range(0, soundArray.Length);

            source.pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);
            source.volume = volume;

            //play sounds
            source.PlayOneShot(soundArray[index]);

        }
    }
}
