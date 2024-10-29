using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;

public static class LandStageID
{
    public const int landStageID = 11;
}

public class BlockManager : MonoBehaviour
{
    //UI
    [SerializeField] Text handnumText;//残り手数を表示
    [SerializeField] GameObject GameOverText;//ゲームオーバーテキスト
    [SerializeField] GameObject GameClear;//ゲームクリアテキスト
    [SerializeField] GameObject BackTitleButton;//タイトルに戻るボタン
    [SerializeField] GameObject CubeList;//ブロックの進化リスト
    [SerializeField] GameObject backStageSelectButton;//ステージ選択画面ボタン
    [SerializeField] GameObject NextStageButton;//次のステージへ
    [SerializeField] Text MinHand;//最短手数
    [SerializeField] Text Myhandnum;//自分自身の最短手数
    [SerializeField] GameObject EndText;//終了テキスト
    [SerializeField] GameObject RetryBuuton;
    GameObject helpButton;//ヘルプボタン

    //変数宣言
    [SerializeField] int hand;//現在の残り手数
    [SerializeField] int TotalNum;//フレームのトータル数
    [SerializeField] int CurrentNum;//現在のどのくらい埋めたか保存するための変数
    int useHandNum;
    int startHand;//最大手数
    public static int CurrentStageNum;//現在のステージ番号
    public static int landid;//島のIDを保存する変数

    //効果音
    [SerializeField] AudioClip TitleSE;//タイトルを押したときのSE
    [SerializeField] AudioClip StageNextSE;//次のステージに行くボタンを押した時のSE
    [SerializeField] AudioClip backStageSelectSE;//ステージ選択画面に行くときのSE

    //フラグ変数
    bool isMove = false;//移動中かのフラグ
    bool isCompleteClear = false;//クリアしたかどうか
    bool isGameOver = false;//ゲームオーバーしたかのフラグ
    bool isMenu = false;//メニューを開いてるかどうか

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

    AudioSource audioSource;

    void Start()
    {

        startHand = hand;

        UpdateHandText();

        TotalNum = GameObject.FindGameObjectsWithTag("ClearCube").Length;

        helpButton = GameObject.Find("helpButton");

        //ステージ10になったら次のステージボタンをfalseにする
        if (CurrentStageNum == 10)
        {
            NextStageButton.GetComponent<Button>().interactable = false;
        }

        audioSource = GetComponent<AudioSource>();

        //ステージの最短手数
        StartCoroutine(NetworkManager.Instance.GetStageMinHandNum(CurrentStageNum, result =>
        {
            if (result != null)
            {
                MinHand.text = result.MinHandNum.ToString();
            }
            else
            {
                MinHand.text = "0";
            }
        }));


        //自身の最短手数の呼び出し
        StartCoroutine(NetworkManager.Instance.GetStageMyHand(CurrentStageNum, result =>
        {
            if (result != null)
            {
                Myhandnum.text = result.MinHandNum.ToString();
            }
            else
            {
                Myhandnum.text = "0";
            }
        }));

    }

