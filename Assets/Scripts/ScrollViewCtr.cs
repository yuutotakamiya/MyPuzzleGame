using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewCtr : MonoBehaviour
{
    //for ScrollButton
    [SerializeField] Scrollbar _Scrollbar;

    //Coroutineを中断するために変数として宣言しておく
    Coroutine _Coroutine;

    //スクロールスピード
    [SerializeField] float ScrollSpeed;

    //LeftButtonに登録
    public void ScrollLeft()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollLeft());
    }

    //LeftestButtonに登録
    public void ScrollLeftest()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollLeft(300f));
    }

    //左スクロールコルーチン
    IEnumerator TimeForScrollLeft(float SpeedRate = 1f)
    {
        while (_Scrollbar.value > 0)
        {
            _Scrollbar.value -= Time.deltaTime * ScrollSpeed * SpeedRate;
            yield return null;
        }
    }

    //Rightボタンに登録
    public void ScrollRight()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollRight());
    }

    //RightestButtonに登録
    public void ScrollRightest()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollRight(300f));
    }

    //右スクロールコルーチン
    IEnumerator TimeForScrollRight(float SpeedRate = 1f)
    {
        while (_Scrollbar.value < 1)
        {
            _Scrollbar.value += Time.deltaTime * ScrollSpeed * SpeedRate;
            yield return null;
        }
    }

    //EventTriggerのPointerUpイベントに登録する処理
    public void StopScroll()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
    }
}
