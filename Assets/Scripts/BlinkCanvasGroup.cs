using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlinkCanvasGroup : MonoBehaviour
{
    public float DurationSeconds;
    public Ease EaseType;

    private CanvasGroup canvasGroup;

    void Start()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.canvasGroup.DOFade(0.0f, this.DurationSeconds).SetEase(this.EaseType).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
