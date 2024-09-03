using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Text handnumText;//残り手数
    [SerializeField] GameObject GameOverText;//ゲームオーバーテキスト
    [SerializeField] GameObject GameClear;//ゲームクリアテキスト
    [SerializeField] GameObject RetryButton;//リトライボタン
    [SerializeField] GameObject BackHomeButton;//ホームに戻るボタン
    [SerializeField] GameObject CubeList;//ブロックの進化リスト
    [SerializeField] GameObject backStageSelectButton;//ステージ選択画面ボタン
    [SerializeField] GameObject NextStageButton;//次のステージへ

    int hand = 10;//手数

    int TotalNum;//フレームのトータル数
    int CurrentNum;//現在のどのくらい埋めたか保存するための変数
    static int CurrentStageNum;//現在のステージ番号

    bool isMove = false;//移動中かのフラグ
    bool isCompleteClear = false;//クリアしたかどうか

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

    GameObject helpButton;

    void Start()
    {
        UpdateHandText();

        TotalNum = GameObject.FindGameObjectsWithTag("ClearCube").Length;

        helpButton = GameObject.Find("helpButton");

        if (CurrentStageNum == 2)
        {
            NextStageButton.GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        if (isMove || isCompleteClear) return;//移動中またはクリアした場合は入力を無視する

        if (Input.GetMouseButtonDown(0) == true)
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            flickValue_x = endTouchPos.x - startTouchPos.x;
            flickValue_y = endTouchPos.y - startTouchPos.y;

            Vector3 direction = Vector3.zero;
            if (flickValue_x < -50)
            {
                //左に移動
                direction = Vector3.left;
            }
            else if (flickValue_x > 50)
            {
                //右に移動
                direction = Vector3.right;
            }
            else if (flickValue_y > 50)
            {
                //奥に移動
                direction = Vector3.forward;
            }
            else if (flickValue_y < -50)
            {
                //後ろに移動
                direction = Vector3.back;
            }

            if (direction != Vector3.zero)
            {
                DecreaseHand();

                //手数が0になったらゲームオーバー
                if (hand <= 0)
                {
                    GameOverText.SetActive(true);
                    RetryButton.SetActive(true);
                    BackHomeButton.SetActive(true);
                    helpButton.SetActive(false);
                    backStageSelectButton.SetActive(true);
                    return;
                }

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

                for (int i = 0; i < List.Length; i++)
                {
                    Block block = List[i].GetComponent<Block>();
                    if (block != null)
                    {
                        block.Move(direction, () =>
                        {
                            isMove = false;
                        });
                    }
                }
            }
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
            handnumText.text = hand.ToString();
        }
    }

    //ホームに戻る
    public void FadeBackHome()
    {
        Initiate.Fade("home", Color.black, 1.0f);
    }

    //リトライ
    public void Retry()
    {
        SceneManager.LoadScene("Stage1");
    }

    //現在のフレームの数がトータルのフレームの数が同じになったら
    public void AddCurrentNum()
    {
        //現在のフレーム数をカウントアップ
        CurrentNum++;

        //ゲームクリア処理
        if (CurrentNum == TotalNum)
        {
            isCompleteClear = true;
            GameClear.SetActive(true);
            BackHomeButton.SetActive(true);
            helpButton.SetActive(false);
            backStageSelectButton.SetActive(true);
            NextStageButton.SetActive(true);
            return;
        }
    }

    //ステージ選択画面に戻る
    public void FadeStageSelect()
    {
        Initiate.Fade("StageSelectScene", Color.black, 1.0f);
    }

    public void FadeNextStage()
    {
        CurrentStageNum += 1;
        Initiate.Fade("Stage"+ CurrentStageNum, Color.black, 1.0f);
    }

    static public void UpdateStageNum(int currentstagenum)
    {
        CurrentStageNum = currentstagenum;
        Initiate.Fade("Stage"+ CurrentStageNum, Color.black, 1.0f);
    }

}
