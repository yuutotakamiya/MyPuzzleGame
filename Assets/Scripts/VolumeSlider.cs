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
            // タイトルシーン用のBGMを再生
            SoundManager.instance.PlayTitleBGM();
        }

        else if (currentSceneName.Contains("Stage"))
        {
            // 共通のステージBGMを再生
            SoundManager.instance.PlayStageBGM();
        }
    }
}
