using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLaid : MonoBehaviour
{
    public int LandID {  get; set; }
    

    public int StageID {  get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RaidButton()
    {
        BlockManager.UpdateStageNum(StageID);
        BlockManager.UpdateLandID(LandID);
        Initiate.Fade("Stage"+ StageID, Color.black, 1.0f,true);
    }
}
