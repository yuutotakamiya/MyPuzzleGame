using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{

    [SerializeField] GameObject BGMSetting;
    bool isTouch = false;

    [SerializeField] Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButton);
    }


    public void OnButton()
    {
        if (!isTouch)
        {
            BGMSetting.SetActive(true);
        }
        else
        {
            BGMSetting.SetActive(false);
        }

        isTouch = !isTouch;
    }
}
