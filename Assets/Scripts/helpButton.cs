using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpButton : MonoBehaviour
{

    [SerializeField] GameObject CubeList;
    // ボタンを押したときtrue、離したときfalseになるフラグ
    public bool buttonDownFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボタンを押したときの処理
    public void OnButtonDown()
    {
        CubeList.SetActive(true);
        buttonDownFlag = true;
    }

    // ボタンを離したときの処理
    public void OnButtonUp()
    {
        CubeList.SetActive(false);
        buttonDownFlag = false;
    }

}
