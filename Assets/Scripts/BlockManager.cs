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
    [SerializeField] Text handnumText;//�c��萔
    [SerializeField] GameObject GameOverText;//�Q�[���I�[�o�[�e�L�X�g
    [SerializeField] GameObject GameClear;//�Q�[���N���A�e�L�X�g
    [SerializeField] GameObject RetryButton;//���g���C�{�^��
    [SerializeField] GameObject BackHomeButton;//�z�[���ɖ߂�{�^��
    [SerializeField] GameObject CubeList;//�u���b�N�̐i�����X�g
    [SerializeField] GameObject backStageSelectButton;//�X�e�[�W�I����ʃ{�^��
    [SerializeField] GameObject NextStageButton;//���̃X�e�[�W��

    int hand = 10;//�萔

    int TotalNum;//�t���[���̃g�[�^����
    int CurrentNum;//���݂̂ǂ̂��炢���߂����ۑ����邽�߂̕ϐ�
    static int CurrentStageNum;//���݂̃X�e�[�W�ԍ�

    bool isMove = false;//�ړ������̃t���O
    bool isCompleteClear = false;//�N���A�������ǂ���

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
        if (isMove || isCompleteClear) return;//�ړ����܂��̓N���A�����ꍇ�͓��͂𖳎�����

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

                //�萔��0�ɂȂ�����Q�[���I�[�o�[
                if (hand <= 0)
                {
                    GameOverText.SetActive(true);
                    RetryButton.SetActive(true);
                    BackHomeButton.SetActive(true);
                    helpButton.SetActive(false);
                    backStageSelectButton.SetActive(true);
                    return;
                }

                //�����u���b�N�̃^�O���擾
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
            handnumText.text = hand.ToString();
        }
    }

    //�z�[���ɖ߂�
    public void FadeBackHome()
    {
        Initiate.Fade("home", Color.black, 1.0f);
    }

    //���g���C
    public void Retry()
    {
        SceneManager.LoadScene("Stage1");
    }

    //���݂̃t���[���̐����g�[�^���̃t���[���̐��������ɂȂ�����
    public void AddCurrentNum()
    {
        //���݂̃t���[�������J�E���g�A�b�v
        CurrentNum++;

        //�Q�[���N���A����
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

    //�X�e�[�W�I����ʂɖ߂�
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
