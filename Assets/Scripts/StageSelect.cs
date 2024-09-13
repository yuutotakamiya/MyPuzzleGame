using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [SerializeField] GameObject[] StageList;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    

}
