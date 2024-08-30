using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;//Sortメソッドを使用するために必要
using UnityEngine.UI;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    float moveDuration = 1f; // 移動にかかる時間
    [SerializeField] float rayDistance = 10f; // Rayの飛距離
    [SerializeField] LayerMask hitLayers; // レイヤーマスク

    bool isCollision = false;

    Vector3 TargetPosition;

    //isCollisionプロパティ
    public bool IsCollision
    {
        get { return isCollision; }
    }

    //isTargetPositionのプロパティ
    public Vector3 VTargetPosition
    {
        get { return TargetPosition; }
    }

    void Start()
    {

    }


    public void Move(Vector3 direction, Action completeMove)
    {
         //キューブの現在の位置
         Vector3 currentPosition = transform.position;

        Ray ray = new Ray(currentPosition, direction);

        //rayを飛ばしたオブジェクトを順番に取得
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
            // Rayがヒットした位置に移動
            TargetPosition = raycastHitList[raycastHitList.Count - 1].transform.position - direction * raycastHitList.Count;

            int count = 1;
            string CmpTag = tag;

            for (int i = 0; i < raycastHitList.Count; i++)
            {
                if (count == 0)
                {
                    CmpTag = raycastHitList[i].collider.tag;
                }
                //ヒットしたオブジェクトが自身と同じタグを持つ場合の処理
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

            //DOTweenを使って移動
            this.transform.GetComponent<Rigidbody>().DOMove(TargetPosition, moveDuration).OnComplete(() =>
            {
                completeMove();
            });
        }
    }
}
