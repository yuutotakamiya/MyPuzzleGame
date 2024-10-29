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
    [SerializeField] Text handnumText;//�c��萔��\��
    [SerializeField] GameObject GameOverText;//�Q�[���I�[�o�[�e�L�X�g
    [SerializeField] GameObject GameClear;//�Q�[���N���A�e�L�X�g
    [SerializeField] GameObject BackTitleButton;//�^�C�g���ɖ߂�{�^��
    [SerializeField] GameObject CubeList;//�u���b�N�̐i�����X�g
    [SerializeField] GameObject backStageSelectButton;//�X�e�[�W�I����ʃ{�^��
    [SerializeField] GameObject NextStageButton;//���̃X�e�[�W��
    [SerializeField] Text MinHand;//�ŒZ�萔
    [SerializeField] Text Myhandnum;//�������g�̍ŒZ�萔
    [SerializeField] GameObject EndText;//�I���e�L�X�g
    [SerializeField] GameObject RetryBuuton;
    GameObject helpButton;//�w���v�{�^��

    //�ϐ��錾
    [SerializeField] int hand;//���݂̎c��萔
    [SerializeField] int TotalNum;//�t���[���̃g�[�^����
    [SerializeField] int CurrentNum;//���݂̂ǂ̂��炢���߂����ۑ����邽�߂̕ϐ�
    int useHandNum;
    int startHand;//�ő�萔
    public static int CurrentStageNum;//���݂̃X�e�[�W�ԍ�
    public static int landid;//����ID��ۑ�����ϐ�

    //���ʉ�
    [SerializeField] AudioClip TitleSE;//�^�C�g�����������Ƃ���SE
    [SerializeField] AudioClip StageNextSE;//���̃X�e�[�W�ɍs���{�^��������������SE
    [SerializeField] AudioClip backStageSelectSE;//�X�e�[�W�I����ʂɍs���Ƃ���SE

    //�t���O�ϐ�
    bool isMove = false;//�ړ������̃t���O
    bool isCompleteClear = false;//�N���A�������ǂ���
    bool isGameOver = false;//�Q�[���I�[�o�[�������̃t���O
    bool isMenu = false;//���j���[���J���Ă邩�ǂ���

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

        //�X�e�[�W10�ɂȂ����玟�̃X�e�[�W�{�^����false�ɂ���
        if (CurrentStageNum == 10)
        {
            NextStageButton.GetComponent<Button>().interactable = false;
        }

        audioSource = GetComponent<AudioSource>();

        //�X�e�[�W�̍ŒZ�萔
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


        //���g�̍ŒZ�萔�̌Ăяo��
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
        if (isMove || isCompleteClear || isGameOver || isMenu) return;//�ړ����܂��̓N���A�����ꍇ�͓��͂𖳎�����

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

            //GameOverText��null����Ȃ�������
            if (GameOverText != null)
            {
                GameOverText.SetActive(true);
            }
            //BackTitleButton��null����Ȃ�������
            else if (BackTitleButton != null)
            {
                BackTitleButton.SetActive(true);
            }
            //helpButton��null����Ȃ�������
            else if (helpButton != null)
            {
                helpButton.SetActive(false);
            }
            //backStageSelectButton��null����Ȃ�������
            else if (backStageSelectButton != null)
            {
                backStageSelectButton.SetActive(true);
            }

        }
        if (CurrentStageNum >= LandStageID.landStageID && hand <= 0 && !isCompleteClear)
        {
            //���̏󋵓o�^API�̌Ăяo��
            StartCoroutine(NetworkManager.Instance.Registland(landid, CurrentNum, result =>
            {

                isGameOver = true;
                //EndText��null����Ȃ�������
                if (EndText != null)
                {
                    EndText.SetActive(true);
                }
                //backStageSelectButton��null����Ȃ�������
                else if (backStageSelectButton != null)
                {
                    backStageSelectButton.SetActive(true);
                }
                //BackTitleButton��null����Ȃ�������
                else if (BackTitleButton != null)
                {
                    BackTitleButton.SetActive(true);
                }
                //GameOverText��null����Ȃ�������
                else if (GameOverText != null)
                {
                    GameOverText.SetActive(false);   
                }

            }));
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
            handnumText.text = (startHand - hand).ToString() + " (MAX:" + startHand + ")";
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
        Addressables.LoadScene("Stage" + CurrentStageNum);
    }

    //���݂̃t���[���̐����g�[�^���̃t���[���̐��������ɂȂ�����
    public void AddCurrentNum()
    {
        //���݂̃t���[�������J�E���g�A�b�v
        CurrentNum++;

        //�Q�[���N���A����
        if (CurrentNum == TotalNum)
        {
            useHandNum = (startHand - hand);

            //�X�e�[�W�N���A�̓o�^
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
                //���̏󋵓o�^
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
        Initiate.Fade("Stage" + CurrentStageNum, Color.black, 1.0f, true);
        audioSource.PlayOneShot(StageNextSE);
    }

    //���݂̃X�e�[�W�ԍ����X�V���郁�\�b�h
    static public void UpdateStageNum(int currentstagenum)
    {
        CurrentStageNum = currentstagenum;
        Initiate.Fade("Stage" + CurrentStageNum, Color.black, 1.0f, true);
    }

    //�����hID��ۑ����邽�߂̃��\�b�h
    static public void UpdateLandID(int id)
    {
        landid = id;
    }

    //LandID���擾���Ă��郁�\�b�h
    static public int GetLandID()
    {
        return landid;
    }

    //  ���j���[�{�^�����������Ƃ��̏���
    public void MenuButton()
    {
        //���݂̃X�e�[�W�ԍ���11�ȏゾ������
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
