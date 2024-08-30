using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Text handnumText;//残り手数

    [SerializeField] GameObject GameOverText;//ゲームオーバーテキスト

    [SerializeField] Text GameClear;//ゲームクリアテキスト

    int hand = 10;//手数

    int Count;

    bool isMove = false;//移動中かのフラグ

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

    void Start()
    {
        UpdateHandText();
    }

    void Update()
    {
       
        if (isMove) return;//移動中は入力を無視

        //手数が0になったらゲームオーバー
        if (hand <=0 )
        {
            GameOverText.SetActive(true);
            return;
        }

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

                //動くブロックのタグを取得
                GameObject[] fireCube =  GameObject.FindGameObjectsWithTag("fireCube");
                GameObject[] combicube = GameObject.FindGameObjectsWithTag("combicube");
                GameObject[] waterCube = GameObject.FindGameObjectsWithTag("waterCube");
                GameObject[] rockCube = GameObject.FindGameObjectsWithTag("RockCube");

                GameObject[] List = new GameObject[fireCube.Length + combicube.Length + waterCube.Length + rockCube.Length];

                fireCube.CopyTo(List,0);
                combicube.CopyTo(List, fireCube.Length);
                waterCube.CopyTo(List, fireCube.Length + combicube.Length);
                rockCube.CopyTo(List, fireCube.Length + combicube.Length + waterCube.Length);

                for (int i = 0; i<List.Length; i++)
                {
                    Block block = List[i].GetComponent<Block>();
                    if (block != null)
                    {
                        block.Move(direction, () => {
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
            handnumText.text = "残り手数: " + hand.ToString();
        }
    }
}
