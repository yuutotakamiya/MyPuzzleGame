using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AddressableAssets;

public class BlockManager : MonoBehaviour
{
    //UI
    [SerializeField] Text handnumText;//�c��萔
    [SerializeField] GameObject GameOverText;//�Q�[���I�[�o�[�e�L�X�g
    [SerializeField] GameObject GameClear;//�Q�[���N���A�e�L�X�g
    [SerializeField] GameObject BackTitleButton;//�^�C�g���ɖ߂�{�^��
    [SerializeField] GameObject CubeList;//�u���b�N�̐i�����X�g
    [SerializeField] GameObject backStageSelectButton;//�X�e�[�W�I����ʃ{�^��
    [SerializeField] GameObject NextStageButton;//���̃X�e�[�W��
     GameObject helpButton;//�w���v�{�^��

    //�ϐ��錾
    [SerializeField]int hand;//�萔
    [SerializeField]int TotalNum;//�t���[���̃g�[�^����
    [SerializeField]int CurrentNum;//���݂̂ǂ̂��炢���߂����ۑ����邽�߂̕ϐ�
    static int CurrentStageNum;//���݂̃X�e�[�W�ԍ�

    //���ʉ�
    [SerializeField] AudioClip TitleSE;//�^�C�g�����������Ƃ���SE
    [SerializeField] AudioClip StageNextSE;//���̃X�e�[�W�ɍs���{�^��������������SE
    [SerializeField] AudioClip backStageSelectSE;//�X�e�[�W�I����ʂɍs���Ƃ���SE
    
    bool isMove = false;//�ړ������̃t���O
    bool isCompleteClear = false;//�N���A�������ǂ���
    bool isGameOver = false;//�Q�[���I�[�o�[�������̃t���O

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

    AudioSource audioSource;

    void Start()
    {
        UpdateHandText();

        TotalNum = GameObject.FindGameObjectsWithTag("ClearCube").Length;

        helpButton = GameObject.Find("helpButton");

        if (CurrentStageNum == 10)
        {
            NextStageButton.GetComponent<Button>().interactable = false;
        }

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (isMove || isCompleteClear || isGameOver ) return;//�ړ����܂��̓N���A�����ꍇ�͓��͂𖳎�����

        if (Input.GetMouseButtonDown(0) == true)
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            flickValue_x = endTouchPos.x - startTouchPos.x;
            flickValue_y = endTouchPos.y - startTouchPos.y;

            //��Βl���擾����֐�
            float x = Math.Abs(flickValue_x);
            float y = Math.Abs(flickValue_y);

            Vector3 direction = Vector3.zero;
            if (flickValue_x < -50 && x > y)
            {
                //���Ɉړ�
                direction = Vector3.left;
            }
            else if (flickValue_x > 50 && x > y)
            {
                //�E�Ɉړ�
                direction = Vector3.right;
            }
            else if (flickValue_y > 50 && y > x)
            {
                //���Ɉړ�
                direction = Vector3.forward;
            }
            else if (flickValue_y < -50 && y > x)
            {
                //��O�Ɉړ�
                direction = Vector3.back;
            }

            if (direction != Vector3.zero)
            {
                DecreaseHand();

                //�����u���b�N�̃^�O���擾
                GameObject[] fireCube = GameObject.FindGameObjectsWithTag("fireCube");
                GameObject[] combicube = GameObject.FindGameObjectsWithTag("combicube");
                GameObject[] waterCube = GameObject.FindGameObjectsWithTag("waterCube");
                GameObject[] rockCube = GameObject.FindGameObjectsWithTag("RockCube");
                //GameObject[] Bronze = GameObject.FindGameObjectsWithTag("BronzeCube");

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

    //�u���b�N�𓮂����֐�
    private IEnumerator MoveBlock()
    {
        yield return new WaitForSeconds(1.0f);

        isMove = false;

        //�萔��0�ɂȂ�����Q�[���I�[�o�[
        if (hand <= 0 && !isCompleteClear)
        {
            isGameOver = true;
            GameOverText.SetActive(true);
            BackTitleButton.SetActive(true);
            helpButton.SetActive(false);
            backStageSelectButton.SetActive(true);
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

    //�^�C�g���ɖ߂�֐�
    public void FadeBackTitle()
    {
        Initiate.Fade("Title", Color.black, 1.0f);
        audioSource.PlayOneShot(TitleSE);
    }

    //���g���C����֐�
    public void Retry()
    {
        Addressables.LoadScene("Stage"+ CurrentStageNum);
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
            BackTitleButton.SetActive(true);
            helpButton.SetActive(false);
            backStageSelectButton.SetActive(true);
            NextStageButton.SetActive(true);
            NetworkManager.Instance.StageClear(CurrentStageNum);
            return;
        }
    }

    //�X�e�[�W�I����ʂɖ߂�
    public void FadeStageSelect()
    {
        Initiate.Fade("StageSelectScene", Color.black, 1.0f);
        audioSource.PlayOneShot(backStageSelectSE);
    }

    //���̃X�e�[�W�ɑJ�ڂ���
    public void FadeNextStage()
    {
        CurrentStageNum += 1;
        Initiate.Fade("Stage"+ CurrentStageNum, Color.black, 1.0f,true);
        audioSource.PlayOneShot(StageNextSE);
    }

    //���݂̃X�e�[�W�ԍ����X�V���郁�\�b�h
    static public void UpdateStageNum(int currentstagenum)
    {
        CurrentStageNum = currentstagenum;
        Initiate.Fade("Stage"+ CurrentStageNum, Color.black, 1.0f,true);
    }
}
