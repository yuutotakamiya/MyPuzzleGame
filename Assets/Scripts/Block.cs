using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;//Sort���\�b�h���g�p���邽�߂ɕK�v
using UnityEngine.UI;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    float moveDuration = 1f; // �ړ��ɂ����鎞��
    [SerializeField] float rayDistance = 10f; // Ray�̔򋗗�
    [SerializeField] LayerMask hitLayers; // ���C���[�}�X�N

    bool isCollision = false;

    Vector3 TargetPosition;

    //isCollision�v���p�e�B
    public bool IsCollision
    {
        get { return isCollision; }
    }

    //isTargetPosition�̃v���p�e�B
    public Vector3 VTargetPosition
    {
        get { return TargetPosition; }
    }

    void Start()
    {

    }


    public void Move(Vector3 direction, Action completeMove)
    {
         //�L���[�u�̌��݂̈ʒu
         Vector3 currentPosition = transform.position;

        Ray ray = new Ray(currentPosition, direction);

        //ray���΂����I�u�W�F�N�g�����ԂɎ擾
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        if (hits.Length >= 1)
        {
            List<RaycastHit> raycastHitList = new List<RaycastHit>();
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.tag == "targetObject")
                {
                    raycastHitList.Add(hits[i]);
                    break;
                }
                raycastHitList.Add(hits[i]);

            }
            if (raycastHitList[0].collider.gameObject.tag == tag)
            {
                isCollision = true;
            }
            // Ray���q�b�g�����ʒu�Ɉړ�
            TargetPosition = raycastHitList[raycastHitList.Count - 1].transform.position - direction * raycastHitList.Count;

            int count = 1;
            string CmpTag = tag;

            for (int i = 0; i < raycastHitList.Count; i++)
            {
                if (count == 0)
                {
                    CmpTag = raycastHitList[i].collider.tag;
                }
                //�q�b�g�����I�u�W�F�N�g�����g�Ɠ����^�O�����ꍇ�̏���
                if (raycastHitList[i].collider.gameObject.tag == CmpTag)
                {
                    count++;
                    if (count == 2)
                    {
                        count = 0;

                        TargetPosition += direction;
                    }
                }
                else
                {
                    count = 1;

                    CmpTag = raycastHitList[i].collider.gameObject.tag;
                }
            }

            //DOTween���g���Ĉړ�
            this.transform.GetComponent<Rigidbody>().DOMove(TargetPosition, moveDuration).OnComplete(() =>
            {
                completeMove();
            });
        }
    }
}
