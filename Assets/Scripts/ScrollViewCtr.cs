using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewCtr : MonoBehaviour
{
    //for ScrollButton
    [SerializeField] Scrollbar _Scrollbar;

    //Coroutine�𒆒f���邽�߂ɕϐ��Ƃ��Đ錾���Ă���
    Coroutine _Coroutine;

    //�X�N���[���X�s�[�h
    [SerializeField] float ScrollSpeed;

    //LeftButton�ɓo�^
    public void ScrollLeft()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollLeft());
    }

    //LeftestButton�ɓo�^
    public void ScrollLeftest()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollLeft(300f));
    }

    //���X�N���[���R���[�`��
    IEnumerator TimeForScrollLeft(float SpeedRate = 1f)
    {
        while (_Scrollbar.value > 0)
        {
            _Scrollbar.value -= Time.deltaTime * ScrollSpeed * SpeedRate;
            yield return null;
        }
    }

    //Right�{�^���ɓo�^
    public void ScrollRight()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollRight());
    }

    //RightestButton�ɓo�^
    public void ScrollRightest()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        _Coroutine = StartCoroutine(TimeForScrollRight(300f));
    }

    //�E�X�N���[���R���[�`��
    IEnumerator TimeForScrollRight(float SpeedRate = 1f)
    {
        while (_Scrollbar.value < 1)
        {
            _Scrollbar.value += Time.deltaTime * ScrollSpeed * SpeedRate;
            yield return null;
        }
    }

    //EventTrigger��PointerUp�C�x���g�ɓo�^���鏈��
    public void StopScroll()
    {
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
    }
}
