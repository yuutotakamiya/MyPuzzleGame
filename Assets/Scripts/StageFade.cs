using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeStage()
    {
        Initiate.Fade("Stage1",Color.black,1.0f);
    }
}
