using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    const string API_BASE_URL = "http://localhost:8000/api/";
    private  int userID = 0;//自分のユーザーID
    private string userName = "";//自分のユーザー名

    private int stageClearNum = 0;

    private int CurrentStageID; 

    private string authToken;//トークンを保存するための変数

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
            this.authToken = response.AuthToken;
            SaveUserData(); 
            isSuccess = true;
        }
        result?.Invoke(request.result == UnityWebRequest.Result.Success);//ここで呼び出し元の処理を呼び出す
    }


    //最短手数の取得
    public IEnumerator GetStageMinHandNum(int CurrentStageID,Action<MinHandNumResponse>result)
    {
        //最短手数のAPIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stage/"+ CurrentStageID);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            //jsonをStageResponseクラスの配列にデシリアライズ
            MinHandNumResponse response = JsonConvert.DeserializeObject<MinHandNumResponse>(resultjson);

            //結果を通知
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //自身の最短手数
    public IEnumerator GetStageMyHand(int CurrentStageID, Action<MyHandNum> result)
    {
        //最短手数のAPIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "min_hand_stage/" + CurrentStageID + "/"+ userID);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            //jsonをStageResponseクラスの配列にデシリアライズ
            MyHandNum response = JsonConvert.DeserializeObject<MyHandNum>(resultjson);

            //結果を通知
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }



    //ユーザー情報を保存する
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
        this.authToken = saveData.AuthToken;
        this.stageClearNum = saveData.StageClearNum;
        return true;
    }

    //ステージクリアする毎に呼び出されるメソッド
    public void StageClear(int StageNum)
    {
        if (StageNum > stageClearNum)
        {
            stageClearNum++;
            SaveUserData();
        }
    }
}
