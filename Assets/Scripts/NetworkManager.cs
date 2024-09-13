using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    const string API_BASE_URL = "https://api-cube-puzzle.japaneast.cloudapp.azure.com/api/";
    private int userID = 0;//�����̃��[�U�[ID
    private string userName = "";//�����̃��[�U�[��

    private int stageClearNum = 0;

    private static NetworkManager intstance;

    public int StageClearNum
    {
        get { return stageClearNum; }
    }

    public static NetworkManager Instance
    {
        get{
            if (intstance==null)
            {
                GameObject gameObject = new GameObject("NetworkManager");
                intstance = gameObject.AddComponent<NetworkManager>();
                DontDestroyOnLoad(gameObject);
            }
            return intstance;
        }
    }

    //���[�U�[�o�^
    public IEnumerator RegistUser(string name,Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;

        //�T�[�o�[�ɑ��M����I�u�W�F�N�g����JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        //���M
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/store", json,"application/json");

        yield return request.SendWebRequest();

        bool isSuccess = false;

        if (request.result == UnityWebRequest.Result.Success && request.responseCode==200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultjson);

            //�t�@�C���Ƀ��[�U�[ID��ۑ�
            this.userName = name;
            this.userID = response.UserID;
            SaveUserData(); 
            isSuccess = true;
        }
        result?.Invoke(isSuccess);//�����ŌĂяo�����̏������Ăяo��
    }

    //���[�U�[����ۑ�����
    private void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.Name = this.userName;
        saveData.UserID = this.userID;
        saveData.StageClearNum = this.StageClearNum;
        string json = JsonConvert.SerializeObject(saveData);
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();

    }

    //���[�U�[����ǂݍ���
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            return false;
        }

        var reader = new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        this.userID = saveData.UserID;
        this.userName = saveData.Name;
        this.stageClearNum = saveData.StageClearNum;
        return true;
    }

    //
    public void StageClear(int StageNum)
    {
        if (StageNum > stageClearNum)
        {
            stageClearNum++;
            SaveUserData();
        }
        
    }

    /*public IEnumerator Getstage(Action<StageResponse[]> result)
    {
        UnityWebRequest request = UnityWebRequest.Get();
        yield return request = SendWebRequest();

    }*/
}
