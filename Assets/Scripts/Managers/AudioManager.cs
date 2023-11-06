using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FXSoundID
{
    /*
    **  /!\ manually needs to synchronize the list of FXSoundID enum here,
    **  with m_fxAudioClips that is also manually initialized in Unity editor
    */

    select1, select2, clickOnBtn1, startGame, construct1
};

public enum EVolume
{
    v10p, v25p, v50p, v75p, v100p
};


public class AudioManager : T_Singleton<AudioManager>
{
    public AudioClip m_backgroundAudioClip;
    public AudioClip[] m_fxAudioClips;

    private AudioSource m_backgroundAudioSrc;
    private List<AudioSource> m_audioSources = new List<AudioSource>();

    private bool m_muteAudio = false;

    private const float m_DEFAULT_FX_VOLUME = 1.0f;

    override protected void Awake()
    {
        base.Awake();
        transform.parent = null; // make sure it is a "root level gameObject"
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    // Own methods :
    private void Initialize()
    {
        if (m_backgroundAudioClip != null)
        {
            m_backgroundAudioSrc = gameObject.AddComponent<AudioSource>();
            m_backgroundAudioSrc.clip = m_backgroundAudioClip;
            m_backgroundAudioSrc.loop = true;
            m_backgroundAudioSrc.volume = 0.2f;
            m_backgroundAudioSrc.Play();
        }
        else
        {
            m_backgroundAudioSrc = null;
            Debug.LogWarning("missing backgroundAudioClip in " + gameObject.name);
        }
    }

    // still need for 'static' methods here ? (since new TSingleton<X> inheritance...)
    public static void PlayFXSound(FXSoundID x_soundID)
    {
        AudioSource audioSrc = _instance.GetAudioSourceWithFXClip(x_soundID);
        audioSrc.Play();
    }
    public static void PlayFXSound(FXSoundID x_soundID, EVolume x_enumVolume)
    {
        AudioSource audioSrc = _instance.GetAudioSourceWithFXClip(x_soundID);
        audioSrc.volume = _instance.GetFloatVolumeFromEnum(x_enumVolume);
        audioSrc.Play();
    }

    private float GetFloatVolumeFromEnum(EVolume x_enumVolume)
    {
        float floatVolume;
        switch (x_enumVolume)
        {
            case EVolume.v10p:
                floatVolume = 0.1f;
                break;
            case EVolume.v25p:
                floatVolume = 0.25f;
                break;
            case EVolume.v50p:
                floatVolume = 0.50f;
                break;
            case EVolume.v75p:
                floatVolume = 0.75f;
                break;
            case EVolume.v100p:
                floatVolume = 1f;
                break;
            default:
                Debug.LogWarning(name + " : unexpected enum Volume value");
                floatVolume = 1f;
                break;
        }
        return floatVolume;
    }

    private AudioSource GetAudioSourceWithFXClip(FXSoundID x_soundID)
    {
        AudioSource audioSrc = _instance.GetNextIdleAudioSrc();
        if (audioSrc == null)
        {
            audioSrc = _instance.CreateAudioSrc();
        }
        audioSrc.clip = _instance.m_fxAudioClips[(int)x_soundID];
        audioSrc = InitIdleAudioSrc(audioSrc);
        return audioSrc;
    }

    private AudioSource InitIdleAudioSrc(AudioSource x_audioSrc)
    {
        x_audioSrc.volume = m_DEFAULT_FX_VOLUME;
        x_audioSrc.mute = m_muteAudio;
        return x_audioSrc;
    }

    private AudioSource GetNextIdleAudioSrc()
    {
        AudioSource audioSrc = null;
        for (int i = 0; i < m_audioSources.Count; i++)
        {
            if (m_audioSources[i] != null && !m_audioSources[i].isPlaying)
            {
                audioSrc = m_audioSources[i];
                break;
            }
        }
        return audioSrc;
    }

    private AudioSource CreateAudioSrc()
    {
        AudioSource audioSrc = gameObject.AddComponent<AudioSource>();
        // Note : we can do some treatment on all newly created audioSrc here...
        // like for example, disabling 'playOnAwake' that is true by default :
        // audioSrc.playOnAwake = false;
        m_audioSources.Add(audioSrc);
        return audioSrc;
    }

    public static void MuteGlobalAudioSystem()
    {
        MuteBackgroundMusic();
        _instance.m_muteAudio = true;
    }
    public static void UnmuteGlobalAudioSystem()
    {
        UnmuteBackgroundMusic();
        _instance.m_muteAudio = false;
    }
    public static void MuteBackgroundMusic()
    {
        if (_instance.m_backgroundAudioSrc != null)
        {
            _instance.m_backgroundAudioSrc.mute = true;
        }
    }
    public static void UnmuteBackgroundMusic()
    {
        if (_instance.m_backgroundAudioSrc != null)
        {
            _instance.m_backgroundAudioSrc.mute = false;
        }
    }

    public static bool IsAudioMute()
    {
        return _instance.m_muteAudio;
    }
}
