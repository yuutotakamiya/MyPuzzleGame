using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    [SerializeField] Text TimeText;

    [SerializeField] Text handnum;//écÇËéËêî

    float Timer = 60;

    public void Awake()
    {
        SceneManager.LoadScene("UIScene",LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start()
    {
        //TimeText = GameObject.Find("Timelimit");

        //handnum = GameObject.Find("handnum");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTimerText()
    {
        TimeText.text = "Time: " + Mathf.Max(Timer, 0).ToString();
    }

    void UpdateTimer()
    {
        Timer -= Time.deltaTime;
        UpdateTimerText();
    }
}
