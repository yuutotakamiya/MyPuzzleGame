using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingButton : MonoBehaviour
{

    [SerializeField] float floatSpeed = 1f; // ふわふわするスピード
    [SerializeField] float scaleAmount = 0.1f; // 拡大縮小の幅

    private Vector3 originalScale;


    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale; // 元のスケールを保存
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.Sinを使ってスムーズな拡大縮小を繰り返す
        float scale = 1 + Mathf.Sin(Time.time * floatSpeed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }


}
