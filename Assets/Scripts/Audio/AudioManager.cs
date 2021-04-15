using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public bool isMute = false;

    [SerializeField] AudioSource _audioSFXSrc;
    [SerializeField] AudioSource _audioBackgroundSrc;
    [SerializeField] AudioSource _audioWalkingSrc;
    [SerializeField] AudioClip[] _sounds;
    [SerializeField] AudioClip[] _backgroundSounds;
    [SerializeField] AudioClip[] _walkingSounds;

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
                case "attackSound":
                    _audioSFXSrc.PlayOneShot(_sounds[1]);
                    break;
                case "coinPickUpSound":
                    _audioSFXSrc.PlayOneShot(_sounds[2]);
                    break;
                case "hatPickUpSound":
                    _audioSFXSrc.PlayOneShot(_sounds[3]);
                    break;
                case "menuClickSound":
                    _audioSFXSrc.PlayOneShot(_sounds[4]);
                    break;
                case "startGameSound":
                    _audioSFXSrc.PlayOneShot(_sounds[5]);
                    break;
                case "endGameSound":
                    _audioSFXSrc.PlayOneShot(_sounds[6]);
                    break;
                case "hitSound":
                    _audioSFXSrc.PlayOneShot(_sounds[7]);
                    break;
                case "scoreSound":
                    _audioSFXSrc.PlayOneShot(_sounds[8]);
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
                    _audioBackgroundSrc.clip = (_backgroundSounds[0]);
                    break;
                case "MainLevel":
                    _audioBackgroundSrc.clip = (_backgroundSounds[1]);
                    break;
                case "ScoringLevel":
                    _audioBackgroundSrc.clip = (_backgroundSounds[2]);
                    break;
                case "EndGame":
                    _audioBackgroundSrc.clip = (_backgroundSounds[3]);
                    break;
                default:
                    Debug.LogError("There is no sound with the given name:" + sound);
                    break;
            }
            _audioBackgroundSrc.loop = true;
            _audioBackgroundSrc.Play();
        }
    }
    public void PlayWalkingSounds(string sound)
    {
        if (!isMute && _audioWalkingSrc != null)
        {
            switch (sound)
            {
                case "Walking":
                    _audioWalkingSrc.clip = (_walkingSounds[0]);
                    break;
                default:
                    Debug.LogError("There is no sound with the given name:" + sound);
                    break;
            }
            _audioWalkingSrc.loop = true;
            _audioWalkingSrc.Play();
        }
    }

    public void StopWalkingSounds()
    {
        _audioWalkingSrc.Stop();
    }
}
