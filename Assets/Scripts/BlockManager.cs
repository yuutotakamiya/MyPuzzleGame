using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;//Sortメソッドを使用するために必要

public class BlockManager : MonoBehaviour
{
    float moveDuration = 1f; // 移動にかかる時間
    [SerializeField] float rayDistance = 10f; // Rayの飛距離
    [SerializeField] LayerMask hitLayers; // レイヤーマスク

    Tween moveTween;

    Rigidbody rb;

    bool isCollision = false;

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

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
        rb = GetComponent<Rigidbody>();
     }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);


            flickValue_x = endTouchPos.x - startTouchPos.x;
            flickValue_y = endTouchPos.y - startTouchPos.y;

            Vector3 direction = Vector3.zero;
            if (flickValue_x  < -400)
            {
                direction = Vector3.left;
            }
            else if (flickValue_x > 400)
            {
                direction = Vector3.right;
            }
            else if (flickValue_y > 400)
            {
                direction = Vector3.forward;
            }
            else if (flickValue_y < -400)
            {
                direction = Vector3.back;
            }


            if (direction != Vector3.zero)
            {
                // キューブの現在の位置
                Vector3 currentPosition = transform.position;

                // 左方向へのRayを飛ばす

                Ray ray = new Ray(currentPosition, direction);

                //rayを飛ばしたオブジェクトを順番に取得
                RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                if (hits.Length >= 1)
                {
                    if(hits[0].collider.gameObject.tag == tag)
                    {
                        isCollision = true;
                    }
                    // Rayがヒットした位置に移動
                    TargetPosition = hits[hits.Length - 1].transform.position - direction * hits.Length;

                    //int movecount = 0;//自分のブロックと比較するブロックの変数
                    int count = 1;
                    string CmpTag = tag;

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (count == 0)
                        {
                            CmpTag = hits[i].collider.tag;
                        }
                        //ヒットしたオブジェクトが自身と同じタグを持つ場合の処理
                        if (hits[i].collider.gameObject.tag == CmpTag)
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

                            CmpTag = hits[i].collider.gameObject.tag;
                        }
                    }

                    //DOTweenを使って移動
                    this.transform.GetComponent<Rigidbody>().DOMove(TargetPosition, moveDuration);
                }
            }
        }
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("MyPuzzleScene");
    }

    
}
