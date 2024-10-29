using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandListManager : MonoBehaviour
{
    [SerializeField] GameObject[] raidObj;

    [SerializeField] Transform Content;
    // Start is called before the first frame update
    void Start()
    {
        //島一覧のAPI呼び出し
        StartCoroutine(NetworkManager.Instance.Getland(result =>
        {
            //resultの結果の長さ分繰り返す
            for (int i = 0; i < result.Length; i++)
            {
                //島の一覧を生成
                GameObject raid = Instantiate(raidObj[result[i].StageID - LandStageID.landStageID] ,transform.position, Quaternion.identity,Content);

                raid.transform.Find("TotalBlockNumText").GetComponent<Text>().text = result[i].BlockMissionSum.ToString();
                raid.transform.Find("CurrentBlockNumText").GetComponent<Text>().text = result[i].LandBlockNum.ToString();
                
                //挑戦ボタンを押したらLandIDを保存
                raid.transform.Find("FadeLand").GetComponent<FadeLaid>().LandID = result[i].LandID;

                //挑戦ボタンを押したらStageIDを保存
                raid.transform.Find("FadeLand").GetComponent<FadeLaid>().StageID = result[i].StageID;

                //ブロックを埋める目標数より現在の埋める個数が多かったら
                if (result[i].LandBlockNum>=result[i].BlockMissionSum)
                {
                    //挑戦ボタンを押せないようにする
                    raid.transform.Find("RaidButton").GetComponent<Button>().interactable = false;
                    raid.transform.Find("Panel").gameObject.SetActive(true);
                    raid.transform.Find("completeText").gameObject.SetActive(true);
                  
                }
            }
        }));
    }
}
