using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] GameObject[] StageList;

    [SerializeField] AudioClip SE;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        for (int i= 0; i < NetworkManager.Instance.StageClearNum; ++i)
        {
            StageList[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeStage(int stagenum)
    {
        BlockManager.UpdateStageNum(stagenum);

        audioSource.PlayOneShot(SE);
    }

    public void RaidStage()
    {
        Initiate.Fade("LandList", Color.black, 1.0f,false);
    } 
}
