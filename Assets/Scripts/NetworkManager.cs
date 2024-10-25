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
    private  int userID = 0;//�����̃��[�U�[ID
    private string userName = "";//�����̃��[�U�[��
    private int stageClearNum = 0;//�X�e�[�W�N���A�����ԍ�

    private int CurrentStageID; //���݂̃X�e�[�WID��ۑ�

    public string authToken;//�g�[�N����ۑ����邽�߂̕ϐ�

    private static NetworkManager intstance;

    public int StageClearNum
    {
        get { return stageClearNum; }
    }
    public int CurrentStageId
    {
        get { return CurrentStageID; }
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
            this.authToken = response.AuthToken;
            SaveUserData(); 
            isSuccess = true;
        }
        result?.Invoke(request.result == UnityWebRequest.Result.Success);//�����ŌĂяo�����̏������Ăяo��
    }


    //�ŒZ�萔�̎擾
    public IEnumerator GetStageMinHandNum(int CurrentStageID,Action<MinHandNumResponse>result)
    {
        //�ŒZ�萔��API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stage/"+ CurrentStageID);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            //json��StageResponse�N���X�̔z��Ƀf�V���A���C�Y
            MinHandNumResponse response = JsonConvert.DeserializeObject<MinHandNumResponse>(resultjson);

            //���ʂ�ʒm
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //���g�̍ŒZ�萔
    public IEnumerator GetStageMyHand(int CurrentStageID, Action<MinHandNumResponse> result)
    {
        //�ŒZ�萔��API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "min_hand_stage/" + CurrentStageID + "/"+ userID);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            //json��StageResponse�N���X�̔z��Ƀf�V���A���C�Y
            MinHandNumResponse response = JsonConvert.DeserializeObject<MinHandNumResponse>(resultjson);

            //���ʂ�ʒm
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //�X�e�[�W�N���A�̓o�^
    public IEnumerator RegistStage(int result,int handnum,int stageid, Action<bool> request)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistStageRequest requestData = new RegistStageRequest();
        requestData.Result = result;
        requestData.StageID = stageid;
        requestData.HandNum = handnum;
        requestData.UserID = userID;

        //�T�[�o�[�ɑ��M����I�u�W�F�N�g����JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        //���M
        UnityWebRequest webrequest = UnityWebRequest.Post(API_BASE_URL + "stage/store", json, "application/json");

        webrequest.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return webrequest.SendWebRequest();

        bool isSuccess = false;

        if (webrequest.result == UnityWebRequest.Result.Success && webrequest.responseCode == 200)
        {
            isSuccess = true;
        }
        request?.Invoke(isSuccess);//�����ŌĂяo�����̏������Ăяo��
    }

    //���̈ꗗ�擾
    public IEnumerator Getland(Action<RegistLandResponse[]> result)
    {
        //�ŒZ�萔��API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "land/index");

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            //json��StageResponse�N���X�̔z��Ƀf�V���A���C�Y
            RegistLandResponse[] response = JsonConvert.DeserializeObject<RegistLandResponse[]>(resultjson);

            //���ʂ�ʒm
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //���̏ڍ׏��擾
    public IEnumerator GetDetailsland(int landid, Action<RegistLandDetailsResponse> result)
    {
        //�ŒZ�萔��API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "land/show"+ landid);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            //json��StageResponse�N���X�̔z��Ƀf�V���A���C�Y
            RegistLandDetailsResponse response = JsonConvert.DeserializeObject<RegistLandDetailsResponse>(resultjson);

            //���ʂ�ʒm
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //���̏󋵓o�^
    public IEnumerator Registland(int landid,int landblocknum, Action<bool> request)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistlandRequest requestData = new RegistlandRequest();
        requestData.LandID = landid;
        requestData.LandBlockNum = landblocknum;
        requestData.UserID = userID;

        //�T�[�o�[�ɑ��M����I�u�W�F�N�g����JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        //���M
        UnityWebRequest webrequest = UnityWebRequest.Post(API_BASE_URL + "land_block/store", json, "application/json");

        webrequest.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return webrequest.SendWebRequest();

        bool isSuccess = false;

        if (webrequest.result == UnityWebRequest.Result.Success && webrequest.responseCode == 200)
        {
            isSuccess = true;
        }
        request?.Invoke(isSuccess);//�����ŌĂяo�����̏������Ăяo��
    }



    //���[�U�[���ƃX�e�[�W�N���A�ԍ���ۑ�����
    private void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.Name = this.userName;
        saveData.UserID = this.userID;
        saveData.AuthToken = this.authToken;
        saveData.StageClearNum = this.StageClearNum;
        string json = JsonConvert.SerializeObject(saveData);
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    //���[�U�[���ƃX�e�[�W�N���A�ԍ���ǂݍ���
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
        this.authToken = saveData.AuthToken;
        this.stageClearNum = saveData.StageClearNum;
        return true;
    }

    //�X�e�[�W�N���A���閈�ɌĂяo����郁�\�b�h
    public void StageClear(int StageNum)
    {
        if (StageNum > stageClearNum)
        {
            stageClearNum++;
            SaveUserData();
        }
    }

    //�g�[�N����������
    public IEnumerator CreateToken(Action<bool> result)
    {
        var requestData = new
        {
            user_id = this.userID,
        };

        //�T�[�o�[�ɑ��M����I�u�W�F�N�g����JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        //���M
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/Token", json, "application/json");

        yield return request.SendWebRequest();

        bool isSuccess = false;

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultjson = request.downloadHandler.text;

            //json��StageResponse�N���X�̔z��Ƀf�V���A���C�Y
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultjson);

            this.userID = response.UserID;
            this.authToken = response.AuthToken;
            SaveUserData();

            isSuccess = true;
        }

        result?.Invoke(request.result == UnityWebRequest.Result.Success);//�����ŌĂяo�����̏������Ăяo��

    }
}
