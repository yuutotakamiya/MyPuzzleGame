using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public void SetBGMVolume(float volume)
    {
        SoundManager.instance.SetBGMVolume(volume);
    }
}
