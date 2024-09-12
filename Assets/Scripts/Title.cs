using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource  = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator CheckCatalog()
    {
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        var Updates = checkHandle.Result;

        Addressables.Release(checkHandle);

        if (Updates.Count >= 1)
        {
            Initiate.Fade("LoadScene", Color.black, 1.0f);
        }
        else
        {
            Initiate.Fade("StageSelectScene", Color.black, 1.0f);
        }
    }

    public void StartButtoon()
    {
        audioSource.PlayOneShot(audioClip);
        bool isSuccess = NetworkManager.Instance.LoadUserData();
        if (!isSuccess)
        {

            //ユーザーデータが保存されてない場合は登録
            StartCoroutine(NetworkManager.Instance.RegistUser(Guid.NewGuid().ToString(),
                result =>
                {
                    StartCoroutine(CheckCatalog());
                    
                }));
        }
        else
        {
            StartCoroutine(CheckCatalog());
        }
        
    }
}
