using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LandListManager : MonoBehaviour
{
    [SerializeField] Text CurrentNumText;//���݃u���b�N�����܂��Ă��鐔

    [SerializeField] Text TotalBlockNumText;//�u���b�N���߂鍇�v��

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NetworkManager.Instance.Getland(result =>
        {

            if (result != null)
            {
                CurrentNumText.text = result[0].LandBlockNum.ToString();
                TotalBlockNumText.text = result[0].BlockMissionSum.ToString();
            }
            else
            {
                CurrentNumText.text = "0";
                TotalBlockNumText.text = "0";
            }

        }));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void RaidButton()
    {
        Initiate.Fade("land1", Color.black, 1.0f);
    }
}
