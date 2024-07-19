using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    [SerializeField] float moveDuration = 1f; // �ړ��ɂ����鎞��
    [SerializeField] float rayDistance = 10f; // Ray�̔򋗗�
    [SerializeField] LayerMask hitLayers; // ���C���[�}�X�N
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
            // �L���[�u�̌��݂̈ʒu
            Vector3 currentPosition = transform.position;

            // �������ւ�Ray���΂�
            Vector3 direction = Vector3.left;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Ray���q�b�g�����ʒu�Ɉړ�
                Vector3 targetPosition = hit.transform.position - direction;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            }
               
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // �L���[�u�̌��݂̈ʒu
            Vector3 currentPosition = transform.position;

            // �E�����ւ�Ray���΂�
            Vector3 direction = Vector3.right;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Ray���q�b�g�����ʒu�Ɉړ�
                Vector3 targetPosition = hit.transform.position - direction;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            };

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            // �L���[�u�̌��݂̈ʒu
            Vector3 currentPosition = transform.position;

            // �������ւ�Ray���΂�
            Vector3 direction = new Vector3(0,0,1);
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);
            RaycastHit hit;

            if (hits.Length >=1)
            {
                // Ray���q�b�g�����ʒu�Ɉړ�
                Vector3 targetPosition = hits[hits.Length-1].transform.position - direction * hits.Length;
                this.transform.GetComponent<Rigidbody>().DOMove(targetPosition, Vector3.Distance(currentPosition, targetPosition));
            }

            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // �L���[�u�̌��݂̈ʒu
            Vector3 currentPosition = transform.position;

            // ��O�����ւ�Ray���΂�
            Vector3 direction = new Vector3(0, 0, -1);
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);

            //RaycastHit hit;
            
            // Ray���q�b�g�����ʒu�Ɉړ�
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
