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
    private  int userID = 0;//自分のユーザーID
    private string userName = "";//自分のユーザー名
    private int stageClearNum = 0;//ステージクリアした番号

    private int CurrentStageID; //現在のステージIDを保存

    public string authToken;//トークンを保存するための変数

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
    public IEnumerator GetStageMyHand(int CurrentStageID, Action<MinHandNumResponse> result)
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
            MinHandNumResponse response = JsonConvert.DeserializeObject<MinHandNumResponse>(resultjson);

            //結果を通知
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //ステージクリアの登録
    public IEnumerator RegistStage(int result,int handnum,int stageid, Action<bool> request)
    {
        //サーバーに送信するオブジェクトを作成
        RegistStageRequest requestData = new RegistStageRequest();
        requestData.Result = result;
        requestData.StageID = stageid;
        requestData.HandNum = handnum;
        requestData.UserID = userID;

        //サーバーに送信するオブジェクトををJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        //送信
        UnityWebRequest webrequest = UnityWebRequest.Post(API_BASE_URL + "stage/store", json, "application/json");

        webrequest.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return webrequest.SendWebRequest();

        bool isSuccess = false;

        if (webrequest.result == UnityWebRequest.Result.Success && webrequest.responseCode == 200)
        {
            isSuccess = true;
        }
        request?.Invoke(isSuccess);//ここで呼び出し元の処理を呼び出す
    }

    //島の一覧取得
    public IEnumerator Getland(Action<RegistLandResponse[]> result)
    {
        //最短手数のAPIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "land/index");

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            //jsonをStageResponseクラスの配列にデシリアライズ
            RegistLandResponse[] response = JsonConvert.DeserializeObject<RegistLandResponse[]>(resultjson);

            //結果を通知
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //島の詳細情報取得
    public IEnumerator GetDetailsland(int landid, Action<RegistLandDetailsResponse> result)
    {
        //最短手数のAPIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "land/show"+ landid);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            //jsonをStageResponseクラスの配列にデシリアライズ
            RegistLandDetailsResponse response = JsonConvert.DeserializeObject<RegistLandDetailsResponse>(resultjson);

            //結果を通知
            result?.Invoke(response);
        }
        else
        {
            result?.Invoke(null);
        }
    }

    //島の状況登録
    public IEnumerator Registland(int landid,int landblocknum, Action<bool> request)
    {
        //サーバーに送信するオブジェクトを作成
        RegistlandRequest requestData = new RegistlandRequest();
        requestData.LandID = landid;
        requestData.LandBlockNum = landblocknum;
        requestData.UserID = userID;

        //サーバーに送信するオブジェクトををJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        //送信
        UnityWebRequest webrequest = UnityWebRequest.Post(API_BASE_URL + "land_block/store", json, "application/json");

        webrequest.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return webrequest.SendWebRequest();

        bool isSuccess = false;

        if (webrequest.result == UnityWebRequest.Result.Success && webrequest.responseCode == 200)
        {
            isSuccess = true;
        }
        request?.Invoke(isSuccess);//ここで呼び出し元の処理を呼び出す
    }



    //ユーザー情報とステージクリア番号を保存する
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

    //ユーザー情報とステージクリア番号を読み込む
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

    //トークン生成処理
    public IEnumerator CreateToken(Action<bool> result)
    {
        var requestData = new
        {
            user_id = this.userID,
        };

        //サーバーに送信するオブジェクトををJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        //送信
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/Token", json, "application/json");

        yield return request.SendWebRequest();

        bool isSuccess = false;

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultjson = request.downloadHandler.text;

            //jsonをStageResponseクラスの配列にデシリアライズ
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultjson);

            this.userID = response.UserID;
            this.authToken = response.AuthToken;
            SaveUserData();

            isSuccess = true;
        }

        result?.Invoke(request.result == UnityWebRequest.Result.Success);//ここで呼び出し元の処理を呼び出す

    }
}
