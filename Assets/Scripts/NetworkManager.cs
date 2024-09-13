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
    private int userID = 0;//自分のユーザーID
    private string userName = "";//自分のユーザー名

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

    //ユーザー登録
    public IEnumerator RegistUser(string name,Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;

        //サーバーに送信するオブジェクトををJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        //送信
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/store", json,"application/json");

        yield return request.SendWebRequest();

        bool isSuccess = false;

        if (request.result == UnityWebRequest.Result.Success && request.responseCode==200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultjson);

            //ファイルにユーザーIDを保存
            this.userName = name;
            this.userID = response.UserID;
            SaveUserData(); 
            isSuccess = true;
        }
        result?.Invoke(isSuccess);//ここで呼び出し元の処理を呼び出す
    }

    //ユーザー情報を保存する
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

    //ユーザー情報を読み込む
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
