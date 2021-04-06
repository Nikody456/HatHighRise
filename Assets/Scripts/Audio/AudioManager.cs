using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public bool isMute = false;

    private AudioSource _audioSrc;
    [SerializeField] AudioClip[] _sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        _audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        if (!isMute)
        {
            switch (sound)
            {
                case "jumpSound":
                    _audioSrc.PlayOneShot(_sounds[0]);
                    break;
                case "otherSound":
                    _audioSrc.PlayOneShot(_sounds[1]);
                    break;
                default:
                    Debug.LogError("There is no sound with the given name:" + sound);
                    break;
            }
        }
    }
}
