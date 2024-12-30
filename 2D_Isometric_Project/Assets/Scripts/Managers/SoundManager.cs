using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    PickBox,
    DropBox,
    ButtonClick,
    Victory,
    Lose
}

public enum BGMType
{
    MainMenu,
    Level
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("Sound Lists")]
    [SerializeField] private List<AudioClip> sfxClips;
    [SerializeField] private List<AudioClip> bgmClips;

    [Header("Volume Settings")]
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float bgmVolume = 1f;
    [SerializeField] private float bgmRelativeVolume = 0.6f; // Multiplier for BGM to make it quieter by default

    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string MUTE_KEY = "Mute";

    private bool isMuted = false;

    public bool IsMuted => isMuted;
    public float SfxVolume => sfxVolume;
    public float BgmVolume => bgmVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;

            // Load saved settings
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
            isMuted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;

            if (sfxSource != null) 
            {
                sfxSource.volume = isMuted ? 0f : sfxVolume;
            }

            if (bgmSource != null) 
            {
                bgmSource.volume = isMuted ? 0f : bgmVolume * bgmRelativeVolume;
            }
        }        
    }

    public void PlaySFX(SFXType sfxType)
    {
        int sfxIndex = (int)sfxType;

        if (sfxIndex >= 0 && sfxIndex < sfxClips.Count)
        {
            sfxSource.PlayOneShot(sfxClips[sfxIndex], sfxVolume);
        }
    }

    public void PlayBGM(BGMType bgmType)
    {
        int bgmIndex = (int)bgmType;

        if (bgmIndex >= 0 && bgmIndex < bgmClips.Count)
        {
            if (bgmSource.clip != bgmClips[bgmIndex])
            {
                bgmSource.Stop();
                bgmSource.clip = bgmClips[bgmIndex];
                bgmSource.loop = true;
                SetBGMVolume(bgmVolume);
                bgmSource.Play();
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        if (sfxSource != null && !isMuted)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        if (bgmSource != null && !isMuted)
        {
            bgmSource.volume = bgmVolume * bgmRelativeVolume;
        }
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        PlayerPrefs.SetInt(MUTE_KEY, isMuted ? 1 : 0);

        if (sfxSource != null) 
        {
            sfxSource.volume = mute ? 0f : sfxVolume;
        }

        if (bgmSource != null) 
        {
            bgmSource.volume = mute ? 0f : bgmVolume * bgmRelativeVolume;
        }
    }
}
