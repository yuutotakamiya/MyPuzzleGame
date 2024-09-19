using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip commonStageBGM;

    AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    // BGMを再生するメソッド
    public void PlayBGM(AudioClip bgmClip)
    {
        if (audioSource.clip != bgmClip)
        {
            audioSource.clip = bgmClip;
            audioSource.Play();
        }
    }

    // ステージのBGMを再生する
    public void PlayStageBGM()
    {
        PlayBGM(commonStageBGM);
    }

    // タイトルのBGMを再生する
    public void PlayTitleBGM()
    {
        PlayBGM(titleBGM);
    }
}
