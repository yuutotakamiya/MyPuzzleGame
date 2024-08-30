using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Text handnumText;//�c��萔

    [SerializeField] GameObject GameOverText;//�Q�[���I�[�o�[�e�L�X�g

    [SerializeField] Text GameClear;//�Q�[���N���A�e�L�X�g

    int hand = 10;//�萔

    int Count;

    bool isMove = false;//�ړ������̃t���O

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
       
        if (isMove) return;//�ړ����͓��͂𖳎�

        //�萔��0�ɂȂ�����Q�[���I�[�o�[
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
                //���Ɉړ�
                direction = Vector3.left;
            }
            else if (flickValue_x > 50)
            {
                //�E�Ɉړ�
                direction = Vector3.right;
            }
            else if (flickValue_y > 50)
            {
                //���Ɉړ�
                direction = Vector3.forward;
            }
            else if (flickValue_y < -50)
            {
                //���Ɉړ�
                direction = Vector3.back;
            }

            if (direction != Vector3.zero)
            {
                DecreaseHand();

                //�����u���b�N�̃^�O���擾
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

    //�萔�����炷����
    void DecreaseHand()
    {
        if (hand > 0)
        {
            hand--;
            UpdateHandText();
        }
    }

    // �c��萔���X�V���郁�\�b�h
    void UpdateHandText()
    {
        if (handnumText != null)
        {
            handnumText.text = "�c��萔: " + hand.ToString();
        }
    }
}
