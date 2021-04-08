using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public bool isMute = false;

    [SerializeField] AudioSource _audioSFXSrc;
    [SerializeField] AudioSource _audioBackgroundSrc;
    [SerializeField] AudioClip[] _sounds;
    [SerializeField] AudioClip[] _backgroundSounds;

    private void Start()
    {
        PlayBackgroundMusic("MainMenu");
    }

    public void PlaySFX(string sound)
    {
        if (!isMute && _audioSFXSrc != null)
        {
            switch (sound)
            {
                case "jumpSound":
                    _audioSFXSrc.PlayOneShot(_sounds[0]);
                    break;
                case "otherSound":
                    _audioSFXSrc.PlayOneShot(_sounds[1]);
                    break;
                default:
                    Debug.LogError("There is no sound with the given name:" + sound);
                    break;
            }
        }
    }

    public void PlayBackgroundMusic(string sound)
    {
        if (!isMute && _audioBackgroundSrc != null)
        {
            switch (sound)
            {
                case "MainMenu":
                    _audioBackgroundSrc.PlayOneShot(_backgroundSounds[0]);
                    break;
                case "MainLevel":
                    _audioBackgroundSrc.PlayOneShot(_backgroundSounds[1]);
                    break;
                case "ScoringLevel":
                    _audioBackgroundSrc.PlayOneShot(_backgroundSounds[2]);
                    break;
                case "EndGame":
                    _audioBackgroundSrc.PlayOneShot(_backgroundSounds[3]);
                    break;
                default:
                    Debug.LogError("There is no sound with the given name:" + sound);
                    break;
            }
        }
    }
}
