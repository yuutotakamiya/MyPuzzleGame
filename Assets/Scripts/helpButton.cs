using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpButton : MonoBehaviour
{

    [SerializeField] GameObject CubeList;
    // �{�^�����������Ƃ�true�A�������Ƃ�false�ɂȂ�t���O
    public bool buttonDownFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �{�^�����������Ƃ��̏���
    public void OnButtonDown()
    {
        CubeList.SetActive(true);
        buttonDownFlag = true;
    }

    // �{�^���𗣂����Ƃ��̏���
    public void OnButtonUp()
    {
        CubeList.SetActive(false);
        buttonDownFlag = false;
    }

}
