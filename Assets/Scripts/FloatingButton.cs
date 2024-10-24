using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingButton : MonoBehaviour
{

    [SerializeField] float floatSpeed = 1f; // �ӂ�ӂ킷��X�s�[�h
    [SerializeField] float scaleAmount = 0.1f; // �g��k���̕�

    private Vector3 originalScale;


    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale; // ���̃X�P�[����ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.Sin���g���ăX���[�Y�Ȋg��k�����J��Ԃ�
        float scale = 1 + Mathf.Sin(Time.time * floatSpeed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }


}
