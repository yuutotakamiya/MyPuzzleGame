using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    [SerializeField] float moveDuration = 1f; // 移動にかかる時間
    [SerializeField] float rayDistance = 10f; // Rayの飛距離
    [SerializeField] LayerMask hitLayers; // レイヤーマスク
    Tween moveTween;

    Rigidbody rb;

     void Start()
     {
        rb = GetComponent<Rigidbody>();
     }

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // キューブの現在の位置
            Vector3 currentPosition = transform.position;

            // 左方向へのRayを飛ばす
            Vector3 direction = Vector3.left;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Rayがヒットした位置に移動
                Vector3 targetPosition = hit.transform.position - direction;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            }
               
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // キューブの現在の位置
            Vector3 currentPosition = transform.position;

            // 右方向へのRayを飛ばす
            Vector3 direction = Vector3.right;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Rayがヒットした位置に移動
                Vector3 targetPosition = hit.transform.position - direction;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            };

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            // キューブの現在の位置
            Vector3 currentPosition = transform.position;

            // 奥方向へのRayを飛ばす
            Vector3 direction = new Vector3(0,0,1);
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);
            RaycastHit hit;

            if (hits.Length >=1)
            {
                // Rayがヒットした位置に移動
                Vector3 targetPosition = hits[hits.Length-1].transform.position - direction * hits.Length;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            }

            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // キューブの現在の位置
            Vector3 currentPosition = transform.position;

            // 手前方向へのRayを飛ばす
            Vector3 direction = new Vector3(0, 0, -1);
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);

            //RaycastHit hit;
            
            // Rayがヒットした位置に移動
            Vector3 targetPosition = hits[0-1].transform.position - direction;
            this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            
        }
    }
   

    public void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag == "targetObject")
        //{
            //this.GetComponent<Rigidbody>().DOKill();
        //}
        //DOTween.Kill(this.transform);
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("MyPuzzleScene");
    }
}
