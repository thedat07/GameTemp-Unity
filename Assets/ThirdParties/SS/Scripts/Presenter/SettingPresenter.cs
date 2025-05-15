using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;

public class SettingPresenter : MonoBehaviour, IInitializable
{
    public static readonly Vector2 ScreenGame = new Vector2(1080f, 2160f);
    
    private SettingData m_SettingData;

    public LeanGameObjectPool audioMusic;
    public LeanGameObjectPool audioSound;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightedColor = new Color(0.9f, 0.9f, 0.9f);
    public Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    public Color selectedColor = Color.white;
    public Color disabledColor = new Color(0.6f, 0.6f, 0.6f);

    [Header("Transition Settings")]
    public float colorMultiplier = 1f;
    public float fadeDuration = 0.1f;

    [Header("Click")]
    public AudioClip audioClick;
    public AudioClip[] LstAudio;
    public AudioClip audioBGRef;

    [Header("DOTween")]
    public bool autoKillMode;
    public bool useSafeMode;
    public LogBehaviour logBehaviour;

    public void Initialize()
    {
        DOTween.Init(autoKillMode, useSafeMode, logBehaviour);
        m_SettingData = GameManager.Instance.GetSettingData();
    }

    public void SetSound()
    {
        m_SettingData.Sound = !m_SettingData.Sound;
        if (m_SettingData.Music == false)
        {
            audioSound.DespawnAll();
        }
        TigerForge.EventManager.EmitEvent(SettingData.Key);
    }

    public void SetMusic()
    {
        m_SettingData.Music = !m_SettingData.Music;
        if (m_SettingData.Music == false)
        {
            audioMusic.DespawnAll();
        }
        else
        {
            PlayMusic();
        }
        TigerForge.EventManager.EmitEvent(SettingData.Key);
    }

    public void SetVibration()
    {
        m_SettingData.Vibration = !m_SettingData.Vibration;
        TigerForge.EventManager.EmitEvent(SettingData.Key);
    }

    public void PlaySound(AudioClip clip)
    {
        if (m_SettingData.Sound == true && clip != null)
        {
            if (audioSound.Spawn(transform).TryGetComponent<AudioSource>(out AudioSource audio))
            {
                PlayPool(ref audio);
            }
        }
    }

    public void PlaySound(TypeAudio type)
    {
        if (m_SettingData.Sound == true)
        {
            if (audioSound.Spawn(transform).TryGetComponent<AudioSource>(out AudioSource audio))
            {
                if (type == TypeAudio.ButtonClick)
                {
                    audio.clip = audioClick;
                }
                else
                {
                    audio.clip = LstAudio[((int)type)];
                }

                PlayPool(ref audio);
            }
        }
    }

    public void PlayMusic()
    {
        if (m_SettingData.Music == true)
        {
            audioMusic.DespawnAll();
            if (audioMusic.Spawn(transform).TryGetComponent<AudioSource>(out AudioSource audio))
            {
                audio.clip = audioBGRef;
                PlayPool(ref audio);
            }
        }
    }

    public void PlaySoundClick(AudioClip clip = null)
    {
        if (clip == null)
        {
            PlaySound(audioClick);
        }
        else
        {
            PlaySound(clip);
        }
    }

    void PlayPool(ref AudioSource audio)
    {
        if (audio.clip != null)
        {
            audio.name = audio.clip.name;
            audio.Stop();
            audio.Play();
            Lean.Pool.LeanPool.Despawn(audio, audio.clip.length);
        }
    }

    public void TapPopVibrate()
    {
        //         if (!m_SettingData.Vibration) return;
        // #if UNITY_ANDROID
        //         Vibration.VibratePop();
        // #endif

        // #if UNITY_IOS
        //         Vibration.VibrateIOS(ImpactFeedbackStyle.Light);
        // #endif
    }

    public void TapPeekVibrate()
    {
        //         if (!m_SettingData.Vibration) return;
        // #if UNITY_ANDROID
        //         Vibration.VibratePeek();
        // #endif

        // #if UNITY_IOS
        //         Vibration.VibrateIOS(ImpactFeedbackStyle.Medium);
        // #endif
    }
}

public enum TypeAudio
{
    ButtonClick = -1,
}