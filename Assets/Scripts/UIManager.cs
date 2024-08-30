using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text handnumText;//�c��萔

    [SerializeField] GameObject GameOverText;

    int hand = 10;


    void Awake()
    {
        SceneManager.LoadScene("Stage1",LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void handNum()
    {
        hand--;
        handnumText.text = "�c��萔:" + hand.ToString();
        if (hand > 0)
        {
            GameOverText.SetActive(true);
        }
    }
}