    void Update()
    {
        if (isMove || isCompleteClear || isGameOver || isMenu) return;//移動中またはクリアした場合は入力を無視する

        if (Input.GetMouseButtonDown(0) == true)
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            flickValue_x = endTouchPos.x - startTouchPos.x;
            flickValue_y = endTouchPos.y - startTouchPos.y;

            //絶対値を取得する関数
            float x = Math.Abs(flickValue_x);
            float y = Math.Abs(flickValue_y);

            Vector3 direction = Vector3.zero;
            if (flickValue_x < -50 && x > y)
            {
                //左に移動
                direction = Vector3.left;
            }
            else if (flickValue_x > 50 && x > y)
            {
                //右に移動
                direction = Vector3.right;
            }
            else if (flickValue_y > 50 && y > x)
            {
                //奥に移動
                direction = Vector3.forward;
            }
            else if (flickValue_y < -50 && y > x)
            {
                //手前に移動
                direction = Vector3.back;
            }

            if (direction != Vector3.zero)
            {
                DecreaseHand();

                //動くブロックのタグを取得
                GameObject[] fireCube = GameObject.FindGameObjectsWithTag("fireCube");
                GameObject[] combicube = GameObject.FindGameObjectsWithTag("combicube");
                GameObject[] waterCube = GameObject.FindGameObjectsWithTag("waterCube");
                GameObject[] rockCube = GameObject.FindGameObjectsWithTag("RockCube");

                GameObject[] List = new GameObject[fireCube.Length + combicube.Length + waterCube.Length + rockCube.Length];

                fireCube.CopyTo(List, 0);
                combicube.CopyTo(List, fireCube.Length);
                waterCube.CopyTo(List, fireCube.Length + combicube.Length);
                rockCube.CopyTo(List, fireCube.Length + combicube.Length + waterCube.Length);

                isMove = true;

                StartCoroutine(MoveBlock());
                for (int i = 0; i < List.Length; i++)
                {
                    Block block = List[i].GetComponent<Block>();
                    if (block != null)
                    {
                        block.Move(direction, () =>
                        {

                        });
                    }
                }
            }
        }
    }

    //ブロックを動かす関数
    private IEnumerator MoveBlock()
    {
        yield return new WaitForSeconds(1.0f);

        isMove = false;

        //手数が0になったらゲームオーバー
        if (hand <= 0 && !isCompleteClear)
        {
            isGameOver = true;

            //GameOverTextがnullじゃなかったら
            if (GameOverText != null)
            {
                GameOverText.SetActive(true);
            }
            //BackTitleButtonがnullじゃなかったら
            else if (BackTitleButton != null)
            {
                BackTitleButton.SetActive(true);
            }
            //helpButtonがnullじゃなかったら
            else if (helpButton != null)
            {
                helpButton.SetActive(false);
            }
            //backStageSelectButtonがnullじゃなかったら
            else if (backStageSelectButton != null)
            {
                backStageSelectButton.SetActive(true);
            }

        }
        if (CurrentStageNum >= LandStageID.landStageID && hand <= 0 && !isCompleteClear)
        {
            //島の状況登録APIの呼び出し
            StartCoroutine(NetworkManager.Instance.Registland(landid, CurrentNum, result =>
            {

                isGameOver = true;
                //EndTextがnullじゃなかったら
                if (EndText != null)
                {
                    EndText.SetActive(true);
                }
                //backStageSelectButtonがnullじゃなかったら
                else if (backStageSelectButton != null)
                {
                    backStageSelectButton.SetActive(true);
                }
                //BackTitleButtonがnullじゃなかったら
                else if (BackTitleButton != null)
                {
                    BackTitleButton.SetActive(true);
                }
                //GameOverTextがnullじゃなかったら
                else if (GameOverText != null)
                {
                    GameOverText.SetActive(false);   
                }

            }));
        }
    }

    //手数を減らす処理
    void DecreaseHand()
    {
        if (hand > 0)
        {
            hand--;
            UpdateHandText();
        }
    }

    // 残り手数を更新するメソッド
    void UpdateHandText()
    {
        if (handnumText != null)
        {
            handnumText.text = (startHand - hand).ToString() + " (MAX:" + startHand + ")";
        }
    }

    //タイトルに戻る関数
    public void FadeBackTitle()
    {
        Initiate.Fade("Title", Color.black, 1.0f);
        audioSource.PlayOneShot(TitleSE);
    }

    //リトライする関数
    public void Retry()
    {
        Addressables.LoadScene("Stage" + CurrentStageNum);
    }

    //現在のフレームの数がトータルのフレームの数が同じになったら
    public void AddCurrentNum()
    {
        //現在のフレーム数をカウントアップ
        CurrentNum++;

        //ゲームクリア処理
        if (CurrentNum == TotalNum)
        {
            useHandNum = (startHand - hand);

            //ステージクリアの登録
            StartCoroutine(NetworkManager.Instance.RegistStage(1, useHandNum, CurrentStageNum, request =>
            {
                if (request == true)
                {
                    isCompleteClear = true;
                    GameClear.SetActive(true);
                    BackTitleButton.SetActive(true);
                    helpButton.SetActive(false);
                    backStageSelectButton.SetActive(true);
                    NextStageButton.SetActive(true);
                    NetworkManager.Instance.StageClear(CurrentStageNum);
                    return;
                }
            }));

            if (CurrentStageNum >= LandStageID.landStageID)
            { 
                //島の状況登録
                StartCoroutine(NetworkManager.Instance.Registland(CurrentStageNum, CurrentNum, result =>
                {

                    isCompleteClear = true;

                    if (EndText != null)
                    {
                        EndText.SetActive(true);
                    }
                    else if (backStageSelectButton != null)
                    {
                        backStageSelectButton.SetActive(true);
                    }
                    else if (BackTitleButton != null)
                    {
                        BackTitleButton.SetActive(true);
                    }
                    else if (GameOverText != null)
                    {
                        GameOverText.SetActive(false);
                    }
                    return;
                }));
            }
        }
    }

    //ステージ選択画面に戻る
    public void FadeStageSelect()
    {
        Initiate.Fade("StageSelectScene", Color.black, 1.0f);
        audioSource.PlayOneShot(backStageSelectSE);
    }

    //次のステージに遷移する
    public void FadeNextStage()
    {
        CurrentStageNum += 1;
        Initiate.Fade("Stage" + CurrentStageNum, Color.black, 1.0f, true);
        audioSource.PlayOneShot(StageNextSE);
    }

    //現在のステージ番号を更新するメソッド
    static public void UpdateStageNum(int currentstagenum)
    {
        CurrentStageNum = currentstagenum;
        Initiate.Fade("Stage" + CurrentStageNum, Color.black, 1.0f, true);
    }

    //ランドIDを保存するためのメソッド
    static public void UpdateLandID(int id)
    {
        landid = id;
    }

    //LandIDを取得してくるメソッド
    static public int GetLandID()
    {
        return landid;
    }

    //  メニューボタンを押したときの処理
    public void MenuButton()
    {
        //現在のステージ番号が11以上だったら
        if (CurrentStageNum >= LandStageID.landStageID)
        {
            if (!isMenu)
            {
                backStageSelectButton.SetActive(true);
                BackTitleButton.SetActive(true);
            }  
            else
            {
                backStageSelectButton.SetActive(false);
                BackTitleButton.SetActive(false);
            }
          
        }
        else 
        {
            backStageSelectButton.SetActive(true);
            BackTitleButton.SetActive(true);
            RetryBuuton.SetActive(true);
        }

        isMenu = !isMenu;
    }
}
