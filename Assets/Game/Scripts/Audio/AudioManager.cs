using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipEntry
{
    public string key;        // Tên dùng để gọi PlayBGM / PlaySFX
    public AudioClip clip;    // File audio
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClipEntry[] bgmEntries;

    [Header("SFX Clips")]
    public AudioClipEntry[] sfxEntries;

    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> sfxDict;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Nếu chưa phát thì phát luôn
        if (!bgmSource.isPlaying)
        {
            PlayBGM("MainMenu", true); // Thay "MainMenu" bằng key bài nhạc bạn muốn
        }
    }
    public void SetVolumeBGM(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }
    public void SetVolumeSFX(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    private void InitDictionaries()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var entry in bgmEntries)
        {
            if (!bgmDict.ContainsKey(entry.key))
                bgmDict.Add(entry.key, entry.clip);
        }

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sfxEntries)
        {
            if (!sfxDict.ContainsKey(entry.key))
                sfxDict.Add(entry.key, entry.clip);
        }
    }

    // ---------------------- BGM Control ----------------------
    public void PlayBGM(string name, bool loop = true)
    {
        if (bgmDict.TryGetValue(name, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found: " + name);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    // ---------------------- SFX Control ----------------------
    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {   
            if (clip ==null) // Kiểm tra nếu không đang phát hoặc đang phát clip khác
            {
                return;
            }
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    public void PlaySFX(string name, float volume = 1f)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume) * sfxSource.volume);
        }
    }

}
