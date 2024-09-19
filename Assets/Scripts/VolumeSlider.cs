using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public void SetBGMVolume(float volume)
    {
        SoundManager.instance.SetBGMVolume(volume);
    }

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Title")
        {
            // �^�C�g���V�[���p��BGM���Đ�
            SoundManager.instance.PlayTitleBGM();
        }

        else if (currentSceneName.Contains("Stage"))
        {
            // ���ʂ̃X�e�[�WBGM���Đ�
            SoundManager.instance.PlayStageBGM();
        }
    }
}
