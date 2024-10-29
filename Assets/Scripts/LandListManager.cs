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
        //���ꗗ��API�Ăяo��
        StartCoroutine(NetworkManager.Instance.Getland(result =>
        {
            //result�̌��ʂ̒������J��Ԃ�
            for (int i = 0; i < result.Length; i++)
            {
                //���̈ꗗ�𐶐�
                GameObject raid = Instantiate(raidObj[result[i].StageID - LandStageID.landStageID] ,transform.position, Quaternion.identity,Content);

                raid.transform.Find("TotalBlockNumText").GetComponent<Text>().text = result[i].BlockMissionSum.ToString();
                raid.transform.Find("CurrentBlockNumText").GetComponent<Text>().text = result[i].LandBlockNum.ToString();
                
                //����{�^������������LandID��ۑ�
                raid.transform.Find("FadeLand").GetComponent<FadeLaid>().LandID = result[i].LandID;

                //����{�^������������StageID��ۑ�
                raid.transform.Find("FadeLand").GetComponent<FadeLaid>().StageID = result[i].StageID;

                //�u���b�N�𖄂߂�ڕW����茻�݂̖��߂��������������
                if (result[i].LandBlockNum>=result[i].BlockMissionSum)
                {
                    //����{�^���������Ȃ��悤�ɂ���
                    raid.transform.Find("RaidButton").GetComponent<Button>().interactable = false;
                    raid.transform.Find("Panel").gameObject.SetActive(true);
                    raid.transform.Find("completeText").gameObject.SetActive(true);
                  
                }
            }
        }));
    }
}
