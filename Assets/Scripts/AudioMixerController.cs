using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgmVolume;
    public Slider sfxVolume;
    public Slider voiceVolume;

    public GameObject setupBtn;

    private bool setupOpend = false;
    void Start()
    {
        setupOpend = false;
        if (bgmVolume != null)
        {
            bgmVolume.value = PlayerPrefs.GetFloat("BgmVolume", 0.2f);
            SetBgm(bgmVolume.value);
        }
        if(sfxVolume != null)
        {
            sfxVolume.value = PlayerPrefs.GetFloat("SfxVolume", 0.2f);
            SetSFX(sfxVolume.value);
        }
        if(voiceVolume != null)
        {
            voiceVolume.value = PlayerPrefs.GetFloat("VoiceVolume", 0.2f);
            SetVoice(voiceVolume.value);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (setupOpend && Input.GetKeyDown(KeyCode.Escape))
        {
            setupBtn.gameObject.SetActive(false);
            setupOpend = false;
        }
    }

    public void SetBgm(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("BgmVolume", volume);
        }     
    }

    public void SetSFX(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SfxVolume", volume);
        }
    }

    public void SetVoice(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Voice", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("VoiceVolume", volume);
        }
    }
    public void OpenSetup()
    {
        setupBtn.gameObject.SetActive(true);
        setupOpend = true;
    }
    public void CloseSetup()
    {
        setupBtn.gameObject.SetActive(false);
        setupOpend = false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
