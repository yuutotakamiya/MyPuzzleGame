using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader : MonoBehaviour
{

    [SerializeField] Slider loadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loading()
    {
        //カタログ更新
        var handle = Addressables.UpdateCatalogs();
        yield return handle;

        //ダウンロード実行
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync("default",false);

        //ダウンロード完了するまでスライダーのUI更新
        while (downloadHandle.Status == AsyncOperationStatus.None)
        {
            loadingSlider.value = downloadHandle.GetDownloadStatus().Percent * 100;
            yield return null;
        }

        loadingSlider.value = 100;
        Addressables.Release(downloadHandle);
        Addressables.Release(handle);

        //次のシーンへ移動
        loadFade();
       
    }
    public void loadFade()
    {
        Initiate.Fade("Stage1", Color.black, 1.0f);
    }

}
