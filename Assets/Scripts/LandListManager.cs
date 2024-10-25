using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandListManager : MonoBehaviour
{
    [SerializeField] GameObject raidObj;

    [SerializeField] Transform Content;
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(NetworkManager.Instance.Getland(result =>
        {
            for (int i = 0; i < result.Length; i++)
            {
               GameObject raid =  Instantiate(raidObj ,transform.position, Quaternion.identity,Content);

                raid.transform.Find("TotalBlockNumText").GetComponent<Text>().text = result[i].BlockMissionSum.ToString();
                raid.transform.Find("CurrentBlockNumText").GetComponent<Text>().text = result[i].LandBlockNum.ToString();
            }
        }));
    }

    public void RaidButton()
    {
        Initiate.Fade("land1", Color.black, 1.0f);
    }
}
