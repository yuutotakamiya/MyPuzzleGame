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

    // BGM���Đ����郁�\�b�h
    public void PlayBGM(AudioClip bgmClip)
    {
        if (audioSource.clip != bgmClip)
        {
            audioSource.clip = bgmClip;
            audioSource.Play();
        }
    }

    // �X�e�[�W��BGM���Đ�����
    public void PlayStageBGM()
    {
        PlayBGM(commonStageBGM);
    }

    // �^�C�g����BGM���Đ�����
    public void PlayTitleBGM()
    {
        PlayBGM(titleBGM);
    }
}
